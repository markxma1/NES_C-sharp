#!/usr/bin/env python3
"""Builds the self-written test ROMs (tests/roms/*.nes).

Every ROM is original code assembled by nesasm.py, so it can be shipped with the
repository. Each one exercises a behaviour that once broke this emulator and
stores its measurements in RAM at $0300+ (and/or draws them on screen); the
expected values come from a reference emulator run, see README.md.
"""
import os
import sys

sys.path.insert(0, os.path.dirname(__file__))
from nesasm import assemble, build_ines  # noqa: E402

HERE = os.path.dirname(os.path.abspath(__file__))

# Shared start-up: reset, wait for the PPU to warm up (two vblanks), no rendering.
BOOT = """
reset:  sei
        cld
        ldx #$40
        stx $4017
        ldx #$FF
        txs
        inx
        stx $2000
        stx $2001
        stx $4010
        bit $2002
w1:     bit $2002
        bpl w1
        lda #0
        tax
clr:    sta $0000,x
        sta $0100,x
        sta $0300,x
        sta $0400,x
        sta $0500,x
        sta $0600,x
        sta $0700,x
        inx
        bne clr
w2:     bit $2002
        bpl w2
"""

VECTORS = """
.org $FFFA
.word nmi, reset, irq
"""


def solid_tile(plane0, plane1):
    """8x8 tile, every row = plane0/plane1 byte."""
    return bytes([plane0] * 8 + [plane1] * 8)


def make_chr(pages=None, size=8192):
    chr_data = bytearray(size)
    for (tile, data) in (pages or []):
        chr_data[tile * 16:tile * 16 + 16] = data
    return chr_data


def write_rom(name, source, chr_data, mapper=0, mirroring=1, base=0x8000):
    image, labels = assemble(source, org_default=base)
    with open(os.path.join(HERE, name + '.nes'), 'wb') as f:
        f.write(build_ines(image, chr_data, mapper=mapper, mirroring=mirroring))
    print('built', name + '.nes')


# ---------------------------------------------------------------------------
# 1. Controller port: strobe, read order, open bus bits, reads after 8 bits.
# ---------------------------------------------------------------------------
CTRL = BOOT + """
        lda #$80
        sta $2000
main:   jmp main
nmi:    pha
        txa
        pha
        lda #1
        sta $4016
        lda #0
        sta $4016
        ldx #0
r1:     lda $4016
        sta $0300,x
        inx
        cpx #12
        bne r1
        ldx #0
r2:     lda $4017
        sta $0310,x
        inx
        cpx #4
        bne r2
        inc $03F0
        pla
        tax
        pla
        rti
irq:    rti
""" + VECTORS

# ---------------------------------------------------------------------------
# 2. NMI/I flag: RTI restores I; an NMI raised inside a handler nests.
# ---------------------------------------------------------------------------
NMIFLAGS = BOOT + """
        lda #$80
        sta $2000
        cli
main:   php
        pla
        and #$04
        sta $0301          ; I flag as seen by the main program (clear)
        jmp main
nmi:    pha
        txa
        pha
        ldx $03F2
        cpx #2
        bcs noent
        lda $0313
        sta $0320,x        ; marker seen when this handler call started
noent:  php
        pla
        and #$04           ; keep only the I flag
        sta $0300          ; I flag inside the handler (set)
        inc $03F0          ; NMI count (counters live at $03F0+, not compared)
        lda $0311
        bne done
        inc $0311
        lda #0
        sta $2000
        lda #$80
        sta $2000          ; 0->1 during vblank raises another NMI at once
        nop
        nop
        lda #$99
        sta $0313          ; reached only after the nested handler returned
done:   inc $03F2
        pla
        tax
        pla
        rti
irq:    rti
""" + VECTORS

