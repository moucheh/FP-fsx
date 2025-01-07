open provjera4

type Pravougaonik (a : double, b : double) =
  inherit Cetverougao (a, b, a, b)

  member this.Sirina 
    with get () = this.Side1
    and set v =
      this.Side1 <- v
      this.Side3 <- v

  member this.Duzina
    with get () = this.Side2
    and set v =
      this.Side2 <- v
      this.Side4 <- v

  interface IPrintable with
    member this.PrintInfo () =
      $"a={this.Sirina}; b={this.Duzina}"

  interface IShape with
    member this.CalculateArea () =
      this.Sirina * this.Duzina


let p = Pravougaonik (4, 5)

printfn $"Stranice su: {(p :> IPrintable).PrintInfo ()}"

printfn $"Obim je: {p.Obim ()}"

printfn $"Povrsina je: {(p :> IShape).CalculateArea ()}"
