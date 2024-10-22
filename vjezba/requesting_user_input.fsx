// pokrenuti sa dotnet fsi requesting_user_input.fsx

printfn "Enter a number"

let x = 
  let input = System.Console.ReadLine() in
    int32 input

let square y = y * y

printfn "You entered %d and its square is %d" x (square x)
