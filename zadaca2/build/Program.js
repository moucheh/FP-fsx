import { map, delay, cache } from "./fable_modules/fable-library-js.4.19.3/Seq.js";
import { rangeDouble } from "./fable_modules/fable-library-js.4.19.3/Range.js";
import { createObj, safeHash, equals, comparePrimitives, getEnumerator } from "./fable_modules/fable-library-js.4.19.3/Util.js";
import { Record, Union } from "./fable_modules/fable-library-js.4.19.3/Types.js";
import { list_type, record_type, bool_type, string_type, int32_type, union_type } from "./fable_modules/fable-library-js.4.19.3/Reflection.js";
import { initialize, singleton as singleton_1, contains, item, sortByDescending, partition as partition_1, empty, append, ofArray, sortBy, filter, length, map as map_1 } from "./fable_modules/fable-library-js.4.19.3/List.js";
import { nonSeeded } from "./fable_modules/fable-library-js.4.19.3/Random.js";
import { Cmd_none } from "./fable_modules/Fable.Elmish.4.0.0/cmd.fs.js";
import { singleton } from "./fable_modules/fable-library-js.4.19.3/AsyncBuilder.js";
import { sleep } from "./fable_modules/fable-library-js.4.19.3/Async.js";
import { join, printf, toConsole } from "./fable_modules/fable-library-js.4.19.3/String.js";
import { Cmd_OfAsync_start, Cmd_OfAsyncWith_perform } from "./fable_modules/Fable.Elmish.4.0.0/cmd.fs.js";
import { List_countBy } from "./fable_modules/fable-library-js.4.19.3/Seq2.js";
import { createElement } from "react";
import { reactApi } from "./fable_modules/Feliz.2.9.0/Interop.fs.js";
import { ProgramModule_mkProgram, ProgramModule_run } from "./fable_modules/Fable.Elmish.4.0.0/program.fs.js";
import { Program_withReactSynchronous } from "./fable_modules/Fable.Elmish.React.4.0.0/react.fs.js";

export const idseq = cache(delay(() => map((i) => i, rangeDouble(1, 1, 1000))));

export const nextid = (() => {
    const enum$ = getEnumerator(idseq);
    const result = () => {
        if (enum$["System.Collections.IEnumerator.MoveNext"]()) {
            return enum$["System.Collections.Generic.IEnumerator`1.get_Current"]() | 0;
        }
        else {
            throw new Error("All ids are in use");
        }
    };
    return result;
})();

export class Combination extends Union {
    constructor(tag, fields) {
        super();
        this.tag = tag;
        this.fields = fields;
    }
    cases() {
        return ["BoardGame", "FPS", "Sports", "CarManufacturer"];
    }
}

export function Combination_$reflection() {
    return union_type("Program.Combination", [], Combination, () => [[], [], [], []]);
}

export class Card extends Record {
    constructor(id, image, selected, shake, guessed, combination, disabled) {
        super();
        this.id = (id | 0);
        this.image = image;
        this.selected = selected;
        this.shake = shake;
        this.guessed = guessed;
        this.combination = combination;
        this.disabled = disabled;
    }
}

export function Card_$reflection() {
    return record_type("Program.Card", [], Card, () => [["id", int32_type], ["image", string_type], ["selected", bool_type], ["shake", bool_type], ["guessed", bool_type], ["combination", Combination_$reflection()], ["disabled", bool_type]]);
}

export function Card_makeCard(src, comb) {
    return new Card(nextid(), src, false, false, false, comb, false);
}

export class Game extends Record {
    constructor(cardsToGuess, mistakesAllowed, combinationsGuessed) {
        super();
        this.cardsToGuess = cardsToGuess;
        this.mistakesAllowed = (mistakesAllowed | 0);
        this.combinationsGuessed = combinationsGuessed;
    }
}

export function Game_$reflection() {
    return record_type("Program.Game", [], Game, () => [["cardsToGuess", list_type(Card_$reflection())], ["mistakesAllowed", int32_type], ["combinationsGuessed", list_type(Combination_$reflection())]]);
}

export class State extends Union {
    constructor(tag, fields) {
        super();
        this.tag = tag;
        this.fields = fields;
    }
    cases() {
        return ["Play", "GoodGuess", "WrongGuess", "OneAway", "GameOver", "GameWon"];
    }
}

