// Napisati funkcije na odgovarajućim mjestima
let pricesInUSD = [100.0; 250.0; 75.0; 300.0]

// 1. Implementirati funkciju konverzije (closure)
let convertToBAM (exchangeRate : float) : (float -> float) =
    // Vaš kod ovdje
    let apply x = x * exchangeRate
    apply

// 2. Implementirati funkciju popusta (closure)
let applyDiscount (discountRate : float) : (float -> float) =
    // Vaš kod ovdje
    let apply x = x - x * discountRate
    apply


// 3. Implementirati funkciju provizije
let addProvisionFee (provisionFee : float) : (float -> float) =
    // Vaš kod ovdje
    let apply x = x + x * provisionFee
    apply

// 3. Implementirati funkciju formatiranja
let formatBAM (price : float) : string =
    // Vaš kod ovdje (hint: use sprintf)
    string price

// 4. Kompozicija funkcija
let processPrices = 
    // Vaš kod ovdje
    convertToBAM 1.81 >> applyDiscount 0.10 >> addProvisionFee 0.01 >> formatBAM

// 5. Transformisati i ispisati rezultate
let finalPrices = List.map processPrices pricesInUSD
List.iter (printfn "%s") finalPrices
