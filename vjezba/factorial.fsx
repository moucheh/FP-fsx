// factorial function

// normal recursion

// let rec factorial n =
//   match n with
//   | 0 -> 1
//   | _ -> n * factorial ( n - 1 )
// printfn $"{factorial 5}"

// tail recursion

printf "Enter input number for factorial function: "
let factorial ( n : uint64 ) = 
  let rec loop ( p : uint64 ) ( x : uint64 ) =
    match x with
    | 0uL -> p
    | _ -> loop ( x * p ) ( x - 1uL )
  loop 1uL n

let input = System.Console.ReadLine ()
printfn $"Factorial of {input} is {factorial (uint64 input)}"


(*
 loop analysis
 n = 5
 => loop   1 x=5
 => loop   5 x=4
 => loop  20 x=3
 => loop  60 x=2
 => loop 120 x=1
 => loop 120 x=0
 *)
