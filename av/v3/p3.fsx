// Zasto se kod ispod ne kompajlira?

let people = [("Ena", 20); ("Sara", 25); ("Denis", 30); ("Mak", 35)]
let format name age = sprintf $"{name} ima {age} godina."

let formattedPeople = people |> 
                      List.map ( fun x -> x ||> format )
                      // trebalo je promjeniti |> u ||>

formattedPeople |> List.iter (printfn "%s")


