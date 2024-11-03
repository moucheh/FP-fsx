// calculate n-th fibonacci number

// let rec fib n =
//   match n with
//   | 0 | 1 -> n
//   | _ -> fib ( n - 1 ) + fib ( n - 2 )
// printfn $"{fib 5}"

printf "Fibonacci n-th term caluclator\nEnter number: "
let fib n =
  let rec loop a1 a2 x =
    match x with
    | 0 -> a1
    | 1 -> a2
    | _ -> loop a2 ( a1 + a2 ) ( x - 1 ) 
  loop 0 1 n

let input = System.Console.ReadLine ()

let format_str =
  match ( fun x -> x % 10 ) ( int input ) with
  | 1 when (int input) % 100 <> 11 -> "st"
  | 2 -> "nd"
  | 3 -> "rd"
  | _ -> "th"
printfn $"The {input}{format_str} fibonacci number is {fib (int input)}"

(*
 loop analysis
 n = 6 
 => loop 0 1 x=6
 => loop 1 1 x=5
 => loop 1 2 x=4
 => loop 2 3 x=3
 => loop 3 5 x=2
 => loop 5 8 x=1
 *)
