open Feliz
open Elmish
open Elmish.React

let idseq = 
  seq {
    for i in 1..1000 do
      yield i
  }
  |> Seq.cache

let nextid =
  let enum = idseq.GetEnumerator ()

  let result () =
    if enum.MoveNext () then
      enum.Current
    else 
      failwith "All ids are in use"
  result

type Combination = 
  | BoardGame
  | FPS
  | Sports
  | CarManufacturer

type Card =
  { 
    id : int;
    image : string;
    selected : bool;
    shake : bool;
    guessed : bool;
    combination : Combination;
    disabled : bool
  }

  static member makeCard (src : string) (comb : Combination) : Card =
    {
      id = nextid ()
      image = src;
      combination = comb;
      selected = false;
      shake = false;
      guessed = false;
      disabled = false
    }

type Game = 
  { 
    cardsToGuess : list<Card>;
    mistakesAllowed : int;
    combinationsGuessed : list<Combination>
  }

type State =
  | Play of Game
  | GoodGuess of Game
  | WrongGuess of Game
  | OneAway of Game
  | GameOver of Game
  | GameWon of Game

type Msg =
  | CardClicked of Card
  | DeselectAll
  | Shuffle
  | EndGame

let selectCard (card : Card) (cards : list<Card>) : list<Card> =
  let select (c : Card) =
    if c.id = card.id then
      { c with selected = true; shake = false }
    else c

  cards |> List.map select

let removeShake (cards : list<Card>) : list<Card> =
  let _removeShake (c : Card) = { c with shake = false }

  cards |> List.map _removeShake

let guessCards (cards : list<Card>) : list<Card> =
  let guess (c : Card) = 
    if c.selected then
      { c with guessed = true; shake = false }
    else c

  cards |> List.map guess

let shakeCards (cards : list<Card>) : list<Card> =
  let shake (c : Card) =
      if c.selected then
        { c with shake = true }
      else c

  cards |> List.map shake

let deselectCards (cards : list<Card>) : list<Card> =
  let deselect (c : Card) = { c with selected = false }

  cards |> List.map deselect

let countSelected (cards : list<Card>) : int =
  cards |> List.filter (fun x -> x.selected) |> List.length

let shuffle (cl : list<Card>) : list<Card> =
  let random = System.Random ()
  cl |> List.sortBy (fun _ -> random.Next(0, 15))
     |> List.sortBy (fun _ -> random.Next(0, 15))
     |> List.sortBy (fun _ -> random.Next(0, 15))

let combo1 : list<Card> =
  [
    Card.makeCard "tennis.jpg" Sports
    Card.makeCard "basketball.jpg" Sports
    Card.makeCard "boxing.png" Sports
    Card.makeCard "football.jpg" Sports
  ]

let combo2 : list<Card> =
  [
    Card.makeCard "bfv.jpg" FPS
    Card.makeCard "cs2.png" FPS
    Card.makeCard "wolfenstein.jpg" FPS
    Card.makeCard "codbo6.jpg" FPS
  ]

let combo3 : list<Card> =
  [
    Card.makeCard "bkngmn.png" BoardGame
    Card.makeCard "chess.jpg" BoardGame
    Card.makeCard "mahjong.png" BoardGame
    Card.makeCard "cnlj.jpg" BoardGame
  ]

let combo4 : list<Card> =
  [
    Card.makeCard "bmw.png" CarManufacturer
    Card.makeCard "koenigsegg.png" CarManufacturer
    Card.makeCard "lambo.png" CarManufacturer
    Card.makeCard "porsche.jpg" CarManufacturer
  ]
 
let init () : State * Cmd<'Msg> =
    let cards = combo1 @ combo2 @ combo3 @ combo4

    let initState = 
      { cardsToGuess = shuffle cards; mistakesAllowed = 4; combinationsGuessed = [] }

    Play initState, Cmd.none

