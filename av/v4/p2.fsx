// Napisati funkciju koja racuna cijenu potrosene elektricne energije.
// Cijena po kWh se racuna na osnovu kolicine potrosene energije
// Postoje 4 opsega potrosnje:
// Opseg A: 0 - 100 kWh - 0.1 KM po kWh
// Opseg B: 101 - 200 kWh - 0.15 po KWh
// Opseg C: 201 - 500 kWh - 0.2 po KWh
// Opseg D: preko 500 kWh - 0.25 po KWh
// Dodatno za preko 300 kWh potrosnje dodati 5 KM na ukupnu cijenu


let calculate ( x : double ) =
    if x > 0 && x < 100 then
        $"{x * 0.1} KM"
    elif x >= 100 && x < 200 then
        $"{x * 0.15} KM"
    elif x >= 200 && x < 300 then
        $"{x * 0.2} KM"
    elif x < 500 && x > 300 then
        $"{x * 0.2 + 5.} KM"
    elif x >= 500 then
        $"{x * 0.25 + 5.} KM"
    else
        ""

printfn $"{calculate 500}"
printfn $"{calculate 100}"
printfn $"{calculate 333}"
printfn $"{calculate 250}"

