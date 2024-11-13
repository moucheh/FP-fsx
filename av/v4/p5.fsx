// Napisati program koji učitava podatke o vremenu u New Yorku iz 2019-e godine.
// Podaci se nalaze u file-u "nyc_temperature.csv". Svaka linija file-a je data 
// u sljedećem formatu:
//
//date,tmax,tmin,tavg,departure,HDD,CDD,precipitation,new_snow,snow_depth
// date - datum mjerenja
// tmax - maksimalna zabiljezena temperatura u Fahrenheit-ima
// tmin - minimalna zabiljezena temperatura u Fahrenheit-ima
// tavg - prosjecna temperatura u Fahrenheit-ima
// departure - temperaturno odstupanje u odnosu na historijski prosjek
// HDD (Heating Degree Day) - mjera koja se koristi za estimiranje potrebne energije
//                              za zagrijavanje objekata
// CDD (Cooling Degree Day) - mjera koja se koristi za estimiranje potrebne energije
//                              za hladjenje objekata
// precipitation - kolicina padavina u incima
// new_snow - kolicina snijeznih padavina u incima
// snow_depth - dubina snijega u incima
//
// Izgraditi sljedeće strukture podataka na osnovu podataka iz file-a:
//    - Datum
//    - Padavine (Kolicina, Zanemarivo, Nikako)
//    - TempRecord - sadrzi informacije date u jednoj liniji mjerenja
//
// Na osnovu podataka iz file-a neophodno je izracunati:
//    - Kog dana je zabiljezena najvisa temperatura i njena vrijednost
//    - Kog dana je zabiljezena najmanja temperatura i njena vrijednost
//    - Prosjecna temperatura u toku godine
//    - Prosjecno temperaturno odstupanje u toku godine
//    - Kog dana je zabiljezeno najvece (apsolutno) temperaturno odstupanje i njena vrijednost
//    - Kog dana je palo najvise snijega i kolicina u cm
//    - Kog dana je zabiljezena najveca dubina snijega u cm
//    - U kom mjesecu je zabiljezena najvisa prosjecna temperatura i njena vrijednost
//    - U kom mjesecu je zabiljezena najniza prosjecna temperatura i njena vrijednost
//    - U kom mjesecu je estimiran najvisi prosjek neophodne energije
//    - Kog dana je zabiljezena najveca temperaturna razlika
//
// Podatke prikazivati u metrickim jedinicama, a temperaturu u stepenima Celzija.

type Datum = { dan : int; mjesec : int; godina : int }

type Padavine =
  | Kolicina of double
  | Zanemarivo
  | Nikako

type TempRecord = {
  datum : Datum;
  tmax : double;
  tmin : double;
  tavg : double;
  departure : double;
  hdd : double;
  cdd : double;
  precipitation : Padavine;
  new_snow : Padavine;
  snow_depth : Padavine
}

let pretvori_u_celzij f = (f - 32.) * 5./9.
pretvori_u_celzij

let pretvori_u_cm inch = inch * 2.54
pretvori_u_cm

let parsirajDatum ( datum : string) =
  match datum.Split '/' with
  | [|danStr; mjesecStr; godinaStr|] ->
    {
      dan = danStr |> int;
      mjesec = mjesecStr |> int;
      godina = godinaStr |> int
    }
  | _ -> failwith "Nevalidan datum"
parsirajDatum

let ispisi_mjesec = function
    | 1 -> "Januar"
    | 2 -> "Februar"
    | 3 -> "Mart"
    | 4 -> "April"
    | 5 -> "Maj"
    | 6 -> "Juni"
    | 7 -> "Juli"
    | 8 -> "August"
    | 9 -> "Septembar"
    | 10 -> "Oktobar"
    | 11 -> "Novembar"
    | 12 -> "Decembar"
    | _ -> "Mjesec ne postoji"

let ispisi_datum { dan = d; mjesec = m; godina = g } =
  
  sprintf $"{d}. {ispisi_mjesec m}, 20{g}."
ispisi_datum

let parsirajPadavine ( padavine : string ) : Padavine =
  match padavine with
  | "0" -> Nikako
  | "T" -> Zanemarivo
  | kolicina -> kolicina |> double |> pretvori_u_cm |> Kolicina
