# Konfiguracja drukarki w CUPS na Linux

Jeśli drukujesz przez CUPS zamiast wysyłać surowe bajty samodzielnie, musisz
zająć się dwiema sprawami. CUPS nie wysyła sekwencji inicjalizującej i nie
konwertuje UTF-8 na cp1250. Dlatego zwykły plik tekstowy wydrukowany przez CUPS
wychodzi ze złą stroną kodową i bez polskich znaków.

Rozwiązaniem jest mały filtr CUPS, który wstrzykuje sekwencję inicjalizującą i
konwertuje tekst, plus plik PPD, który na niego wskazuje.

Producent dostarcza też gotowy sterownik CUPS w katalogu `Linux_driver(...)`
(paczki deb i rpm, `hwacups-1.0.1`, 32 i 64 bit). Użyj go, jeśli chcesz tylko
działającej kolejki i nie potrzebujesz obsługi polskiego tekstu. Filtr poniżej
jest na wypadek, gdy polski tekst jest potrzebny.

## Krok 1, filtr

Zapisz to jako `/usr/lib/cups/filter/texttoescpos-pl`:

```bash
#!/bin/bash
# argv: job-id user title copies options [file]
FILE="${6:-/dev/stdin}"

# init: ESC @ | FS . | SUB x 1 | ESC t 5 | GS L 20 | ESC M 0
printf '\033@\034.\032x\001\033t\005\035L\024\000\033M\000'

# konwersja UTF-8 na cp1250
iconv -f UTF-8 -t CP1250//TRANSLIT < "$FILE"

# wysuw i cięcie
printf '\n\n\n\033i'
```

Nadaj mu prawo wykonywania i właściciela root:

```bash
sudo chmod 755 /usr/lib/cups/filter/texttoescpos-pl
sudo chown root:root /usr/lib/cups/filter/texttoescpos-pl
```

## Krok 2, PPD

Zapisz to jako `hmk072-pl.ppd`:

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

## Krok 3, dodanie kolejki

```bash
sudo lpadmin -p HMK072-PL -v usb://Hwasung/HMK072 -P hmk072-pl.ppd -E
sudo cupsaccept HMK072-PL
```

Rzeczywisty URI swojej drukarki znajdziesz komendą:

```bash
lpinfo -v | grep usb
```

## Krok 4, druk

```bash
echo "Zolc gesla jazn" | lp -d HMK072-PL
lp -d HMK072-PL paragon.txt
```

## Ograniczenia drogi przez CUPS

- Obsługuje tylko zwykły tekst (`text/plain`, UTF-8).
- PDF-y i obrazki idą przez ścieżkę rastrową, która używa bufora 4 KB, a ten
  wywraca HMK-072. Do paragonów z formatowaniem wysyłaj surowy ESC/POS
  samodzielnie (droga przez Python) albo użyj `application/vnd.cups-raw`.
- Filtr nie ma pogrubienia ani sterowania rozmiarem. To po prostu printf z
  konwersją strony kodowej. Prawdziwe formatowanie wymaga surowych bajtów.