export function State_$reflection() {
    return union_type("Program.State", [], State, () => [[["Item", Game_$reflection()]], [["Item", Game_$reflection()]], [["Item", Game_$reflection()]], [["Item", Game_$reflection()]], [["Item", Game_$reflection()]], [["Item", Game_$reflection()]]]);
}

export class Msg extends Union {
    constructor(tag, fields) {
        super();
        this.tag = tag;
        this.fields = fields;
    }
    cases() {
        return ["CardClicked", "DeselectAll", "Shuffle", "EndGame"];
    }
}

export function Msg_$reflection() {
    return union_type("Program.Msg", [], Msg, () => [[["Item", Card_$reflection()]], [], [], []]);
}

export function selectCard(card, cards) {
    const select = (c) => {
        if (c.id === card.id) {
            return new Card(c.id, c.image, true, false, c.guessed, c.combination, c.disabled);
        }
        else {
            return c;
        }
    };
    return map_1(select, cards);
}

export function removeShake(cards) {
    const _removeShake = (c) => (new Card(c.id, c.image, c.selected, false, c.guessed, c.combination, c.disabled));
    return map_1(_removeShake, cards);
}

export function guessCards(cards) {
    const guess = (c) => {
        if (c.selected) {
            return new Card(c.id, c.image, c.selected, false, true, c.combination, c.disabled);
        }
        else {
            return c;
        }
    };
    return map_1(guess, cards);
}

export function shakeCards(cards) {
    const shake_1 = (c) => {
        if (c.selected) {
            return new Card(c.id, c.image, c.selected, true, c.guessed, c.combination, c.disabled);
        }
        else {
            return c;
        }
    };
    return map_1(shake_1, cards);
}

export function deselectCards(cards) {
    const deselect = (c) => (new Card(c.id, c.image, false, c.shake, c.guessed, c.combination, c.disabled));
    return map_1(deselect, cards);
}

export function countSelected(cards) {
    return length(filter((x) => x.selected, cards));
}

export function shuffle(cl) {
    const random = nonSeeded();
    return sortBy((_arg_2) => random.Next2(0, 15), sortBy((_arg_1) => random.Next2(0, 15), sortBy((_arg) => random.Next2(0, 15), cl, {
        Compare: comparePrimitives,
    }), {
        Compare: comparePrimitives,
    }), {
        Compare: comparePrimitives,
    });
}

export const combo1 = ofArray([Card_makeCard("tennis.jpg", new Combination(2, [])), Card_makeCard("basketball.jpg", new Combination(2, [])), Card_makeCard("boxing.png", new Combination(2, [])), Card_makeCard("football.jpg", new Combination(2, []))]);

export const combo2 = ofArray([Card_makeCard("bfv.jpg", new Combination(1, [])), Card_makeCard("cs2.png", new Combination(1, [])), Card_makeCard("wolfenstein.jpg", new Combination(1, [])), Card_makeCard("codbo6.jpg", new Combination(1, []))]);

export const combo3 = ofArray([Card_makeCard("bkngmn.png", new Combination(0, [])), Card_makeCard("chess.jpg", new Combination(0, [])), Card_makeCard("mahjong.png", new Combination(0, [])), Card_makeCard("cnlj.jpg", new Combination(0, []))]);

export const combo4 = ofArray([Card_makeCard("bmw.png", new Combination(3, [])), Card_makeCard("koenigsegg.png", new Combination(3, [])), Card_makeCard("lambo.png", new Combination(3, [])), Card_makeCard("porsche.jpg", new Combination(3, []))]);

export function init() {
    const cards = append(combo1, append(combo2, append(combo3, combo4)));
    const initState = new Game(shuffle(cards), 4, empty());
    return [new State(0, [initState]), Cmd_none()];
}

