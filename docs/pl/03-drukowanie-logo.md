# Zapis i drukowanie logo

Nie musisz wysyłać bitmapy logo przy każdym paragonie. Drukarka potrafi
przechowywać logo we własnej pamięci flash. Rejestrujesz je raz, a potem
drukujesz krótką komendą za każdym razem.

## Rejestracja logo (jednorazowo)

Użyj `FS q` (bajty `1C 71 n xL xH yL yH <dane>`). To zapisuje obraz w pamięci
nieulotnej drukarki.

- `n` to liczba definiowanych obrazów.
- Szerokość to `xL + xH * 256`, a wysokość to `yL + yH * 256`, obie liczone w
  grupach po 8 dotów. Dlatego szerokość i wysokość w pikselach muszą być
  podzielne przez 8.
- Dane obrazu są w układzie column major. Wysyłasz je kolumna po kolumnie, od
  lewej. Każdy bajt to 8 pionowych pikseli, z najbardziej znaczącym bitem na
  górze.

Jest to opisane w instrukcji producenta na stronie 63, którą warto przeczytać,
jeśli budujesz bitmapę samodzielnie.

## Drukowanie zapisanego logo (na każdym paragonie)

Gdy logo jest już zarejestrowane, siedzi w pamięci flash drukarki. Żeby je
wydrukować, wysyłasz tylko trzy bajty:

```
1C 70 01 00
```

To `FS p` z `n = 1` (slot 1) i `m = 0` (tryb normalny). Drukarka sama wyciąga
obraz z własnej pamięci, więc nie wysyłasz bitmapy ponownie.

## Uwaga praktyczna

Rejestracja logo to najbardziej żmudny etap ze względu na format column major i
grupowanie po 8 pikseli. Gdy logo jest już zapisane, jego drukowanie jest
trywialne i prawie nie zwiększa rozmiaru paragonu.