# ---------------------------------------------------------------------------
# 3. PPU palette mirrors, backdrop colour and the left-8-pixels mask.
#    A held on the pad selects $2001=$08 (left column hidden) instead of $0A.
# ---------------------------------------------------------------------------
PALETTE = BOOT + """
        ; --- palette: write the backdrop through the $3F10 mirror
        lda #$3F
        sta $2006
        lda #$10
        sta $2006
        lda #$21          ; blue backdrop via $3F10
        sta $2007
        lda #$16
        sta $2007         ; $3F11
        lda #$3F
        sta $2006
        lda #$14
        sta $2006
        lda #$27          ; $3F14 mirrors $3F04
        sta $2007
        ; read back $3F00 and $3F04 (palette reads are not buffered)
        lda #$3F
        sta $2006
        lda #$00
        sta $2006
        lda $2007
        sta $0300
        lda #$3F
        sta $2006
        lda #$04
        sta $2006
        lda $2007
        sta $0301
        ; --- background palette 0 colours 1..3
        lda #$3F
        sta $2006
        lda #$01
        sta $2006
        lda #$16
        sta $2007
        lda #$30
        sta $2007
        lda #$0F          ; colour 3 = black (must stay distinct from the blue backdrop)
        sta $2007
        ; --- nametable 0: alternate tile 1 / tile 2 columns, rest blank tile 0
        lda #$20
        sta $2006
        lda #$00
        sta $2006
        ldy #30
row:    ldx #32
col:    txa
        and #$07
        cmp #$07
        bne notc3
        lda #3
        bne put
notc3:  cmp #$03
        bcs blank
        and #$01
        clc
        adc #1
        bne put
blank:  lda #0
put:    sta $2007
        dex
        bne col
        dey
        bne row
        lda #$00
        sta $2005
        sta $2005
        lda #$80
        sta $2000
main:   jmp main
nmi:    pha
        lda #1
        sta $4016
        lda #0
        sta $4016
        lda $4016
        and #1
        beq nomask
        lda #$08
        bne setm
nomask: lda #$0A
setm:   sta $2001
        lda #0
        sta $2005
        sta $2005
        inc $03F0
        pla
        rti
irq:    rti
""" + VECTORS

# ---------------------------------------------------------------------------
# 4. MMC3 status-bar split: scanline IRQ, CHR bank switch + $2006 reload.
# ---------------------------------------------------------------------------
SPLIT_CODE = """
.org $C000
""" + BOOT.replace("reset:", "reset:") + """
        lda #1            ; MMC3 mirroring: horizontal, so $2800 is not a copy of $2000
        sta $A000
        ; palette
        lda #$3F
        sta $2006
        lda #$00
        sta $2006
        lda #$0F
        sta $2007
        lda #$16          ; colour 1 red
        sta $2007
        lda #$21          ; colour 2 blue
        sta $2007
        lda #$2A          ; colour 3 green
        sta $2007
        ; nametable 0: rows 0-23 tile 1, rows 24-29 tile 2
        lda #$20
        sta $2006
        lda #$00
        sta $2006
        ldx #0
        ldy #24
n0a:    ldx #32
n0b:    lda #1
        sta $2007
        dex
        bne n0b
        dey
        bne n0a
        ldy #6
n0c:    ldx #32
n0d:    lda #2
        sta $2007
        dex
        bne n0d
        dey
        bne n0c
        ; nametable 2 ($2800): all tile 2 (used after the split)
        lda #$28
        sta $2006
        lda #$00
        sta $2006
        ldy #30
        lda #1
        sta $05
n2a:    ldx #32
n2b:    lda $05
        sta $2007
        dex
        bne n2b
        lda $05
        eor #3             ; alternate tile 1 / tile 2 per row
        sta $05
        dey
        bne n2a
        lda #$80
        sta $2000
        lda #$0A          ; background on, left column shown
        sta $2001
        cli
main:   jmp main

nmi:    pha
        txa
        pha
        ; game CHR banks: R0 (2 KB at $0000) = page 0, R1 = page 2
        lda #0
        sta $8000
        lda #0
        sta $8001
        lda #1
        sta $8000
        lda #2
        sta $8001
        lda #$00
        sta $2006
        sta $2006
        lda #0
        sta $2005
        sta $2005
        lda #$80
        sta $2000
        ; arm the scanline IRQ: fires at line 100
        sta $E000
        lda #99
        sta $C000
        sta $C001
        sta $E001
        inc $03F0
        pla
        tax
        pla
        rti

irq:    pha
        sta $E000          ; acknowledge
        ; switch CHR bank R0 to page 4 (different tile 1/2 look)
        lda #0
        sta $8000
        lda #4
        sta $8001
        ; load v = $0880: nametable 2, coarse Y = 4 (double $2006 write)
        lda #$08
        sta $2006
        lda #$80
        sta $2006
        lda #0
        sta $2005
        sta $2005
        inc $03F1
        pla
        rti
""" + VECTORS

