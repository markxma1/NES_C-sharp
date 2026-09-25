# Test ROMs

Small NES programs written from scratch for testing emulators (no commercial
data, so they can be shared). They were built for the C++ rewrite of this
emulator, https://github.com/markxma1/nes-emu-cpp - see `tests/roms` there for
how they are verified against a reference emulator.

This C# version is kept as it was, **including its known bugs**; running these
ROMs is a quick way to see them: controller reads, NMI/I-flag handling, PPU
palette mirroring and left-column mask, MMC3 scanline IRQ with a status-bar
split, and CPU cycle counts.

| ROM | Checks |
|---|---|
| `ctrl_read.nes` | controller strobe/read order/open bus (RAM `$0300+`) |
| `nmi_flags.nes` | RTI restores the I flag, nested NMI (RAM `$0300+`) |
| `ppu_palette.nes` | `$3F10` mirrors `$3F00`, backdrop colour, left-8-pixel mask (hold A) |
| `mmc3_split.nes` | MMC3 IRQ, CHR bank switch and mid-frame `$2006` reload |
| `timing.nes` | cycle counts of branches, page crossings and OAM DMA (RAM `$0300+`) |

Rebuild with `python3 build_roms.py`.
