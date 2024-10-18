// Zasto se dati izraz ne kompajlira?

let foo = fun x y -> x * y
let _ = foo 5.4f 6.1f
foo 7.2f 8.4f

(*
  u liniji 4, aplicira se funkcija foo na float32 vrijednosti,
  od tog momenta simbol foo je tipa float32 -> float32
  tako da aplikacija u liniji 5 ne uspijeva, jer se tipovi ne
  poklapaju, float32 != float
*)
