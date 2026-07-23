# Setting the printer up under CUPS on Linux

If you print through CUPS rather than sending raw bytes yourself, you have to
deal with two things. CUPS does not send the init sequence, and it does not
convert UTF-8 to cp1250. So a plain text file printed through CUPS comes out
with the wrong code page and no Polish characters.

The fix is a small CUPS filter that injects the init sequence and converts the
text, plus a PPD that points at it.

The vendor also ships a ready-made CUPS driver in the
`Linux_driver(...)` folder (deb and rpm packages, `hwacups-1.0.1`, 32 and 64
bit). Use that if you just want a working queue and do not need Polish text
handling. The filter below is for when you do need Polish text.

## Step 1, the filter

Save this as `/usr/lib/cups/filter/texttoescpos-pl`:

```bash
#!/bin/bash
# argv: job-id user title copies options [file]
FILE="${6:-/dev/stdin}"

# init: ESC @ | FS . | SUB x 1 | ESC t 5 | GS L 20 | ESC M 0
printf '\033@\034.\032x\001\033t\005\035L\024\000\033M\000'

# convert UTF-8 to cp1250
iconv -f UTF-8 -t CP1250//TRANSLIT < "$FILE"

# feed and cut
printf '\n\n\n\033i'
```

Make it executable and owned by root:

```bash
sudo chmod 755 /usr/lib/cups/filter/texttoescpos-pl
sudo chown root:root /usr/lib/cups/filter/texttoescpos-pl
```

## Step 2, the PPD

Save this as `hmk072-pl.ppd`:

```
*PPD-Adobe: "4.3"
*FormatVersion: "4.3"
*FileVersion: "1.0"
*LanguageVersion: English
*LanguageEncoding: ISOLatin1
*PCFileName: "HMK072PL.PPD"
*Product: "(HMK-072)"
*Manufacturer: "Hwasung"
*ModelName: "HMK-072 Polish"
*NickName: "HMK-072 (CP1250 Polish text)"
*ShortNickName: "HMK-072-PL"
*cupsVersion: 2.0
*cupsFilter: "text/plain 0 texttoescpos-pl"
*cupsFilter: "application/vnd.cups-raw 0 -"
*DefaultPageSize: Receipt
*PageSize Receipt/80mm: "<</PageSize[204 1000]/ImagingBBox null>>setpagedevice"
*PageRegion Receipt/80mm: "<</PageSize[204 1000]/ImagingBBox null>>setpagedevice"
*DefaultImageableArea: Receipt
*ImageableArea Receipt/80mm: "0 0 204 1000"
*DefaultPaperDimension: Receipt
*PaperDimension Receipt/80mm: "204 1000"
```

## Step 3, add the queue

```bash
sudo lpadmin -p HMK072-PL -v usb://Hwasung/HMK072 -P hmk072-pl.ppd -E
sudo cupsaccept HMK072-PL
```

Find your printer's real URI with:

```bash
lpinfo -v | grep usb
```

## Step 4, print

```bash
echo "Zolc gesla jazn" | lp -d HMK072-PL
lp -d HMK072-PL receipt.txt
```

## Limits of the CUPS route

- It only handles plain text (`text/plain`, UTF-8).
- PDFs and images go through the raster path, which uses a 4 KB buffer that
  crashes the HMK-072. For receipts with formatting, send raw ESC/POS yourself
  (the Python route) or use `application/vnd.cups-raw`.
- The filter has no bold or size control. It is just a printf with a code page
  conversion. Any real formatting needs raw bytes.
