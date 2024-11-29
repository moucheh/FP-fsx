//  _____  _  _
// |  ___|| || |_
// | |_ |_  ..  _|
// |  _||_      _|
// |_|    |_||_|
//
// Prvojera 2
//
// 1. a
type Date =
  {
    day : int;
    month : int;
    year : int
  }

// 1. b
type PromoCode =
  | None
  | PC5 of string
  | PC10 of string
  | PC20 of string

// 1. c
type Ride =
  {
    RideId : int;
    DriverId : int;
    City : string;
    Date : Date;
    DistanceKM : float;
    DurationMin : int;
    Fare : float;
    Rating : float;
    PromoCode : PromoCode
  }

// 2.
let rideQuality { Ride.Rating = r} : string =
  if r < 1 then "Terrible"
  elif r >= 1 && r < 2 then "Bad"
  elif r >= 2 && r < 3 then "Okay"
  elif r >= 3 && r < 4 then "Good"
  else "Excellent"

// dato
let stringToDate (str : string) : Date =
  match str.Split '/' with
  | [|d; m; y|] ->
    {
      day = d |> int;
      month = m |> int;
      year = y |> int
    }
  | _ -> failwith "Invalid date"

// dato
let stringToPromoCode (str : string) : PromoCode =
  match str with
  | "SAVE5" -> PC5 "SAVE5"
  | "DISCOUNT10" -> PC10 "DISCOUNT10"
  | "PROMO20" -> PC20 "PROMO20"
  | _ -> None

// 3.
let loadData (u : unit) : List<Ride> =
  let headers = System.Console.ReadLine ()
  let rec loadData_ (u : unit) : List<Ride> =
    let line = System.Console.ReadLine ()
    if line = null then []
    else
      let parsed =
        match line.Split ',' with
        | [|rid; did; c; d; dkm; dmin; f; r; pc|] ->
          {
            RideId = rid |> int;
            DriverId = did |> int;
            City = c;
            Date = d |> stringToDate;
            DistanceKM = dkm |> float;
            DurationMin = dmin |> int;
            Fare = f |> float;
            Rating = r |> float;
            PromoCode = pc |> stringToPromoCode
          }
        | _ -> failwith "Invalid ride format."
      parsed :: loadData_ () 
  loadData_ ()

// 4.
let rec divide (pred : Ride -> bool) (l : List<Ride>) : List<Ride> * List<Ride> =
  match l with
  | [] -> ([], []) 
  | x :: xs when pred x ->
    (x :: fst (divide pred xs),  snd (divide pred xs))
  | x :: xs -> 
    (fst (divide pred xs), x :: snd (divide pred xs))

// let rec divideI (pred : int -> bool) (l : List<int>) : List<int> * List<int> =
//   match l with
//   | [] -> ([], []) 
//   | x :: xs when pred x ->
//     (x :: fst (divideI pred xs),  snd (divideI pred xs))
//   | x :: xs -> 
//     (fst (divideI pred xs), x :: snd (divideI pred xs))
//
// [1; 2; 3; 4; 5; 6; 7; 8; 9; 10] |> divideI (fun x -> x % 2 = 0) |> printfn "%A"

// 5.
let firstVsLast (l : List<Ride>) : bool =
  if l.IsEmpty then failwith "The list is empty"
  else
    l.Head.Fare < (List.last l).Fare

// ispisi
let rides = loadData ()

printfn "%A" rides

List.iter (fun x -> rideQuality x |> printfn "%s") rides 

firstVsLast rides |> printfn "%A"

divide (fun x -> x.RideId % 2 = 0) rides |> printfn "%A"
