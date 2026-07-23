#!/usr/bin/env python3
"""Read the paper and error status of a Hwasung HMK-072 over USB.

The manual's status commands (ESC/POS DLE EOT and GS r) do not answer over the
USB interface, only over RS-232. Over USB the printer answers a different,
6-byte command that is not in the manual:

    10 AA 55 80 54 AB

We recovered it from Hwasung's own Android and Windows SDKs and confirmed it on
the hardware. The flow is: write the 6 bytes to bulk OUT, wait about 1 ms, read
one status byte from bulk IN.

The reply is one byte of flags:
    bit 0  no paper
    bit 1  cover or head open
    bit 2  paper jam
    bit 3  paper low (near end)
    bit 5  cutter jam
    bit 7  paper printed but not taken
A value of 0x00 means everything is fine.

This only reads status. It does not print and does not change any setting.

Needs pyusb (pip install pyusb).
"""
import sys
import time

import usb.core

VID = 0x0006
PID = 0x000b
OUT_EP = 0x01
IN_EP = 0x83
STATUS_CMD = bytes([0x10, 0xAA, 0x55, 0x80, 0x54, 0xAB])


def decode(b):
    if b == 0:
        return "OK (paper loaded, no errors)"
    flags = []
    if b & 0x01:
        flags.append("no paper")
    if b & 0x02:
        flags.append("cover or head open")
    if b & 0x04:
        flags.append("paper jam")
    if b & 0x08:
        flags.append("paper low")
    if b & 0x20:
        flags.append("cutter jam")
    if b & 0x80:
        flags.append("paper not taken")
    return ", ".join(flags) if flags else "unknown flag 0x%02x" % b


def main():
    dev = usb.core.find(idVendor=VID, idProduct=PID)
    if dev is None:
        print("Printer %04x:%04x not found" % (VID, PID))
        return 1

    # On Linux the usblp kernel driver usually holds interface 0. Take it back.
    try:
        if dev.is_kernel_driver_active(0):
            dev.detach_kernel_driver(0)
    except usb.core.USBError as e:
        print("Could not detach kernel driver:", e)

    try:
        dev.set_configuration()
    except usb.core.USBError:
        # Already configured, this is fine.
        pass

    dev.write(OUT_EP, STATUS_CMD)
    time.sleep(0.001)

    started = time.time()
    reply = dev.read(IN_EP, 1, 500)
    elapsed_ms = (time.time() - started) * 1000

    status = reply[0]
    print("status byte : 0x%02x" % status)
    print("meaning     : %s" % decode(status))
    print("reply time  : %.0f ms" % elapsed_ms)
    return 0


if __name__ == "__main__":
    sys.exit(main())
