// napisati niz let izraza koji dokazuju
// sljedecu izjavu: 2^q = 1 << q

let q = 5
let lhs = (float 2) ** (float q)
let rhs = 1 <<< q
let res = (int lhs) = rhs
printfn "%A" res
