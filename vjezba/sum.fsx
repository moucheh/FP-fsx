// sum of first n natural numbers

// normal recursion

// let rec sum x =
//   match x with
//   | 0 -> 0
// printfn $"Sum of first 10 natrual numbers is {sum 10}"

// tail recursion

printf "Enter input number for the sum function: "
let sum n =
  let rec loop a x =
    match x with
    | 0 -> a
    | _ -> loop ( a + x ) ( x - 1 )
  loop 0 n
let input = System.Console.ReadLine ()
printfn $"The sum of first {input} natural numbers is {sum (int input)}"

(*
 n = 5
 => loop  0 x=5
 => loop  5 x=4
 => loop  9 x=3
 => loop 12 x=2
 => loop 14 x=1
 => loop 15 x=0
 *)
