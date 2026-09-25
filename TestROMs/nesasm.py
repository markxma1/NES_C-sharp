#!/usr/bin/env python3
"""Tiny 6502 assembler + iNES writer used to build the self-written test ROMs.

Supports the official opcodes the test programs need, labels, `.org`, `.byte`,
`.word`, `.fill` and simple `label+N` / `<label` / `>label` expressions.
Everything the tests build is original code (see tests/roms/README.md), so the
ROMs can live in the repository without any copyright issue.
"""
import re
import struct

# mnemonic -> {mode: opcode}.  Modes: imp, acc, imm, zp, zpx, abs, abx, aby, izy, rel, ind
OPS = {
    'LDA': {'imm': 0xA9, 'zp': 0xA5, 'zpx': 0xB5, 'abs': 0xAD, 'abx': 0xBD, 'aby': 0xB9, 'izy': 0xB1},
    'LDX': {'imm': 0xA2, 'zp': 0xA6, 'abs': 0xAE, 'aby': 0xBE},
    'LDY': {'imm': 0xA0, 'zp': 0xA4, 'abs': 0xAC, 'abx': 0xBC},
    'STA': {'zp': 0x85, 'zpx': 0x95, 'abs': 0x8D, 'abx': 0x9D, 'aby': 0x99, 'izy': 0x91},
    'STX': {'zp': 0x86, 'abs': 0x8E},
    'STY': {'zp': 0x84, 'abs': 0x8C},
    'ADC': {'imm': 0x69, 'zp': 0x65, 'abs': 0x6D},
    'SBC': {'imm': 0xE9, 'zp': 0xE5, 'abs': 0xED},
    'AND': {'imm': 0x29, 'zp': 0x25, 'abs': 0x2D},
    'ORA': {'imm': 0x09, 'zp': 0x05, 'abs': 0x0D},
    'EOR': {'imm': 0x49, 'zp': 0x45, 'abs': 0x4D},
    'CMP': {'imm': 0xC9, 'zp': 0xC5, 'abs': 0xCD, 'abx': 0xDD},
    'CPX': {'imm': 0xE0, 'zp': 0xE4, 'abs': 0xEC},
    'CPY': {'imm': 0xC0, 'zp': 0xC4, 'abs': 0xCC},
    'BIT': {'zp': 0x24, 'abs': 0x2C},
    'INC': {'zp': 0xE6, 'abs': 0xEE},
    'DEC': {'zp': 0xC6, 'abs': 0xCE},
    'ASL': {'acc': 0x0A, 'zp': 0x06}, 'LSR': {'acc': 0x4A, 'zp': 0x46},
    'ROL': {'acc': 0x2A, 'zp': 0x26}, 'ROR': {'acc': 0x6A, 'zp': 0x66},
    'JMP': {'abs': 0x4C, 'ind': 0x6C}, 'JSR': {'abs': 0x20},
    'BEQ': {'rel': 0xF0}, 'BNE': {'rel': 0xD0}, 'BCC': {'rel': 0x90}, 'BCS': {'rel': 0xB0},
    'BPL': {'rel': 0x10}, 'BMI': {'rel': 0x30}, 'BVC': {'rel': 0x50}, 'BVS': {'rel': 0x70},
    'INX': {'imp': 0xE8}, 'INY': {'imp': 0xC8}, 'DEX': {'imp': 0xCA}, 'DEY': {'imp': 0x88},
    'TAX': {'imp': 0xAA}, 'TXA': {'imp': 0x8A}, 'TAY': {'imp': 0xA8}, 'TYA': {'imp': 0x98},
    'TXS': {'imp': 0x9A}, 'TSX': {'imp': 0xBA},
    'PHA': {'imp': 0x48}, 'PLA': {'imp': 0x68}, 'PHP': {'imp': 0x08}, 'PLP': {'imp': 0x28},
    'RTS': {'imp': 0x60}, 'RTI': {'imp': 0x40}, 'NOP': {'imp': 0xEA},
    'CLC': {'imp': 0x18}, 'SEC': {'imp': 0x38}, 'CLI': {'imp': 0x58}, 'SEI': {'imp': 0x78},
    'CLD': {'imp': 0xD8}, 'CLV': {'imp': 0xB8}, 'SED': {'imp': 0xF8},
}
BRANCHES = {m for m, v in OPS.items() if 'rel' in v}


def _value(expr, labels, allow_missing):
    expr = expr.strip()
    m = re.fullmatch(r'([<>])(.+)', expr)
    if m:
        v = _value(m.group(2), labels, allow_missing)
        return (v & 0xFF) if m.group(1) == '<' else (v >> 8) & 0xFF
    m = re.fullmatch(r'(.+?)\s*([+-])\s*(\d+|\$[0-9A-Fa-f]+)', expr)
    if m and not expr.startswith('$') and not expr.isdigit():
        base = _value(m.group(1), labels, allow_missing)
        off = _value(m.group(3), labels, allow_missing)
        return base + off if m.group(2) == '+' else base - off
    if expr.startswith('$'):
        return int(expr[1:], 16)
    if expr.startswith('%'):
        return int(expr[1:], 2)
    if re.fullmatch(r'\d+', expr):
        return int(expr)
    if expr in labels:
        return labels[expr]
    if allow_missing:
        return 0
    raise ValueError('unknown symbol: ' + expr)