export function update(msg, state) {
    const wait2sec = () => singleton.Delay(() => singleton.Bind(sleep(2000), () => singleton.Return(undefined)));
    const waitHalfSec = () => singleton.Delay(() => singleton.Bind(sleep(500), () => singleton.Return(undefined)));
    switch (state.tag) {
        case 1: {
            const gameState_1 = state.fields[0];
            toConsole(printf("Good guess!"));
            if (msg.tag === 1) {
                const partition = partition_1((x_9) => x_9.guessed, gameState_1.cardsToGuess);
                const newCards_2 = deselectCards(removeShake(append(partition[0], partition[1])));
                if (length(gameState_1.combinationsGuessed) === 4) {
                    return [new State(5, [gameState_1]), Cmd_none()];
                }
                else {
                    return [new State(0, [new Game(newCards_2, gameState_1.mistakesAllowed, gameState_1.combinationsGuessed)]), Cmd_none()];
                }
            }
            else {
                return [new State(0, [gameState_1]), Cmd_none()];
            }
        }
        case 2: {
            const gameState_2 = state.fields[0];
            toConsole(printf("Wrong guess!"));
            toConsole(printf("Mistakes allowed: %d"))(gameState_2.mistakesAllowed);
            if (msg.tag === 1) {
                if (gameState_2.mistakesAllowed === 0) {
                    return [new State(4, [gameState_2]), Cmd_OfAsyncWith_perform((x_10) => {
                        Cmd_OfAsync_start(x_10);
                    }, wait2sec, undefined, () => (new Msg(3, [])))];
                }
                else {
                    const newCards_3 = removeShake(deselectCards(gameState_2.cardsToGuess));
                    return [new State(0, [new Game(newCards_3, gameState_2.mistakesAllowed, gameState_2.combinationsGuessed)]), Cmd_none()];
                }
            }
            else {
                return [new State(0, [gameState_2]), Cmd_none()];
            }
        }
        case 3: {
            const gameState_3 = state.fields[0];
            toConsole(printf("One Away!"));
            toConsole(printf("Mistakes allowed: %d"))(gameState_3.mistakesAllowed);
            if (gameState_3.mistakesAllowed === 0) {
                return [new State(4, [gameState_3]), Cmd_OfAsyncWith_perform((x_11) => {
                    Cmd_OfAsync_start(x_11);
                }, wait2sec, undefined, () => (new Msg(3, [])))];
            }
            else {
                const newCards_4 = removeShake(deselectCards(gameState_3.cardsToGuess));
                return [new State(0, [new Game(newCards_4, gameState_3.mistakesAllowed, gameState_3.combinationsGuessed)]), Cmd_none()];
            }
        }
        case 4: {
            const gameState_4 = state.fields[0];
            toConsole(printf("Game over!"));
            if (msg.tag === 3) {
                const lastCards = map_1((c) => (new Card(c.id, c.image, c.selected, c.shake, c.guessed, c.combination, true)), gameState_4.cardsToGuess);
                return [new State(4, [new Game(lastCards, gameState_4.mistakesAllowed, gameState_4.combinationsGuessed)]), Cmd_none()];
            }
            else {
                return [new State(4, [gameState_4]), Cmd_none()];
            }
        }
        case 5: {
            const gameState_5 = state.fields[0];
            toConsole(printf("Game won!"));
            if (msg.tag === 3) {
                return [new State(5, [gameState_5]), Cmd_none()];
            }
            else {
                return [new State(5, [gameState_5]), Cmd_none()];
            }
        }
        default: {
            const gameState = state.fields[0];
            switch (msg.tag) {
                case 0: {
                    const card = msg.fields[0];
                    const newCards = selectCard(card, gameState.cardsToGuess);
                    if (countSelected(gameState.cardsToGuess) < 3) {
                        return [new State(0, [new Game(newCards, gameState.mistakesAllowed, gameState.combinationsGuessed)]), Cmd_none()];
                    }
                    else {
                        const selectedCards = filter((x) => x.selected, newCards);
                        const combinationsCounted = sortByDescending((x_3) => x_3[1], List_countBy((x_1) => x_1.combination, selectedCards, {
                            Equals: equals,
                            GetHashCode: safeHash,
                        }), {
                            Compare: comparePrimitives,
                        });
                        if (item(0, combinationsCounted)[1] === 4) {
                            const comb = item(0, combinationsCounted)[0];
                            const newCombs = contains(comb, gameState.combinationsGuessed, {
                                Equals: equals,
                                GetHashCode: safeHash,
                            }) ? gameState.combinationsGuessed : append(gameState.combinationsGuessed, singleton_1(comb));
                            const newState = new Game(guessCards(newCards), gameState.mistakesAllowed, newCombs);
                            return [new State(1, [newState]), Cmd_OfAsyncWith_perform((x_6) => {
                                Cmd_OfAsync_start(x_6);
                            }, waitHalfSec, undefined, () => (new Msg(1, [])))];
                        }
                        else if (item(0, combinationsCounted)[1] === 3) {
                            toConsole(printf("One Away!"));
                            const shakenCards = shakeCards(newCards);
                            const newState_1 = new Game(deselectCards(shakenCards), gameState.mistakesAllowed - 1, gameState.combinationsGuessed);
                            return [new State(3, [newState_1]), Cmd_OfAsyncWith_perform((x_7) => {
                                Cmd_OfAsync_start(x_7);
                            }, wait2sec, undefined, () => (new Msg(1, [])))];
                        }
                        else {
                            const shakenCards_1 = shakeCards(newCards);
                            const newState_2 = new Game(deselectCards(shakenCards_1), gameState.mistakesAllowed - 1, gameState.combinationsGuessed);
                            return [new State(2, [newState_2]), Cmd_OfAsyncWith_perform((x_8) => {
                                Cmd_OfAsync_start(x_8);
                            }, wait2sec, undefined, () => (new Msg(1, [])))];
                        }
                    }
                }
                case 1: {
                    const newCards_1 = removeShake(deselectCards(gameState.cardsToGuess));
                    return [new State(0, [new Game(newCards_1, gameState.mistakesAllowed, gameState.combinationsGuessed)]), Cmd_none()];
                }
                case 2:
                    return [new State(0, [new Game(shuffle(gameState.cardsToGuess), gameState.mistakesAllowed, gameState.combinationsGuessed)]), Cmd_none()];
                default:
                    return [new State(0, [gameState]), Cmd_none()];
            }
        }
    }
}

