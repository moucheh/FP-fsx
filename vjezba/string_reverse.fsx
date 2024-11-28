let a = "Hello World!"
let rec reverse (str : string) : string =
  match str.Length with
  | 0 -> ""
  | 1 -> (str[0]).ToString ()
  | _ -> 
    let last = str.Length - 1 
    let new_bound = last - 1
    (str[last]).ToString () + reverse str[0..new_bound]

a |> reverse |> printfn "%s"