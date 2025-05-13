// Digitalni korijen predstavlja vrijednost koju dobijemo rekurzivno
// sabirajuci cifre broja sve dok rezultat sabiranja cifara nije
// jednocifren broj.
//
// Primjer:
// Digitalni korijen broja 9876:
//  Suma cifara: 9 + 8 + 7 + 6 = 30
//  30 nije jednocifren broj, idemo ponovno u rekurziju:
//  3 + 0 = 3
//
// Rezultat je 3

// Uporediti rezultat sa modulom broja 9. :)
let num = 9876

let rec sum (l : char list) : int =
    match l with
    | [] -> 0;
    | x :: xs ->
        let xn = int x - int '0'
        xn + sum xs

let rec digital_root x =
    match x |> string |> Seq.toList |> List.length with
    | 0 -> 0
    | 1 -> x 
    | _ -> string x |> Seq.toList |> sum |> digital_root

let result = digital_root num 

num % 9 = result