# ---------------------------------------------------------------------------
# 5. CPU timing: loop iterations between two vblanks for cycle-exact variants.
# ---------------------------------------------------------------------------
def timing_source():
    src = BOOT + """
        ; each variant: sync to vblank, count loop iterations until the next
        ; vblank flag, store the 16-bit count at $0300 + 2*variant
        ldy #0
next:   bit $2002
sync:   bit $2002
        bpl sync
        lda #0
        sta $10
        sta $11
        ldx #1              ; X used by the page-cross variant
        lda #2              ; page for the OAM DMA variant
        jsr dispatch
        tya
        asl A
        tax
        lda $10
        sta $0300,x
        lda $11
        sta $0301,x
        iny
        cpy #5
        bne next
        inc $03F0
        jmp forever
forever: jmp forever
dispatch: cpy #0
        bne d1
        jmp v0
d1:     cpy #1
        bne d2
        jmp v1
d2:     cpy #2
        bne d3
        jmp v2
d3:     cpy #3
        bne d4
        jmp v3
d4:     jmp v4
"""
    # v0: baseline loop (nop body)
    src += """
v0:     inc $10
        bne v0c
        inc $11
v0c:    nop
        bit $2002
        bpl v0
        rts
"""
    # v1: taken branch, same page
    src += """
v1:     inc $10
        bne v1c
        inc $11
v1c:    clc
        bcc v1d
v1d:    bit $2002
        bpl v1
        rts
"""
    # v2: taken branch whose target lies on another page than the next
    # instruction ($85FD/$85FE -> next pc $85FF, target $8602)
    src += """
.org $85F0
v2:     inc $10
        bne v2c
        inc $11
v2c:    bit $2002
        bmi v2e
        clc
        nop
        bcc v2far
v2e:    rts
.org $8602
v2far:  jmp v2
"""
    # v3: LDA abs,X crossing a page vs not (X=1 -> $80FF+1 crosses)
    src += """
v3:     inc $10
        bne v3c
        inc $11
v3c:    lda $80FF,x
        bit $2002
        bpl v3
        rts
"""
    # v4: OAM DMA every iteration (513/514 cycle CPU stall)
    src += """
v4:     inc $10
        bne v4c
        inc $11
v4c:    sta $4014
        bit $2002
        bpl v4
        rts
nmi:    rti
irq:    rti
""" + VECTORS
    return src


def main():
    tile_solid1 = (1, solid_tile(0xFF, 0x00))
    tile_solid2 = (2, solid_tile(0x00, 0xFF))
    write_rom('ctrl_read', CTRL, make_chr())
    write_rom('nmi_flags', NMIFLAGS, make_chr())
    write_rom('ppu_palette', PALETTE, make_chr([tile_solid1, tile_solid2, (3, solid_tile(0xFF, 0xFF))]))

    # MMC3: 16 KB CHR = 16 pages of 1 KB. Page 0 and 2: red/blue solids; page 4:
    # tile 1 = colour 3 (both planes), tile 2 = colour 1 - a visibly different set.
    chr_split = bytearray(16384)
    for page, t1, t2 in ((0, solid_tile(0xFF, 0x00), solid_tile(0x00, 0xFF)),
                         (2, solid_tile(0xFF, 0x00), solid_tile(0x00, 0xFF)),
                         (4, solid_tile(0xFF, 0xFF), solid_tile(0xFF, 0x00))):
        chr_split[page * 1024 + 16:page * 1024 + 32] = t1
        chr_split[page * 1024 + 32:page * 1024 + 48] = t2
    write_rom('mmc3_split', SPLIT_CODE, chr_split, mapper=4, mirroring=0, base=0xC000)
    write_rom('timing', timing_source(), make_chr())


if __name__ == '__main__':
    main()