let update (msg : Msg) (state : State) : State * Cmd<Msg> =
  let wait2sec () = async {
    do! Async.Sleep 2000
  }

  let waitHalfSec () = async {
    do! Async.Sleep 500
  }

  match state with
  | Play gameState ->
    match msg with
      | CardClicked card ->
          let newCards = selectCard card gameState.cardsToGuess

          if countSelected gameState.cardsToGuess < 3 then
            Play { gameState with cardsToGuess = newCards }, Cmd.none

          else
            let selectedCards = List.filter (fun x -> x.selected) newCards
            let combinationsCounted = 
              List.countBy (fun x -> x.combination) selectedCards 
              |> List.sortByDescending (fun x -> snd x)

            if snd combinationsCounted[0] = 4 then
              let comb : Combination = (fst combinationsCounted[0])
              let newCombs : list<Combination> = 
                if List.contains comb gameState.combinationsGuessed then
                  gameState.combinationsGuessed
                else
                  List.append gameState.combinationsGuessed [ comb ]

              let newState = 
                { gameState with
                    cardsToGuess = newCards |> guessCards;
                    combinationsGuessed = newCombs
                }

              GoodGuess newState,
              Cmd.OfAsync.perform waitHalfSec () (fun () -> DeselectAll)
            elif snd combinationsCounted[0] = 3 then
              printfn "One Away!"
              let shakenCards = shakeCards newCards
              let newState =
                { gameState with 
                    cardsToGuess = shakenCards |> deselectCards;
                    mistakesAllowed = gameState.mistakesAllowed - 1 
                }

              OneAway newState,
              Cmd.OfAsync.perform wait2sec () (fun () -> DeselectAll) 

            else
              let shakenCards = shakeCards newCards

              let newState =
                { gameState with
                    cardsToGuess = shakenCards |> deselectCards;
                    mistakesAllowed = gameState.mistakesAllowed - 1
                }

              WrongGuess newState,
              Cmd.OfAsync.perform wait2sec () (fun () -> DeselectAll) 
      | DeselectAll ->
        let newCards = gameState.cardsToGuess |> deselectCards |> removeShake
        Play { gameState with cardsToGuess = newCards }, Cmd.none
      | Shuffle ->
        Play { gameState with cardsToGuess = shuffle gameState.cardsToGuess }, Cmd.none
      | _ -> Play gameState, Cmd.none
        
  | GoodGuess gameState ->
    printfn "Good guess!"
    match msg with
    | DeselectAll ->
      let partition = List.partition (fun (x : Card) -> x.guessed) gameState.cardsToGuess
      let newCards = fst partition @ snd partition |> removeShake |> deselectCards

      if gameState.combinationsGuessed.Length = 4 then
        GameWon gameState, Cmd.none
      else
        Play { gameState with cardsToGuess = newCards }, Cmd.none
    | _ -> Play gameState, Cmd.none
  | WrongGuess gameState -> 
    printfn "Wrong guess!"
    printfn "Mistakes allowed: %d" gameState.mistakesAllowed
    match msg with
    | DeselectAll ->
      if gameState.mistakesAllowed = 0 then
        GameOver gameState, Cmd.OfAsync.perform wait2sec () (fun () -> EndGame)
      else
        let newCards = gameState.cardsToGuess |> deselectCards |> removeShake
        Play { gameState with cardsToGuess = newCards }, Cmd.none
    | _ -> Play gameState, Cmd.none

  | OneAway gameState ->
    printfn "One Away!"
    printfn "Mistakes allowed: %d" gameState.mistakesAllowed

    if gameState.mistakesAllowed = 0 then
      GameOver gameState, Cmd.OfAsync.perform wait2sec () (fun () -> EndGame)
    else
      let newCards = gameState.cardsToGuess |> deselectCards |> removeShake

      Play { gameState with cardsToGuess = newCards }, Cmd.none
  | GameOver gameState ->
    printfn "Game over!"
    match msg with
    | EndGame ->
      let lastCards = List.map (fun (c : Card) -> { c with disabled = true }) gameState.cardsToGuess
      GameOver { gameState with cardsToGuess = lastCards }, Cmd.none
    | _ -> GameOver gameState, Cmd.none
  | GameWon gameState ->
    printfn "Game won!"
    match msg with
    | EndGame ->
      GameWon gameState, Cmd.none 
    | _ -> GameWon gameState, Cmd.none

