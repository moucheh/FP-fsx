// Napisati discriminated union Oblik koji ima 3 varijante konstruktora:
// Krug, Kvadrat i Pravougaonik
// Svaki od konstruktora treba da uzme odgovarajuci broj parametara potrebnih
// za izracunavanje obima i povrsine oblika.
// Napisati funkcije obim i povrsina koje izracunavaju obim i povrsinu oblika.

type Oblik =
    | Krug of resultMul : double
    | Kvadrat of a : double
    | Pravougaonik of a : double * b : double

let Obim oblik =
    match oblik with
    | Krug r -> r * 6.28
    | Kvadrat a -> 4. * a
    | Pravougaonik (a, b) -> 2. * a + 2. * b

let Povrsina oblik =
    match oblik with
    | Krug r -> r * r * 3.14
    | Kvadrat a -> a * a
    | Pravougaonik (a, b) -> a * b

