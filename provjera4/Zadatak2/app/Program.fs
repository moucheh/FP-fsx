open Feliz
open Elmish
open Elmish.React

type state = string

type Msg =
  | NumberClicked of string

let init (u : unit) : string = ""

let appendDigit (digit : string) (state : string) : string =
  state + digit

let update msg state =
  match msg with
  | NumberClicked number -> 
    printfn "Number %s" number
    appendDigit number state

let digitButton (digit : string) dispatch : ReactElement =
  Html.button [
    prop.className "digit-button"
    prop.text digit
    prop.onClick (fun _ -> digit |> NumberClicked |> dispatch)
  ]

let view (state : state) dispatch = 
  Html.div [
    prop.className "grid"
    prop.children [
      Html.div [
        prop.text state
        prop.className "display"
      ]
      digitButton "0" dispatch
      digitButton "1" dispatch
      digitButton "2" dispatch
      digitButton "3" dispatch
      digitButton "4" dispatch
      digitButton "5" dispatch
      digitButton "6" dispatch
      digitButton "7" dispatch
      digitButton "8" dispatch
      digitButton "9" dispatch
  ]
  ] 

Program.mkSimple init update view
|> Program.withReactSynchronous "app"
|> Program.run
