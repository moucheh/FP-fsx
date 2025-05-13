open Feliz
open Elmish
open Elmish.React

// type definitions

type State =
  {
    firstOperand : string;
    operation : char;
    secondOperand : string;
    firstOperandEntered : bool;
  } 
  
type Msg =
  | Append of string 
  | Add
  | Sub
  | Mul
  | Div
  | Eq
  | CE

// helper functions

let calculate state op =
  if state.firstOperand = "NaN" then "NaN"
  elif state.firstOperand = "∞" && op <> '/' then "∞"
  elif state.firstOperand = "-∞" && op <> '/' then "-∞"
  elif state.secondOperand = "" then state.firstOperand
  else
      match op with
      | '+' -> (int64 state.firstOperand + int64 state.secondOperand) |> string
      | '-' -> (int64 state.firstOperand - int64 state.secondOperand) |> string
      | '*' -> (int64 state.firstOperand * int64 state.secondOperand) |> string
      | '/' -> 
        if state.secondOperand = "0" then "NaN"
        elif state.firstOperand = "∞" then "∞"
        elif state.firstOperand = "-∞" then "-∞"
        else (int64 state.firstOperand / int64 state.secondOperand) |> string
      | ' ' -> state.firstOperand
      | _ -> ""

let appendDigit digit state =
  if state.firstOperandEntered then
    if state.secondOperand.Length + 1 > 10 then
      state
    elif state.secondOperand = "0" || state.secondOperand = "" then
      { state with secondOperand = digit }
    else
      { state with secondOperand = state.secondOperand + digit }
  else
    if state.firstOperand = "NaN" then
      { state with firstOperand = digit }
    elif state.firstOperand = "-∞" || state.firstOperand = "∞" then
      { state with firstOperand = digit } 
    elif state.firstOperand = "0" then
      { state with firstOperand = digit }
    elif state.firstOperand.Length + 1 > 10 then
      state
    else
      { state with firstOperand = state.firstOperand + digit }

let handleOperationChange op state =
  if state.firstOperandEntered && state.secondOperand <> "" then
    let calculated = calculate state op 
    { state with firstOperand = calculated; operation = op; firstOperandEntered = false; }
  else
    { state with operation = op; firstOperandEntered = true }

let displayText state =
  if state.firstOperandEntered && state.secondOperand <> "" then
    state.secondOperand
  else
    state.firstOperand


// functions that create UI elements

let display state =
  Html.div [
        prop.className "display"
        prop.children [
          Html.label [
            state |> displayText |> prop.text 
          ]
        ]
      ]
 
let numberKey (number : string) dispatch =
  Html.button [
        prop.className "number" 
        prop.onClick (fun _ -> Append number |> dispatch) 
        prop.text number 
      ]

let operationKey (operation : string) (msg : Msg) dispatch =
  Html.button [
        prop.className "operation"
        prop.onClick (fun _ -> dispatch msg)
        prop.text operation 
      ]      

let init () = 
  {
    firstOperand = "0";
    operation = ' ';
    secondOperand = "";
    firstOperandEntered = false;
  }

let update msg state =
  match msg with
  | Append digit -> appendDigit digit state 
  | Add -> handleOperationChange '+' state
  | Sub -> handleOperationChange '-' state 
  | Mul -> handleOperationChange '*' state
  | Div -> handleOperationChange '/' state
  | CE -> init () 
  | Eq -> 
    let calculated = calculate state state.operation
    let result =
      if calculated.Length > 10 && (int64 calculated) > 0 then "∞"  
      elif calculated.Length > 10 && (int64 calculated) < 0 then "-∞"
      else calculated
    { state with firstOperand = result; operation = ' '; firstOperandEntered = false; secondOperand = "" } 
 
let view state dispatch =
  Html.div [
    prop.className "grid"
    prop.children [
      display state
      
      numberKey "1" dispatch
      numberKey "2" dispatch 
      numberKey "3" dispatch
      operationKey "+" Add dispatch
       
      numberKey "4" dispatch 
      numberKey "5" dispatch
      numberKey "6" dispatch
      operationKey "-" Sub dispatch
      
      numberKey "7" dispatch
      numberKey "8" dispatch
      numberKey "9" dispatch
      operationKey "*" Mul dispatch

      operationKey "CE" CE dispatch 
      numberKey "0" dispatch
      operationKey "=" Eq dispatch 
      operationKey "/" Div dispatch 
    ]
  ]

Program.mkSimple init update view
|> Program.withReactSynchronous "app"
|> Program.run
