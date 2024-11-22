// Napisati funkciju koja uzima 2 argumenta: divisor:int i listu integer-a.
// Funkcija treba nazad da vrati listu svih integer-a djeljivih sa
// prvim argumentom.

let rec foo (d : int) (l : List<int>) : List<int> =
   match l with
   | [] -> []
   | x :: xs when x % d = 0 -> x :: foo d xs 
   | x :: xs -> foo d xs

[1; 2; 3; 4; 5; 6; 7; 8;] |> foo 2 |> printfn "%A"