parsirajPadavine

let ucitaj_podatke ( u : unit) : List<TempRecord> =
  let headers = System.Console.ReadLine () |> ignore
  let rec foo () =
    let line = System.Console.ReadLine ()
    if line = null then []
    else 
      let result =
        match line.Split ',' with
        | [|datumStr; tmaxStr; tminStr; tavgStr; departureStr; hddStr; cddStr; precipitationStr; new_snowStr; snow_depthStr|] ->
          {
            datum = datumStr |> parsirajDatum;
            tmax = tmaxStr |> double |> pretvori_u_celzij;
            tmin = tminStr |> double |> pretvori_u_celzij;
            tavg = tavgStr |> double |> pretvori_u_celzij;
            departure = departureStr |> double;
            hdd = hddStr |> double;
            cdd = cddStr |> double;
            precipitation = precipitationStr |> parsirajPadavine;
            new_snow = new_snowStr |> parsirajPadavine;
            snow_depth = snow_depthStr |> parsirajPadavine
          }
        | _ -> failwith "Nevaildan unos" 
      result :: foo ()
  foo () 


let podaci = ucitaj_podatke ()

let najveca_temperatura = podaci |> List.maxBy (fun x -> x.tmax)
printfn $"Na dan {ispisi_datum najveca_temperatura.datum} je najveca temperatura bila {najveca_temperatura.tmax}" 

let najmanja_temperatura = podaci |> List.minBy (fun x -> x.tmin)
printfn $"Na dan {ispisi_datum najmanja_temperatura.datum} je najmanja temperatura bila {najmanja_temperatura.tmin}"

let prosjecna_temperatura = podaci |> List.averageBy (fun x -> x.tavg) 
printfn $"Prosjecna temperatura u toku godine iznosi {prosjecna_temperatura} C"

let prosjecno_odstupanje = podaci |> List.averageBy (fun x -> x.departure)
printfn $"Prosjecno odstupanje u toku godine iznosi {prosjecno_odstupanje}"

let najvece_temperaturno_odstupanje = 
  podaci |> List.maxBy (fun x -> if x.departure > 0. then x.departure else -x.departure)
printfn $"Najvece apsolutno odstupanje iznosi {najvece_temperaturno_odstupanje.departure} na dan {ispisi_datum najvece_temperaturno_odstupanje.datum}"

let matchPadavine x = 
  match x.snow_depth with
  | Zanemarivo -> 0.
  | Nikako -> 0.
  | Kolicina k -> k
matchPadavine

let najvise_snijega = podaci |> List.maxBy matchPadavine 
printfn $"Najvise snijega je palo na dan {ispisi_datum najvise_snijega.datum} i to {najvise_snijega.new_snow} cm"

let najdublji_snijeg = podaci |> List.maxBy matchPadavine
printfn $"Najdublji snijeg je bio na dan {ispisi_datum najdublji_snijeg.datum} i to dubine {najdublji_snijeg.snow_depth} cm"

let najvisa_prosjecna = podaci |> List.maxBy (fun x -> x.tavg)
printfn $"Najvisa prosjecna temperatura je bila u {ispisi_mjesec najvisa_prosjecna.datum.mjesec} i to {najvisa_prosjecna.tavg} C"

let najniza_prosjecna = podaci |> List.minBy (fun x -> x.tavg)
printfn $"Najniza prosjecna temperatura je bila u {ispisi_mjesec najniza_prosjecna.datum.mjesec} i to {najniza_prosjecna.tavg} C"

let najvisi_prosjek_neophodne_energije = podaci |> List.maxBy (fun x -> (x.hdd + x.cdd) / 2.) 
printfn $"Najvisi estimirani prosjek neophodne energije je bio u {ispisi_mjesec najvisi_prosjek_neophodne_energije.datum.mjesec}"

let najveca_temperaturna_razlika = podaci |> List.maxBy (fun x -> x.tmax - x.tmin)
printfn $"Najvisa temperaturna razlika je bila na dan {ispisi_datum najveca_temperaturna_razlika.datum} i to {najveca_temperaturna_razlika.tmax - najveca_temperaturna_razlika.tmin} C"
