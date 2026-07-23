# Reading paper and error status over USB

This is the part that caused the most trouble, so it comes first.

## The problem

The HMK-072 manual lists real-time status commands on pages 79 and 80. Those
are the ESC/POS `DLE EOT` commands (bytes `10 04 n`) and the `GS r` commands
(bytes `1D 72 n`). On paper they return a status byte that tells you if the
paper is out or nearly out.

Over the USB interface none of them answer. You send the command, you try to
read the reply, and the read times out. It behaves the same whether paper is
loaded or removed, so you get no useful information. Those commands only work
on the printer's RS-232 serial port.

We kept a script that demonstrates this, `scripts/escpos_status_probe.py`, and
its output in `scripts/samples/manual-commands-timeout.txt`. You do not need it
for normal work, it is only there as proof.

## What actually works

Over USB the printer answers a different command that is not in the manual. We
found it inside Hwasung's own SDKs (the Android SDK and the Windows
`HwaUSB.dll`) where it is built byte for byte, and then confirmed it on the
hardware.

The command is six bytes:

```
10 AA 55 80 54 AB
```

The procedure is:

1. Write those six bytes to the bulk OUT endpoint `0x01`.
2. Wait about one millisecond.
3. Read one byte from the bulk IN endpoint `0x83`.

That one byte is the status. A short read timeout is fine because the reply is
almost immediate.

## How to read the status byte

The byte is a set of flags. Check it bit by bit:

- bit 0 set: no paper
- bit 1 set: cover or print head is open
- bit 2 set: paper jam
- bit 3 set: paper is low (near the end of the roll)
- bit 5 set: cutter jam
- bit 7 set: paper printed but not taken

A value of `0x00` means everything is fine.

For example `0x09` is bit 0 plus bit 3, which means paper out and paper low at
the same time. That is exactly what you get when the roll is removed.

## What we measured on the unit

- Paper loaded, the reply is `0x00`, and it comes back in under a millisecond.
- Paper removed, the reply is `0x09`, and it comes back in about 15 to 30 ms.

So paper-out and paper-low are both detectable over USB with this one command.
You do not need to wire up the serial port.

## USB details you may need

- Vendor id `0x0006`, product id `0x000b`.
- Interface 0. On Linux you may have to detach the kernel `usblp` driver from
  it first, then claim it.
- Bulk OUT is endpoint address `0x01` (the first OUT endpoint on interface 0).
- Bulk IN is endpoint address `0x83` (the IN endpoint at index 2 on interface
  0).
- Read length is one byte. Keep the timeout short.

## A note for the supplier

This USB status command is not documented anywhere in the HMK-072 manual, which
only documents the serial `DLE EOT` and `GS r` commands. If you talk to
Hwasung, it is worth asking them to document the USB command officially,
confirm the full meaning of every bit, and confirm whether the same command is
used across the whole HMK series.
