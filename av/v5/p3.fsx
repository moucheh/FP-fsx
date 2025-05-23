// Napisati rekurzivnu polimorfnu data strukturu V5L koja treba da radi sljedece operacije:

// foldl - funkcije viseg reda koje uzimaju folding funkciju kao prvi parametar, inicijalno stanje fold-a kao drugi parametar te listu kao treci parametar. Funkcija treba da aplicira folding funkciju na inicijalno stanje i prvi parametar liste, zatim rezultat prosljedjuje folding funkciji sa drugim argumentom i tako dalje.
// foldr - funkcija viseg reda koja uzim folding funkciju kao prvi parametar, inicijalno stanje fold-a kao drugi parametar te listu kao treci parametar. Funkcija dreba da aplicira folding funkciju na posljednji element liste i inicijalno stanje, zatim aplicira predposljednji element liste i rezultat folding funkcije i tako dalje. 
// append - funkcija koja uzima dvije liste istog tipa te vraca nazad novu listu gdje se na pocetku nalaze prvo elementi prve liste, zatim elementi druge liste. Napraviti da operator @@ append-a liste.
// filter - funkcija koja uzima filter funkciju ('a->bool), listu, a nazad vraca samo one elemente liste za koje filter funkcija vrati tacno.
// head - funkcija koja mozda vrati nazad prvi element liste
// tail - funkcija koja mozda vrati nazad posljednji element liste
// pushFront - funkcija koja dodaje element na pocetak liste. Napraviti da operator ++ radi pushFront
// take - funkcija koja uzima integer i listu, a nazad vraca listu koja se sastoji od prvih n elemenata liste.
// zip - Uzima dvije liste kao argument, te nazad vraca listu tuple-ova gdje je svaki element par prvi element prve liste, prvi element druge liste. Funkcija vraca min(l1.Length, l2.Length) clanova.

type List<'a> =
| End
| Node of 'a * List<'a> 

let rec foldl (f : 'a -> 'b -> 'a) (a : 'a) (l : List<'b>) : 'a =
    match l with
    | End -> a
    | Node (x, xs) -> foldl f (f a x) xs

let reverse (l : List<'a>) : List<'a> =
  foldl (fun a e -> Node (e, a)) End l

let head l =
  match l with
  | End -> System.Exception "Prazna lista" |> raise 
  | Node (x, xs) -> x

let tail l =
  match reverse l with
  | End -> System.Exception "Prazna lista." |> raise
  | Node (x, xs) -> x

let foldr (f : 'a -> 'b -> 'b) (a : 'b) (l : List<'a>) : 'b =
  let reversed = reverse l
  let rec foldr' f' a' l' =
    match l' with
    | End -> a'
    | Node (x, xs) -> foldr' f' (f' x a') xs
  foldr' f a reversed

let pushFront e l =
    Node (e, l)
    
let (++) = pushFront

let rec append a b =
    match a with
    | End -> b
    | Node (x, xs) ->
        Node (x, append xs b)

let take (n : int) (l : List<'a>) : List<'a> =
  let rec loop a n' l' =
    match (n', l') with
    | (0, _) -> a
    | (_, End) -> End
    | (n'', Node (x, xs)) -> loop ( Node (x, a) ) (n'' - 1) xs
  loop End n l |> reverse

let rec drop (n : int) (l : List<'a>) : List<'a> =
  match (n, l) with
  | (0, xs) -> xs
  | (_, End) -> End
  | (n', Node (x, xs)) -> drop (n' - 1) xs

let rec filter (f : 'a -> bool)  (l : List<'a>) =
    match l with
    | End -> End
    | Node (x, xs) when f x -> Node (x, filter f xs)
    | Node (x, xs) -> filter f xs 

let rec zip (l1 : List<'a>) (l2 : List<'b>) : List<('a * 'b)> =
  match (l1, l2) with
  | (_, End) -> End
  | (End, _) -> End
  | (Node (x1, xs1), Node (x2, xs2)) -> Node ((x1, x2), zip xs1 xs2)

let (@@) = append

let rec sort (f : 'a -> 'a -> bool) (l : List<'a>) : List<'a> =
    match l with
    | End -> End
    | Node (x, xs) ->
        let falsy = filter (fun y -> f y x) xs
        let truthy = filter (fun y -> f x y ) xs
        sort f falsy @@ Node (x, End) @@ sort f truthy 

let lst = Node (1, Node (7, Node (2, Node (6, Node (3, Node (4, End))))))
let lst2 = Node (5, Node (9, Node (11, End)))

let rec printList (l : List<'a>) =
  match l with
  | End -> printf "\n"
  | Node (x, xs) -> 
      printf $"{x} " 
      printList xs

printfn "lst:"

printList lst

printfn "lst2:"

printList lst2

printfn "Sortirana lista lst:"
lst |> sort (fun x y -> x < y) |> printList

printfn "Lista lst u kojoj su samo parni brojevi"
lst |> filter (fun x -> x % 2 = 0) |> printList 

printfn "lst2 appendiran na lst:"
lst @@ lst2 |> printList

printfn "Suma lst: "
printList lst
lst |> foldl (fun x a -> x + a) 0 |> printfn "= %d"
