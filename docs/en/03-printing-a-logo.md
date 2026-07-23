# Storing and printing a logo

You do not have to send the logo bitmap on every receipt. The printer can keep
a logo in its own flash memory. You register it once, then print it with a
short command each time.

## Registering the logo (once)

Use `FS q` (bytes `1C 71 n xL xH yL yH <data>`). This stores the image in the
printer's non-volatile memory.

- `n` is the number of images you are defining.
- The width is `xL + xH * 256` and the height is `yL + yH * 256`, both counted
  in groups of 8 dots. So the width and height in pixels must both be
  divisible by 8.
- The image data is column major. You send it column by column, from the left.
  Each byte is 8 vertical pixels, with the most significant bit at the top.

This is described in the manufacturer manual on page 63, which is worth reading
if you build the bitmap yourself.

## Printing the stored logo (on each receipt)

Once the logo is registered it stays in the printer's flash. To print it you
send just three bytes:

```
1C 70 01 00
```

That is `FS p` with `n = 1` (slot 1) and `m = 0` (normal mode). The printer
pulls the image out of its own memory, so you do not send the bitmap again.

## Practical note

Registering the logo is the fiddly part because of the column-major, 8-pixel
grouped format. Once it is stored, printing it is trivial and adds almost
nothing to the size of each receipt.