export function view(state, dispatch) {
    let elems_9, elems_8;
    const card = (c) => {
        let elems;
        let cardClasses;
        const defaultClass = singleton_1("card");
        const addSelected = c.selected ? append(singleton_1("selected-card"), defaultClass) : defaultClass;
        const addGuessed = c.guessed ? append(singleton_1("guessed-card"), addSelected) : addSelected;
        const addShake = c.shake ? append(singleton_1("shake-card"), addGuessed) : addGuessed;
        cardClasses = (c.disabled ? ofArray(["card", "disabled"]) : addShake);
        return createElement("div", createObj(ofArray([["className", join(" ", cardClasses)], (elems = [createElement("img", {
            src: `/img/${c.image}`,
            alt: "Card",
        })], ["children", reactApi.Children.toArray(Array.from(elems))]), ["onClick", (_arg) => {
            dispatch(new Msg(0, [c]));
        }]])));
    };
    const cards = () => {
        let game_1, game_2, game_3, game_4, game_5, game;
        const result = map_1(card, (state.tag === 1) ? ((game_1 = state.fields[0], game_1.cardsToGuess)) : ((state.tag === 2) ? ((game_2 = state.fields[0], game_2.cardsToGuess)) : ((state.tag === 3) ? ((game_3 = state.fields[0], game_3.cardsToGuess)) : ((state.tag === 4) ? ((game_4 = state.fields[0], game_4.cardsToGuess)) : ((state.tag === 5) ? ((game_5 = state.fields[0], game_5.cardsToGuess)) : ((game = state.fields[0], game.cardsToGuess)))))));
        return result;
    };
    const description = (comb) => {
        const _description = (text) => {
            let elems_1;
            return createElement("div", createObj(ofArray([["className", "title"], (elems_1 = [createElement("div", {
                children: text[0],
            }), createElement("div", {
                className: "subtitle",
                children: text[1],
            })], ["children", reactApi.Children.toArray(Array.from(elems_1))])])));
        };
        const combinationToDescription = (comb_1) => {
            switch (comb_1.tag) {
                case 2:
                    return ["Sports", " Football, Basketball, Tennis, Boxing"];
                case 0:
                    return ["Board games", "Chess, Backgammon, Man don\'t get angry, Mahjong"];
                case 3:
                    return ["Car Manufacturers", "BMW, Lamborghini, Koenigsegg, Porsche"];
                default:
                    return ["First person shooters", " CS2, COD BO6, Wolfenstein, Battlefield V"];
            }
        };
        return _description(combinationToDescription(comb));
    };
    const descriptions = () => {
        let game_7, game_8, game_9, game_10, game_11, game_6;
        const result_1 = map_1(description, (state.tag === 1) ? ((game_7 = state.fields[0], game_7.combinationsGuessed)) : ((state.tag === 2) ? ((game_8 = state.fields[0], game_8.combinationsGuessed)) : ((state.tag === 3) ? ((game_9 = state.fields[0], game_9.combinationsGuessed)) : ((state.tag === 4) ? ((game_10 = state.fields[0], game_10.combinationsGuessed)) : ((state.tag === 5) ? ((game_11 = state.fields[0], game_11.combinationsGuessed)) : ((game_6 = state.fields[0], game_6.combinationsGuessed)))))));
        return result_1;
    };
    const grid = () => {
        let elems_2;
        return createElement("div", createObj(ofArray([["className", "grid"], (elems_2 = append(descriptions(), cards()), ["children", reactApi.Children.toArray(Array.from(elems_2))])])));
    };
    const button = (text_1, msg, dispatch_1) => createElement("button", {
        className: "button",
        children: text_1,
        onClick: (_arg_1) => {
            dispatch_1(msg);
        },
    });
    let mistakesCount;
    switch (state.tag) {
        case 1: {
            const gameState_1 = state.fields[0];
            mistakesCount = gameState_1.mistakesAllowed;
            break;
        }
        case 2: {
            const gameState_2 = state.fields[0];
            mistakesCount = gameState_2.mistakesAllowed;
            break;
        }
        case 3: {
            const gameState_3 = state.fields[0];
            mistakesCount = gameState_3.mistakesAllowed;
            break;
        }
        case 4: {
            const gameState_4 = state.fields[0];
            mistakesCount = gameState_4.mistakesAllowed;
            break;
        }
        case 5: {
            const gameState_5 = state.fields[0];
            mistakesCount = gameState_5.mistakesAllowed;
            break;
        }
        default: {
            const gameState = state.fields[0];
            mistakesCount = gameState.mistakesAllowed;
        }
    }
    const mistakesIndicator = (count) => {
        let elems_6;
        const mistakesIndicatorText = createElement("div", {
            className: "mistakes-indicator",
            children: "Mistakes allowed",
        });
        const dot = () => createElement("div", {
            className: "dot",
        });
        const dots = () => {
            let elems_4;
            return createElement("div", createObj(ofArray([["className", "dots"], (elems_4 = initialize(count, (_arg_2) => dot()), ["children", reactApi.Children.toArray(Array.from(elems_4))])])));
        };
        return createElement("div", createObj(singleton_1((elems_6 = [mistakesIndicatorText, dots()], ["children", reactApi.Children.toArray(Array.from(elems_6))]))));
    };
    const gameWon = () => {
        let elems_7;
        let _gameWon;
        if (state.tag === 5) {
            const gameState_6 = state.fields[0];
            _gameWon = true;
        }
        else {
            _gameWon = false;
        }
        if (_gameWon) {
            return createElement("div", createObj(ofArray([["className", "game-won"], (elems_7 = [createElement("p", {
                children: "Game won!",
            })], ["children", reactApi.Children.toArray(Array.from(elems_7))])])));
        }
        else {
            return createElement("div", {
                children: "",
            });
        }
    };
    const gameOver = () => {
        let _gameOver;
        if (state.tag === 4) {
            const gameState_7 = state.fields[0];
            _gameOver = true;
        }
        else {
            _gameOver = false;
        }
        if (_gameOver) {
            return createElement("div", {
                className: "game-over",
                children: "Game over!",
            });
        }
        else {
            return createElement("div", {
                children: "",
            });
        }
    };
    const oneAway = () => {
        let _oneAway;
        if (state.tag === 3) {
            const gameState_8 = state.fields[0];
            _oneAway = true;
        }
        else {
            _oneAway = false;
        }
        if (_oneAway) {
            return createElement("div", {
                className: "one-away",
                children: "One away!",
            });
        }
        else {
            return createElement("div", {
                className: "one-away-placeholder",
                children: "One Away!",
            });
        }
    };
    return createElement("div", createObj(ofArray([["className", "container"], (elems_9 = [gameWon(), gameOver(), oneAway(), grid(), createElement("div", createObj(ofArray([["className", "button-container"], (elems_8 = [button("Shuffle", new Msg(2, []), dispatch), button("Deselect", new Msg(1, []), dispatch)], ["children", reactApi.Children.toArray(Array.from(elems_8))])]))), mistakesIndicator(mistakesCount)], ["children", reactApi.Children.toArray(Array.from(elems_9))])])));
}

ProgramModule_run(Program_withReactSynchronous("app", ProgramModule_mkProgram(init, update, view)));

