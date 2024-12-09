//1.

type Lista<'a> =
  | Kraj
  | Cvor of 'a * Lista<'a>

//2.

let rec zip (l1 : Lista<'a>) (l2 : Lista<'b>) : Lista<'a * 'b> =
  match (l1, l2) with
  | (Kraj, _) -> Kraj
  | (_, Kraj) -> Kraj
  | (Cvor (x1, xs1), Cvor (x2, xs2)) -> Cvor ((x1, x2), zip xs1 xs2)

//3.

let rec foldr (f: 'a -> 'b -> 'b) (a : 'b) (l : Lista<'a>) : 'b =
  match l with
  | Kraj -> a
  | Cvor (x, xs) -> f x (foldr f a xs)

//4.

let unzip (l : Lista<'a * 'b>) : Lista<'a> * Lista<'b> =
    foldr (fun e a -> Cvor (fst e, fst a), Cvor (snd e, snd a)) (Kraj, Kraj) l


let lst1 : Lista<int> = Cvor(1, Cvor(2, Cvor(3, Cvor(4, Cvor(5, Kraj)))))
let lst2 : Lista<int> = Cvor(6, Cvor(7, Cvor(8, Cvor(9, Cvor(10, Kraj)))))

lst1 |> foldr (+) 0 |> printfn "%d"

let zipped : Lista<int * int> = zip lst1 lst2 

zipped |> printfn "%A"

unzip zipped |> printfn "%A"

Cvor(8, Cvor(12, Cvor(24, Cvor(4, Kraj)))) |> foldr (/) 2 |> printfn "%d"
