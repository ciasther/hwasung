#!/usr/bin/env python3
"""Show that the manual's status commands do not answer over USB.

This is kept as evidence, not as something you need for normal use. It sends
every status command that the HMK-072 manual documents on pages 79 and 80 over
the USB interface and prints, for each one, either the reply or the exception.
The point is to demonstrate that this USB board never returns the documented
real-time status, whatever the paper state.

Run it once with paper loaded and once with paper removed and compare. The
results are the same both times, which is the whole problem: over USB these
commands give you nothing. Use read_paper_status.py instead.

Pass a label as the first argument so the two runs can be told apart, for
example:

    python3 escpos_status_probe.py PAPER-PRESENT
    python3 escpos_status_probe.py PAPER-OUT

This only reads status. It does not print.

Needs pyusb (pip install pyusb).
"""
import sys
import time

import usb.core
import usb.util

VID = 0x0006
PID = 0x000b
OUT_EP = 0x01
IN_EP = 0x83
READ_LEN = 64
TIMEOUT_MS = 500

# Every status command from the manual, with a short note on what it should do.
MANUAL_COMMANDS = [
    ("DLE EOT n=1 (printer status)", [0x10, 0x04, 0x01], "real-time printer status"),
    ("DLE EOT n=2 (paper-roll status)", [0x10, 0x04, 0x02], "bit0 no paper, bit3 paper low"),
    ("DLE EOT n=3 (error status)", [0x10, 0x04, 0x03], "real-time error status"),
    ("DLE EOT n=4 (paper-sensor status)", [0x10, 0x04, 0x04], "real-time paper-sensor status"),
    ("GS r n=1 (paper-sensor status)", [0x1D, 0x72, 0x01], "paper end and near-end bits"),
    ("GS r n=2 (drawer-kick status)", [0x1D, 0x72, 0x02], "drawer and sensor status"),
]


def main():
    label = sys.argv[1] if len(sys.argv) > 1 else "UNLABELED"

    dev = usb.core.find(idVendor=VID, idProduct=PID)
    if dev is None:
        print("Printer %04x:%04x not found" % (VID, PID))
        return 1

    try:
        if dev.is_kernel_driver_active(0):
            dev.detach_kernel_driver(0)
    except usb.core.USBError as e:
        print("Could not detach kernel driver:", e)

    try:
        dev.set_configuration()
    except usb.core.USBError:
        pass

    print("HMK-072 manual status commands over USB, run [%s]" % label)
    print("device %04x:%04x found" % (VID, PID))
    print()

    # First show the endpoints, so it is clear the USB layer itself is healthy.
    cfg = dev.get_active_configuration()
    intf = cfg[(0, 0)]
    print("interface class %d (7 means printer), subclass %d, protocol %d"
          % (intf.bInterfaceClass, intf.bInterfaceSubClass, intf.bInterfaceProtocol))
    for ep in intf:
        direction = "IN " if (ep.bEndpointAddress & 0x80) else "OUT"
        print("  endpoint 0x%02x %s" % (ep.bEndpointAddress, direction))
    print()

    # The USB control path works: identity readback succeeds. So the printer is
    # alive and talking, it just will not answer the ESC/POS status commands.
    try:
        raw = dev.ctrl_transfer(0xA1, 0, 0, 0, 1024)
        ident = bytes(raw[2:]).decode("ascii", "replace").strip()
        print("device id (USB control readback works): %s" % ident)
    except usb.core.USBError as e:
        print("device id readback failed:", e)
    print()

    # Now the manual's status commands, one by one. Every one times out.
    for name, cmd, note in MANUAL_COMMANDS:
        hexcmd = " ".join("%02X" % b for b in cmd)
        print("%s  [%s]" % (name, hexcmd))
        print("  expected: %s" % note)
        try:
            dev.write(OUT_EP, bytes(cmd))
        except usb.core.USBError as e:
            print("  write failed:", e)
            print()
            continue
        try:
            data = dev.read(IN_EP, READ_LEN, TIMEOUT_MS)
            print("  reply: %s" % list(data))
        except usb.core.USBError as e:
            print("  no reply: %s" % e)
        print()

    print("done")
    return 0


if __name__ == "__main__":
    sys.exit(main())
