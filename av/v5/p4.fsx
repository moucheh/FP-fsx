// Napisati rekurzivnu strukturu podataka "BinaryTree". Struktura
// treba da bude polimorfna i da sadrzi comparison constraint nad
// tipom. 
// Napisati funkcije push, traverse, foldl i foldr. Elementi treba
// da se dodaju u binarno stablo koristeci operator "<".
// Traverse treba da prolazi kroz binarno stablo i ispisuje elemente
// in-order koristeci prnt funkciju koja treba da bude parametar
// travers-a.
// foldl treba uzme 3 parametra: funkciju koja uzima dva parametra
// pocetnu vrijednost i BinaryTree strukturu.
// Potrebno je pocetnu vrijednost apply-ati funkcijom sa elementom na 
// kranjoj lijevoj strani i rezultat date operacije apply-ati kao
// pocetnu vrijednost stabla sa desne strane   
// foldr treba da radi istu stvar u suprotnom smijeru.

type BinaryTree<'a> when 'a : comparison =
  | End
  | Node of 'a * BinaryTree<'a> * BinaryTree<'a>

let rec push v bt =
  match bt with
  | End -> Node (v, End, End)
  | Node (value, End, End) when v < value -> Node (value, Node (v, End, End), End)
  | Node (value, End, End) -> Node (value, End, Node(v, End, End))
  | Node (value, left, right) when v < value -> Node (value, push v left, right)
  | Node (value, left, right) -> Node (value, left, push v right)

let (<) bt v = push v bt

let bst = End < 10 < 9 < 11 < 3 < 4 < 15 < 13 < 5

let rec traverseInOrder (f : 'a -> unit) (bt : BinaryTree<'a>) =
  match bt with 
  | End -> () 
  | Node (value, left, right) ->
    traverseInOrder f left
    f value
    traverseInOrder f right
   

let rec traversePreOrder (f : 'a -> unit) (bt : BinaryTree<'a>) =
  match bt with 
  | End -> () 
  | Node (value, left, right) ->
    f value
    traversePreOrder f left
    traversePreOrder f right

let rec traversePostOrder (f : 'a -> unit) (bt : BinaryTree<'a>) =
  match bt with 
  | End -> () 
  | Node (value, left, right) ->
    traversePostOrder f left
    traversePostOrder f right
    f value

printfn"Prolazak u in order redoslijedu: "
traverseInOrder (printf "%A ") bst
printfn ""

printfn "Prolazak u post order redoslijedu: "
traversePostOrder (printf "%A ") bst
printfn ""

printfn "Prolazak u pre order redoslijedu: "
traversePreOrder (printf "%A ") bst
printfn ""