def _mode(mnemonic, operand):
    if operand is None:
        return 'acc' if 'acc' in OPS[mnemonic] and 'imp' not in OPS[mnemonic] else 'imp'
    op = operand.strip()
    if mnemonic in BRANCHES:
        return 'rel'
    if op == 'A':
        return 'acc'
    if op.startswith('#'):
        return 'imm'
    if re.fullmatch(r'\((.+)\),\s*[Yy]', op):
        return 'izy'
    if re.fullmatch(r'\((.+)\)', op):
        return 'ind'
    m = re.fullmatch(r'(.+),\s*([XxYy])', op)
    if m:
        return ('abx' if m.group(2) in 'Xx' else 'aby')  # resolved to zpx below if it fits
    return 'abs'


def assemble(source, org_default=0x8000):
    lines = []
    for raw in source.splitlines():
        line = raw.split(';')[0].strip()
        if line:
            lines.append(line)

    consts = {}

    def run(pass_labels, final):
        pc = org_default
        out = {}          # address -> byte
        labels = {}
        for line in lines:
            mc = re.match(r'^([A-Za-z_][A-Za-z0-9_]*)\s*=\s*(.+)$', line)
            if mc:
                consts[mc.group(1)] = _value(mc.group(2), {**pass_labels, **consts}, True)
                continue
            while True:
                m = re.match(r'^([A-Za-z_][A-Za-z0-9_]*):\s*(.*)$', line)
                if not m:
                    break
                labels[m.group(1)] = pc
                line = m.group(2)
            if not line:
                continue
            allow = not final
            lab = {**pass_labels, **consts, **labels} if final else {**pass_labels, **consts}
            if line.startswith('.org'):
                pc = _value(line.split(None, 1)[1], lab, allow)
                continue
            if line.startswith('.byte'):
                for tok in line.split(None, 1)[1].split(','):
                    out[pc] = _value(tok, lab, allow) & 0xFF
                    pc += 1
                continue
            if line.startswith('.word'):
                for tok in line.split(None, 1)[1].split(','):
                    v = _value(tok, lab, allow)
                    out[pc] = v & 0xFF
                    out[pc + 1] = (v >> 8) & 0xFF
                    pc += 2
                continue
            if line.startswith('.fill'):
                n, v = [t.strip() for t in line.split(None, 1)[1].split(',')]
                for _ in range(_value(n, lab, allow)):
                    out[pc] = _value(v, lab, allow) & 0xFF
                    pc += 1
                continue
            parts = line.split(None, 1)
            mn = parts[0].upper()
            operand = parts[1] if len(parts) > 1 else None
            if mn not in OPS:
                raise ValueError('unknown mnemonic: ' + line)
            mode = _mode(mn, operand)
            if mode == 'imm':
                val = _value(operand.strip()[1:], lab, allow)
            elif mode in ('acc', 'imp'):
                val = None
            elif mode in ('abx', 'aby', 'izy', 'ind'):
                inner = re.sub(r'^\((.+)\)(,\s*[Yy])?$', r'\1', operand.strip()) if mode in ('izy', 'ind') else operand.split(',')[0]
                val = _value(inner, lab, allow)
                if mode == 'abx' and 'zpx' in OPS[mn] and val < 0x100 and (re.fullmatch(r'\$[0-9A-Fa-f]{1,3}|\d+', inner.strip()) or inner.strip() in consts):
                    mode = 'zpx'
            else:
                val = _value(operand, lab, allow)
                sym = operand.strip()
                literal = re.fullmatch(r'\$[0-9A-Fa-f]{1,3}|%[01]+|\d+', sym) is not None
                if 'zp' in OPS[mn] and val < 0x100 and (literal or sym in consts):
                    mode = 'zp'
            if mode not in OPS[mn]:
                if mode == 'zp' and 'abs' in OPS[mn]:
                    mode = 'abs'
                else:
                    raise ValueError(f'{mn} does not support mode {mode}: {line}')
            out[pc] = OPS[mn][mode]
            if mode == 'rel':
                out[pc + 1] = (val - (pc + 2)) & 0xFF if final else 0
                if final and not -128 <= val - (pc + 2) <= 127:
                    raise ValueError('branch out of range: ' + line)
                pc += 2
            elif mode in ('imm', 'zp', 'zpx', 'izy'):
                out[pc + 1] = val & 0xFF
                pc += 2
            elif mode in ('abs', 'abx', 'aby', 'ind'):
                out[pc + 1] = val & 0xFF
                out[pc + 2] = (val >> 8) & 0xFF
                pc += 3
            else:
                pc += 1
        return out, labels

    _, labels1 = run({}, False)
    out, labels = run(labels1, True)
    return out, labels


def build_ines(prg_image, chr_data, mapper=0, mirroring=1, prg_base=0x8000):
    """prg_image: {address: byte}. PRG size is rounded up to 16 KB units."""
    size = 16384
    top = max(prg_image) - prg_base + 1
    while size < top:
        size *= 2
    prg = bytearray([0xFF] * size)
    for addr, b in prg_image.items():
        prg[addr - prg_base] = b
    header = b'NES\x1a' + bytes([size // 16384, len(chr_data) // 8192,
                                  (mapper & 0x0F) << 4 | (mirroring & 1), mapper & 0xF0]) + bytes(8)
    return header + bytes(prg) + bytes(chr_data)
