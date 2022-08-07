﻿open System

let rng: Random = Random()
let rollDice (diceCt: int) : int = rng.Next(diceCt, (diceCt * 6) + 1)


printfn "===DICE SIM==="
printfn "1 | Stats"
printfn "2 | Lucky 7's"
printf "Choose module to load: "

let input: string = Console.ReadLine()

match Int32.TryParse input with
| false, _ -> failwith $"%s{input} is not a valid selection..."
| true, num ->
    match num with
    | 1 -> // Stats
        printfn "*** Dice Stats ***"
        printf "Please number of dice to roll: "
        let diceInput: string = Console.ReadLine()

        printf "Please number of rolls: "
        let rollsInput: string = Console.ReadLine()

        match Int32.TryParse diceInput with
        | false, _ -> failwith "Invalid number of dice to roll..."
        | true, numDice ->
            // validate num of rolls
            match Int32.TryParse rollsInput with
            | false, _ -> failwith "Could not parse number of rolls..."
            | true, numRolls ->
                match numRolls > 0 with
                | false -> failwith "Invalid number of rolls..."
                | true ->
                    // sequence expressions - very much like list comprehensions in python
                    let rolls: int list =
                        [ for _ in 1..numRolls -> rollDice numDice ]

                    let rollsIdx: (int * int) list =
                        rolls |> List.indexed

                    match List.isEmpty rolls with
                    | true -> printfn "List empty, average = 0"
                    | false ->
                        // `List.average` doesn't work for int as it uses float, not integer, division internally
                        // use `List.averageBy` to allow for a delegate to cast to float
                        // use the inner expression `float` which is the same as `(fun r -> float r)`
                        rolls
                        |> List.averageBy float
                        |> printfn "Average of rolls = %.4f"

                        // mode
                        let modeRoll: int * int =
                            rolls
                            |> Seq.countBy id // groups items to tuple (roll, count)
                            |> Seq.sortByDescending id
                            |> Seq.truncate 1 // take first (will be highest count)
                            |> Seq.item 0 // unbox item from sequence

                        printfn $"Mode is %i{fst modeRoll}, rolled %i{snd modeRoll} times"

                        // min
                        let worst: int * int =
                            rollsIdx |> List.minBy snd

                        printfn $"Worst roll = %i{snd worst} on roll %i{fst worst}"

                        // median
                        let sortArr: int [] =
                            rolls |> Seq.toArray |> Array.sort

                        printfn $"Median = %i{sortArr[sortArr.Length / 2]}"
                        // printfn $"Sorted Rolls: %A{sortArr}"

                        // max
                        let best: int * int =
                            rollsIdx |> List.maxBy snd

                        printfn $"Best roll = %i{snd best} on roll %i{fst best}"

                        printfn $"Your Rolls: %A{rolls}"
    | 2 -> // Lucky 7's
        let maxBuyIn: decimal = 100m
        
        printfn "*** Lucky 7's Sim ***"
        printf $"Enter buy in amount (up to %.2f{maxBuyIn}): $"
        let buyInStr: string = Console.ReadLine()
        
        match Decimal.TryParse buyInStr with
        | false, _ -> failwith $"%s{buyInStr} could not be parsed to a monetary value..."
        | true, buyIn ->
            match buyIn > 0m && buyIn <= 100m with
            | false -> failwith $"%.2f{buyIn} is not a valid amount..."
            | true ->
                printf "Enter bet amount - wins are doubled: $"
                let betStr: string = Console.ReadLine()
                
                match Decimal.TryParse betStr with
                | false, _ -> failwith $"%s{betStr} could not be parsed to a monetary value..."
                | true, bet ->
                    match bet > 0m && bet < buyIn with
                    | false -> failwith "Invalid bet amount..."
                    | true ->
                        let mutable pot: decimal = buyIn
                        let mutable round: int = 0
                        while pot > 0m do
                            round <- round + 1
                            
                            let roll: int = rollDice 2
                            match roll with
                            | 7 -> pot <- pot + (bet * 2m)
                            | _ -> pot <- pot - bet
                            //printfn $"Rolled %i{roll} | $%.2f{pot}"
                        
                        printfn $"You went broke after %i{round} rounds"
    | _ -> failwith "Not a valid module to run..."
    
