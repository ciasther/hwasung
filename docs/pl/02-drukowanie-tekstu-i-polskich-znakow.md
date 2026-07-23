# Drukowanie tekstu i polskich znaków

Drukowanie na HMK-072 to zwykły ESC/POS. Wysyłasz surowe bajty na endpoint bulk
OUT `0x01`. Jedyne, co wymaga uwagi, to poprawne wyświetlenie polskich znaków.

## Jak działają polskie znaki

Drukarka ma Windows-1250 wbudowany w ROM, a Font A potrafi go drukować. Stąd
dwie zasady:

- Wybierz stronę kodową Windows-1250 komendą `ESC t 5` (bajty `1B 74 05`).
- Wysyłaj tekst zakodowany w cp1250, a nie w UTF-8. Jeśli wyślesz UTF-8, w
  miejscu polskich liter pojawią się krzaki.

Dwie rzeczy, na które trzeba uważać:

- Polskie znaki drukuje tylko Font A. Font B (`ESC M 1`, bajty `1B 4D 01`) jest
  wyłącznie ASCII.
- Każda komenda `ESC M` (dowolna zmiana fontu) resetuje stronę kodową. Dlatego
  po każdej zmianie fontu trzeba ponownie wysłać `ESC t 5`, zanim pojawi się
  kolejna linia z polskimi znakami.

## Sekwencja inicjalizująca

Wyślij ją raz, zaraz po otwarciu połączenia:

- `1B 40` (ESC @) reset drukarki
- `1C 2E` (FS .) wyłączenie trybu 2-bajtowego (koreańskiego)
- `1A 78 01` (SUB x 1) przełączenie na tryb 1-bajtowy extended graphic
- `1B 74 05` (ESC t 5) wybór strony kodowej Windows-1250
- `1D 4C 14 00` (GS L 20) lewy margines około 2,5 mm
- `1B 4D 00` (ESC M 0) wybór Fontu A

Potem możesz drukować tekst. Pamiętaj, żeby ponownie wysłać `1B 74 05` po każdej
zmianie fontu.

## Minimalny przykład w Pythonie

Korzysta z biblioteki python-escpos:

```python
from escpos.printer import Usb

p = Usb(0x0006, 0x000b, timeout=5000, in_ep=0x83, out_ep=0x01)

# inicjalizacja
p._raw(b'\x1B\x40\x1C\x2E\x1A\x78\x01\x1B\x74\x05\x1D\x4C\x14\x00\x1B\x4D\x00')

# tekst z polskimi znakami, zakodowany w cp1250
p._raw('Zolc, gesla jazn, 1,23 PLN\n'.encode('cp1250'))

# wysuw i cięcie
p._raw(b'\n\n\n\x1B\x69')
p.close()
```

## Rozmiar fontu i formatowanie

- Rozmiar ustawia się komendą `GS !` (bajty `1D 21 nn`). `0x11` daje podwójną
  szerokość i podwójną wysokość, `0x00` wraca do normalnego rozmiaru.
- Wyrównanie to `ESC a` (`1B 61 n`), gdzie n równe 0, 1, 2 to lewo, środek,
  prawo.
- Po zmianie fontu komendą `ESC M` wyślij ponownie `ESC t 5`, zanim wydrukujesz
  polski tekst.

Przykład większego nagłówka:

```python
p._raw(b'\x1B\x4D\x00\x1B\x74\x05')   # Font A, ponowne cp1250
p._raw(b'\x1D\x21\x11')               # podwójny rozmiar
p._raw('NAGLOWEK\n'.encode('cp1250'))
p._raw(b'\x1D\x21\x00')               # powrót do normalnego rozmiaru
```

## Cięcie papieru

Krótki przykład powyżej używa `ESC i` (bajty `1B 69`) do cięcia. Wysuń najpierw
kilka pustych linii, żeby cięcie nie przecięło ostatniej linii tekstu.
