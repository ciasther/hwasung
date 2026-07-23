# Odczyt statusu papieru i błędów przez USB

To był najtrudniejszy element, dlatego opisujemy go na początku.

## Problem

Instrukcja HMK-072 podaje komendy statusu na stronach 79 i 80. Są to komendy
ESC/POS `DLE EOT` (bajty `10 04 n`) oraz `GS r` (bajty `1D 72 n`). Zgodnie z
instrukcją zwracają one bajt statusu, który mówi, czy papier się skończył lub
kończy.

Przez interfejs USB żadna z nich nie odpowiada. Wysyłasz komendę, próbujesz
odczytać odpowiedź i odczyt kończy się timeoutem. Zachowuje się tak samo
niezależnie od tego, czy papier jest założony, czy wyjęty, więc nie dostajesz
żadnej użytecznej informacji. Te komendy działają wyłącznie na porcie
szeregowym RS-232 drukarki.

Zostawiliśmy skrypt, który to pokazuje, `scripts/escpos_status_probe.py`, oraz
jego wynik w `scripts/samples/manual-commands-timeout.txt`. Do normalnej pracy
nie jest potrzebny, jest tam tylko jako dowód.

## Co naprawdę działa

Przez USB drukarka odpowiada na inną komendę, której nie ma w instrukcji.
Znaleźliśmy ją w firmowych SDK Hwasunga (w SDK na Androida oraz w windowsowej
`HwaUSB.dll`), gdzie jest budowana bajt po bajcie, a potem potwierdziliśmy ją
na sprzęcie.

Komenda ma sześć bajtów:

```
10 AA 55 80 54 AB
```

Procedura wygląda tak:

1. Zapisz te sześć bajtów na endpoint bulk OUT `0x01`.
2. Odczekaj około jednej milisekundy.
3. Odczytaj jeden bajt z endpointu bulk IN `0x83`.

Ten jeden bajt to status. Krótki timeout odczytu wystarczy, bo odpowiedź
przychodzi niemal natychmiast.

## Jak czytać bajt statusu

Bajt to zestaw flag. Sprawdzaj go bit po bicie:

- bit 0 ustawiony: brak papieru
- bit 1 ustawiony: otwarta pokrywa lub głowica
- bit 2 ustawiony: zacięcie papieru
- bit 3 ustawiony: mało papieru (rolka blisko końca)
- bit 5 ustawiony: zacięcie obcinarki
- bit 7 ustawiony: papier wydrukowany, ale nieodebrany

Wartość `0x00` oznacza, że wszystko jest w porządku.

Na przykład `0x09` to bit 0 plus bit 3, czyli jednocześnie brak papieru i mało
papieru. Dokładnie taki wynik dostajesz po wyjęciu rolki.

## Co zmierzyliśmy na drukarce

- Papier założony, odpowiedź to `0x00`, przychodzi w mniej niż milisekundę.
- Papier wyjęty, odpowiedź to `0x09`, przychodzi w około 15 do 30 ms.

Czyli brak papieru i niski stan papieru są wykrywalne przez USB tą jedną
komendą. Nie trzeba podłączać portu szeregowego.

## Szczegóły USB, które mogą się przydać

- Vendor id `0x0006`, product id `0x000b`.
- Interfejs 0. Na Linuxie może być konieczne najpierw odpięcie sterownika
  jądra `usblp` od tego interfejsu, a potem jego przejęcie.
- Bulk OUT to adres endpointu `0x01` (pierwszy endpoint OUT na interfejsie 0).
- Bulk IN to adres endpointu `0x83` (endpoint IN o indeksie 2 na interfejsie
  0).
- Długość odczytu to jeden bajt. Timeout ustaw krótki.

## Uwaga dla dostawcy

Ta komenda statusu USB nie jest nigdzie udokumentowana w instrukcji HMK-072,
która opisuje tylko szeregowe komendy `DLE EOT` i `GS r`. Jeśli będziecie
rozmawiać z Hwasungiem, warto poprosić o oficjalne udokumentowanie tej komendy
USB, potwierdzenie znaczenia wszystkich bitów oraz potwierdzenie, czy ta sama
komenda działa w całej serii HMK.
