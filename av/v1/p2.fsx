// Napisati let izraz koji ce dokazati da operator left shift
// radi operaciju p<<q <==> p = p * 2 ^ q (koristiti ** operator)

let p = 8
let q = 2

let lhs = (float (p <<< q))
let rhs = (float p) * 2. ** (float q)

printfn $"{lhs = rhs}" 
