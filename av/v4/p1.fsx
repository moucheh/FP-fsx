// Napisati funkciju calcNums koja uzima dva broja (double tipa) i
// string koji predstavlja operaciju koju treba izvesti.
// Funkcija nazad vraca string koji predstavlja izvedenu operaciju
// ili error poruku ukoliko operacija nije uspjela (dijeljenje sa 0
// ili nevalidna operacija).
// Funkcija treba da podrzava operacije "add", "sub", "mul" i "div"
// Primjer: calcNums 10.0 5.0 mul -> "10.0 * 5.0 = 50.0"

// let calcNums (a:float) (b:float) op =
//     if op = "add" then
//         $"{a} + {b} = {a + b}" 
//     elif op = "sub" then
//         $"{a} - {b} = {a - b}"
//     elif op = "mul" then
//         $"{a} * {b} = {a * b}" 
//     elif op = "div" then
//         if b = 0 then "nevalidna operacija" else
//         $"{a} \ {b} = {a / b}"
//     elif op = "mod" then
//         if b = 0 then "nevalidna operacija" else
//         $"{a} %% {b} = {a % b}"
//     else 
//         ""

let calcNums (a:float) (b:float) op =
    let result =
        match op with
        | "add" -> $"{a} + {b} = {a + b}"
        | "sub" -> $"{a} - {b} = {a - b}"
        | "mul" -> $"{a} * {b} = {a * b}"
        | "div" when b <> 0 -> $"{a} / {b} = {a / b}"
        | "div" -> "nevalidna operacija"
        | "mod" when b <> 0 -> $"{a} %% {b} = {a % b}"
        | "mod" -> "nevaildna opercija"
        | _ -> ""
    result

let resultAdd = calcNums 10.0 5.0 "add"
let resultSub = calcNums 10.0 5.0 "sub"
let resultMul = calcNums 10.0 5.0 "mul"
let resultDiv = calcNums 10.0 5.0 "div"
let resultDivByZero = calcNums 10.0 0.0 "div"
let resultInvalid = calcNums 10.0 5.0 "mod" // Invalid operation
printfn "%s" resultAdd
printfn "%s" resultSub
printfn "%s" resultMul
printfn "%s" resultDiv
printfn "%s" resultDivByZero
printfn "%s" resultInvalid
calcNums 10.0 5.0 "div" |> ignore
let aaa = ignore
let bbb = printf
calcNums 6.0 4.0 "add"
calcNums 6.0 4.0 "add"
