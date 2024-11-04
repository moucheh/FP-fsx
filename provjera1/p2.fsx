// dodati potreban kod da kompletan kod kompajlira
// definisati funkciju koja ukoliko je broj paran
// ispise paran na ekran i vrati true
// u suprotnom se baca iznimka

//vas kod vodjika

let foo x =
  let 0 = x () % 2
  printfn "Paran"
  true

let enterNum (u:unit) : int =
  printf "Unesite neki broj: "
  System.Console.ReadLine () |> int
enterNum |> foo
