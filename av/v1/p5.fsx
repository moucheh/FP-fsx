// Napisati let izraz koji veze simbol concat za lambda izraz
// koji uzima 3 parametra. Prva dva parametra su stringovi
// koje treba spojiti, a treci parametar je delimiter izmedju
// dva stringa. Pozvati lambdu u expr2 dijelu let izraza:
// concat "Hello" "World" "_" ispisuje Hello_World

let concat first second delimiter =
    first + delimiter + second

// let concat = fun a b c -> a + c + b in
// concat "Hello" "World" "_"

concat "Hello" "World" "_"