let view (state : State) (dispatch : Msg -> unit) : ReactElement =
  let card (c : Card) =
      let cardClasses =
          let defaultClass = [ "card" ]

          let addSelected =
            if c.selected then
              List.append [ "selected-card" ] defaultClass
            else
              defaultClass

          let addGuessed =
            if c.guessed then
              List.append [ "guessed-card" ] addSelected
            else
              addSelected

          let addShake =
            if c.shake then
              List.append [ "shake-card" ] addGuessed
            else
              addGuessed

          if c.disabled then
            [ "card"; "disabled"]  
          else
            addShake

      Html.div [ 
         prop.classes cardClasses
         prop.children [
           Html.img [ 
             prop.src $"/img/{c.image}"
             prop.alt "Card"
           ] 
         ]
         prop.onClick (fun _ -> c |> CardClicked |> dispatch)
      ]

  let cards () : list<ReactElement> =
    let result =
        match state with
        | Play game -> game.cardsToGuess
        | GoodGuess game -> game.cardsToGuess
        | WrongGuess game -> game.cardsToGuess
        | OneAway game -> game.cardsToGuess
        | GameOver game -> game.cardsToGuess
        | GameWon game -> game.cardsToGuess
        |> List.map card
    result

 
  let description (comb : Combination) : ReactElement =
    let _description (text : string * string) : ReactElement = 
      Html.div [  
        prop.className "title"
        prop.children [
          Html.div [
            text |> fst |> prop.text
          ]
          Html.div [
            prop.className "subtitle"
            text |> snd |> prop.text
          ]
        ]
      ]

    let combinationToDescription (comb : Combination) : string * string =
      match comb with
      | FPS -> "First person shooters", " CS2, COD BO6, Wolfenstein, Battlefield V"
      | Sports -> "Sports", " Football, Basketball, Tennis, Boxing"
      | BoardGame -> "Board games", "Chess, Backgammon, Man don't get angry, Mahjong"
      | CarManufacturer -> "Car Manufacturers", "BMW, Lamborghini, Koenigsegg, Porsche"
    comb |> combinationToDescription |> _description

  let descriptions () : list<ReactElement> =
    let result =
      match state with
      | Play game -> game.combinationsGuessed
      | GoodGuess game -> game.combinationsGuessed
      | WrongGuess game -> game.combinationsGuessed
      | OneAway game -> game.combinationsGuessed
      | GameOver game -> game.combinationsGuessed
      | GameWon game -> game.combinationsGuessed
      |> List.map description
    result

  let grid () =
    Html.div [ 
      prop.className "grid"
      descriptions () @ cards () |> prop.children
    ]
  
  let button (text : string) (msg : Msg) dispatch : ReactElement =
    Html.button [
      prop.className "button"
      prop.text text
      prop.onClick (fun _ -> dispatch msg)
    ]

  let mistakesCount = 
    match state with
    | Play gameState -> gameState.mistakesAllowed
    | GoodGuess gameState -> gameState.mistakesAllowed
    | WrongGuess gameState -> gameState.mistakesAllowed
    | OneAway gameState -> gameState.mistakesAllowed
    | GameOver gameState -> gameState.mistakesAllowed
    | GameWon gameState -> gameState.mistakesAllowed

  let mistakesIndicator (count : int) : ReactElement =
    let mistakesIndicatorText : ReactElement =
      Html.div [
        prop.className "mistakes-indicator"
        prop.text "Mistakes allowed"
      ]

    let dot () : ReactElement =
      Html.div [
        prop.className "dot"
      ]

    let dots () : ReactElement =
      Html.div [
        prop.className "dots"
        List.init count (fun _ -> dot ()) |> prop.children
      ]

    Html.div [
      prop.children [
        mistakesIndicatorText
        dots ()
      ]
    ]

  let gameWon () : ReactElement =
    let _gameWon =
      match state with
      | GameWon gameState -> true
      | _ -> false
    if _gameWon then
      Html.div [
        prop.className "game-won"
        prop.children [
          Html.p [
            prop.text "Game won!"
          ]
        ]
      ]
    else
      Html.div [
        prop.text ""
      ]

  let gameOver () : ReactElement =
    let _gameOver =
      match state with
      | GameOver gameState -> true
      | _ -> false
    if _gameOver then
      Html.div [
        prop.className "game-over"
        prop.text "Game over!"
      ]
    else
      Html.div [
        prop.text ""
      ]


  let oneAway () : ReactElement =
    let _oneAway =
      match state with
      | OneAway gameState -> true
      | _ -> false

    if _oneAway then
      Html.div [
        prop.className "one-away"
        prop.text "One away!"
      ]
    else
      Html.div [
        prop.className "one-away-placeholder"
        prop.text "One Away!"
      ]

  Html.div [
    prop.className "container"
    prop.children [ 
      gameWon ()
      gameOver ()
      oneAway ()
      grid ()
      Html.div [
        prop.className "button-container"
        prop.children [
          button "Shuffle" Shuffle dispatch 
          button "Deselect" DeselectAll dispatch
        ]
      ]
      mistakesIndicator mistakesCount
    ] 
  ]

Program.mkProgram init update view
|> Program.withReactSynchronous "app"
|> Program.run
