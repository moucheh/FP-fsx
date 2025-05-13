// da li kompajlira
// koji je ispis
// koji su simboli pravilno definisani
// navesti njihov tip i vrijednos

let foo x y z =
  printf "1"
  let res a =
    x + y - a + z
  res
printf "2"
let bar = foo 5 4
printf "3"
let tar = bar 4 5
printf "4"
tar

// ispis 2314
// tar : int = 8
// foo : x: int -> y: int -> z: int -> (int -> int)
