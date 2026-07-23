# Hwasung HMK-072 integration notes

These are practical integration notes for the Hwasung HMK-072 thermal receipt
printer. They cover the things that are not obvious from the manual, mainly how
to read the paper sensor over USB, how to print Polish characters, how to store
and print a logo, and how to set the printer up under CUPS on Linux.

The notes exist because we hit real problems during integration and had to work
them out on the hardware. Everything written here was tested on an actual
HMK-072 unit.

Both an English and a Polish version of every document are included.

## What is in this folder

- `en/` and `pl/` hold the same four documents in English and Polish:
  - `01` paper and error status over USB (the important one)
  - `02` printing text and Polish characters
  - `03` storing and printing a logo
  - `04` setting the printer up under CUPS on Linux
- `scripts/` holds small Python scripts you can run to check the printer:
  - `read_paper_status.py` reads the paper and error state over USB
  - `escpos_status_probe.py` shows that the manual's status commands do not
    answer over USB (kept as proof, you do not need it for normal use)
  - `samples/` holds example output from those scripts
- The original vendor material sits one level up:
  - `HMK-072series_technical manual_eng.pdf` is the manufacturer manual
  - `SDK/` holds the vendor SDKs for Windows and Android
  - `Linux_driver(...)/` holds the CUPS driver packages for Linux

The scripts keep their comments in English because that is the normal
convention for code, but the documents that explain them exist in both
languages.

## What works, in short

- The printer is a USB device with vendor id `0x0006` and product id `0x000b`.
  It shows up as "hwasung HWASUNG USB Printer I/F".
- Over USB it identifies itself as
  `MFG:HWASUNG;CMD:ESC/POS;MDL:HMK-072;CLS:PRINTER;DES:THERMAL PRINTER`.
- Printing is plain ESC/POS. You write raw bytes to the bulk OUT endpoint
  `0x01`.
- Polish characters work on Font A through the Windows-1250 code page. You
  select it with `ESC t 5` (bytes `1B 74 05`) and you send the text encoded as
  cp1250, not UTF-8.
- The paper sensor can be read over USB, but not with the commands printed in
  the manual. The manual's `DLE EOT` and `GS r` status commands only answer on
  the RS-232 serial port. Over USB they time out.
- Over USB the printer answers a different, undocumented 6-byte command,
  `10 AA 55 80 54 AB`. You write those six bytes to the bulk OUT endpoint and
  read one status byte back from the bulk IN endpoint `0x83`.
- We confirmed on the hardware: with paper loaded the reply is `0x00`, with
  paper removed it is `0x09`, which means paper out plus paper low. The reply
  comes back in well under 30 ms.
- A logo can be stored in the printer once and then printed with a short
  command each time, so you do not have to send the image on every receipt.

## Where to start

If your first problem is the paper sensor, read `en/01-paper-status-over-usb.md`
(or the Polish version in `pl/`) and run `scripts/read_paper_status.py`.
Everything else is standard ESC/POS.
