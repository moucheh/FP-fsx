import { Union, Record } from "./fable_modules/fable-library-js.4.19.3/Types.js";
import { union_type, record_type, bool_type, char_type, string_type } from "./fable_modules/fable-library-js.4.19.3/Reflection.js";
import { createObj, int64ToString } from "./fable_modules/fable-library-js.4.19.3/Util.js";
import { fromInt32, compare, op_Division, op_Subtraction, op_Addition, op_Multiply, toInt64 } from "./fable_modules/fable-library-js.4.19.3/BigInt.js";
import { parse } from "./fable_modules/fable-library-js.4.19.3/Long.js";
import { createElement } from "react";
import { reactApi } from "./fable_modules/Feliz.2.9.0/Interop.fs.js";
import { ofArray } from "./fable_modules/fable-library-js.4.19.3/List.js";
import { ProgramModule_mkSimple, ProgramModule_run } from "./fable_modules/Fable.Elmish.4.0.0/program.fs.js";
import { Program_withReactSynchronous } from "./fable_modules/Fable.Elmish.React.4.0.0/react.fs.js";

export class State extends Record {
    constructor(firstOperand, operation, secondOperand, firstOperandEntered) {
        super();
        this.firstOperand = firstOperand;
        this.operation = operation;
        this.secondOperand = secondOperand;
        this.firstOperandEntered = firstOperandEntered;
    }
}

export function State_$reflection() {
    return record_type("Program.State", [], State, () => [["firstOperand", string_type], ["operation", char_type], ["secondOperand", string_type], ["firstOperandEntered", bool_type]]);
}

export class Msg extends Union {
    constructor(tag, fields) {
        super();
        this.tag = tag;
        this.fields = fields;
    }
    cases() {
        return ["Append", "Add", "Sub", "Mul", "Div", "Eq", "CE"];
    }
}

export function Msg_$reflection() {
    return union_type("Program.Msg", [], Msg, () => [[["Item", string_type]], [], [], [], [], [], []]);
}

export function calculate(state, op) {
    if (state.firstOperand === "NaN") {
        return "NaN";
    }
    else if ((state.firstOperand === "∞") && (op !== "/")) {
        return "∞";
    }
    else if ((state.firstOperand === "-∞") && (op !== "/")) {
        return "-∞";
    }
    else if (state.secondOperand === "") {
        return state.firstOperand;
    }
    else {
        switch (op) {
            case " ":
                return state.firstOperand;
            case "*":
                return int64ToString(toInt64(op_Multiply(toInt64(parse(state.firstOperand, 511, false, 64)), toInt64(parse(state.secondOperand, 511, false, 64)))));
            case "+":
                return int64ToString(toInt64(op_Addition(toInt64(parse(state.firstOperand, 511, false, 64)), toInt64(parse(state.secondOperand, 511, false, 64)))));
            case "-":
                return int64ToString(toInt64(op_Subtraction(toInt64(parse(state.firstOperand, 511, false, 64)), toInt64(parse(state.secondOperand, 511, false, 64)))));
            case "/":
                if (state.secondOperand === "0") {
                    return "NaN";
                }
                else {
                    switch (state.firstOperand) {
                        case "∞":
                            return "∞";
                        case "-∞":
                            return "-∞";
                        default:
                            return int64ToString(toInt64(op_Division(toInt64(parse(state.firstOperand, 511, false, 64)), toInt64(parse(state.secondOperand, 511, false, 64)))));
                    }
                }
            default:
                return "";
        }
    }
}

export function appendDigit(digit, state) {
    if (state.firstOperandEntered) {
        if ((state.secondOperand.length + 1) > 11) {
            return state;
        }
        else if ((state.secondOperand === "0") ? true : (state.secondOperand === "")) {
            return new State(state.firstOperand, state.operation, digit, state.firstOperandEntered);
        }
        else {
            return new State(state.firstOperand, state.operation, state.secondOperand + digit, state.firstOperandEntered);
        }
    }
    else if (state.firstOperand === "NaN") {
        return new State(digit, state.operation, state.secondOperand, state.firstOperandEntered);
    }
    else if ((state.firstOperand === "-∞") ? true : (state.firstOperand === "∞")) {
        return new State(digit, state.operation, state.secondOperand, state.firstOperandEntered);
    }
    else if (state.firstOperand === "0") {
        return new State(digit, state.operation, state.secondOperand, state.firstOperandEntered);
    }
    else if ((state.firstOperand.length + 1) > 10) {
        return state;
    }
    else {
        return new State(state.firstOperand + digit, state.operation, state.secondOperand, state.firstOperandEntered);
    }
}

export function handleOperationChange(op, state) {
    if (state.firstOperandEntered && (state.secondOperand !== "")) {
        const calculated = calculate(state, op);
        return new State(calculated, op, state.secondOperand, false);
    }
    else {
        return new State(state.firstOperand, op, state.secondOperand, true);
    }
}

export function displayText(state) {
    if (state.firstOperandEntered && (state.secondOperand !== "")) {
        return state.secondOperand;
    }
    else {
        return state.firstOperand;
    }
}

export function display(state) {
    let elems;
    return createElement("div", createObj(ofArray([["className", "display"], (elems = [createElement("label", {
        children: displayText(state),
    })], ["children", reactApi.Children.toArray(Array.from(elems))])])));
}

export function numberKey(number, dispatch) {
    return createElement("button", {
        className: "number",
        onClick: (_arg) => {
            dispatch(new Msg(0, [number]));
        },
        children: number,
    });
}

export function operationKey(operation, msg, dispatch) {
    return createElement("button", {
        className: "operation",
        onClick: (_arg) => {
            dispatch(msg);
        },
        children: operation,
    });
}

export function init() {
    return new State("0", " ", "", false);
}

export function update(msg, state) {
    switch (msg.tag) {
        case 1:
            return handleOperationChange("+", state);
        case 2:
            return handleOperationChange("-", state);
        case 3:
            return handleOperationChange("*", state);
        case 4:
            return handleOperationChange("/", state);
        case 6:
            return init();
        case 5: {
            const calculated = calculate(state, state.operation);
            const result = ((calculated.length > 10) && (compare(toInt64(parse(calculated, 511, false, 64)), toInt64(fromInt32(0))) > 0)) ? "∞" : (((calculated.length > 10) && (compare(toInt64(parse(calculated, 511, false, 64)), toInt64(fromInt32(0))) < 0)) ? "-∞" : calculated);
            return new State(result, " ", "", false);
        }
        default: {
            const digit = msg.fields[0];
            return appendDigit(digit, state);
        }
    }
}

export function view(state, dispatch) {
    let elems;
    return createElement("div", createObj(ofArray([["className", "grid"], (elems = [display(state), numberKey("1", dispatch), numberKey("2", dispatch), numberKey("3", dispatch), operationKey("+", new Msg(1, []), dispatch), numberKey("4", dispatch), numberKey("5", dispatch), numberKey("6", dispatch), operationKey("-", new Msg(2, []), dispatch), numberKey("7", dispatch), numberKey("8", dispatch), numberKey("9", dispatch), operationKey("*", new Msg(3, []), dispatch), operationKey("CE", new Msg(6, []), dispatch), numberKey("0", dispatch), operationKey("=", new Msg(5, []), dispatch), operationKey("/", new Msg(4, []), dispatch)], ["children", reactApi.Children.toArray(Array.from(elems))])])));
}

ProgramModule_run(Program_withReactSynchronous("app", ProgramModule_mkSimple(init, update, view)));

