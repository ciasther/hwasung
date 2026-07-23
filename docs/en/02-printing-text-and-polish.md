# Printing text and Polish characters

Printing on the HMK-072 is plain ESC/POS. You write raw bytes to the bulk OUT
endpoint `0x01`. The only thing that needs care is getting Polish characters to
come out correctly.

## How Polish characters work

The printer has Windows-1250 built into its ROM, and Font A can print it. So
the two rules are:

- Select the Windows-1250 code page with `ESC t 5` (bytes `1B 74 05`).
- Send the text encoded as cp1250, not UTF-8. If you send UTF-8 you get
  garbage in place of the accented letters.

Two things to watch out for:

- Only Font A prints Polish. Font B (`ESC M 1`, bytes `1B 4D 01`) is ASCII
  only.
- Every `ESC M` command (any font change) resets the code page. So after each
  font change you have to send `ESC t 5` again before the next line with Polish
  characters.

## The init sequence

Send this once, right after you open the connection:

- `1B 40` (ESC @) reset the printer
- `1C 2E` (FS .) turn off the 2-byte Korean mode
- `1A 78 01` (SUB x 1) switch to the 1-byte extended graphic mode
- `1B 74 05` (ESC t 5) select the Windows-1250 code page
- `1D 4C 14 00` (GS L 20) set a left margin of about 2.5 mm
- `1B 4D 00` (ESC M 0) select Font A

After that you can print text. Remember to resend `1B 74 05` after any font
change.

## A minimal Python example

This uses the python-escpos library:

```python
from escpos.printer import Usb

p = Usb(0x0006, 0x000b, timeout=5000, in_ep=0x83, out_ep=0x01)

# init
p._raw(b'\x1B\x40\x1C\x2E\x1A\x78\x01\x1B\x74\x05\x1D\x4C\x14\x00\x1B\x4D\x00')

# text with Polish characters, encoded as cp1250
p._raw('Zolc, gesla jazn, 1,23 PLN\n'.encode('cp1250'))

# feed and cut
p._raw(b'\n\n\n\x1B\x69')
p.close()
```

## Font size and formatting

- Size is set with `GS !` (bytes `1D 21 nn`). `0x11` gives double width and
  double height, `0x00` goes back to normal.
- Alignment is `ESC a` (`1B 61 n`), with n of 0, 1, 2 for left, center, right.
- After changing font with `ESC M`, resend `ESC t 5` before printing Polish
  text.

Example, a larger heading:

```python
p._raw(b'\x1B\x4D\x00\x1B\x74\x05')   # Font A, reassert cp1250
p._raw(b'\x1D\x21\x11')               # double size
p._raw('NAGLOWEK\n'.encode('cp1250'))
p._raw(b'\x1D\x21\x00')               # back to normal size
```

## Cutting the paper

The short example above uses `ESC i` (bytes `1B 69`) to cut. Feed a few blank
lines first so the cut does not slice through the last line of text.
