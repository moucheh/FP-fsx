// Napisati program koji definira tip Student koji sadrzi:
// ime, prezime, godine i najdrazi_predmet
// Program treba da instancira 3 studenta te ih ispise na ekran
// ime studenta te najdrazi predmet koristeci funkciju "ispisiStudenta"
// Funkcija treba da ispise studenta na sljedeci nacin:
//
// |Student|Godine|Najdrazi predmet|?|
// |Meho|Funkcionalno programiranje|:)|
// ili
// |Student|Godine|Najdrazi predmet|?|
// |Ivica|Objektno orijentirano programiranje|:(|

type Student = { 
    ime : string;
    prezime : string;
    godine : int;
    najdrazi_predmet : string 
}

let a = { ime = "Muhamed"; prezime = "Taletovic"; godine = 21; najdrazi_predmet = "Funckionalno programiranje"}
let b = { ime = "Mahir"; prezime = "Suljic"; godine = 21; najdrazi_predmet = "Objektno orijentirnao programiranje" }
let c = { ime = "Ahmed"; prezime = "Mesanovic"; godine = 21; najdrazi_predmet = "Modeliranje kod brate" }

let ispisiStudenta { ime = Ime; najdrazi_predmet= Predmet } =
    let smajli = ":)"
    let tugy = ":("
    let pr = "Funckionalno programiranje"
    printfn $"|{Ime}|{Predmet}|{ if Predmet = pr then smajli else tugy}"