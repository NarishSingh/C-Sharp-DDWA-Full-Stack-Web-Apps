open System

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
                    let rng: Random = Random()
                    let rollDice (diceCt: int) : int = rng.Next(diceCt, (diceCt * 6) + 1)

                    // sequence expressions - very much like list comprehensions in python
                    let rolls: int list =
                        [ for _ in 1..numRolls -> rollDice numDice ] // todo try this with List.indexed and unwrap tuple

                    match List.isEmpty rolls with
                    | true -> printfn "List empty, average = 0"
                    | false ->
                        // note: `List.average` doesn't work for int,
                        // use `List.averageBy` to allow for a delegate to cast to float
                        // use the inner expression `float` which is the same as `(fun r -> float r)`
                        rolls
                        |> List.averageBy float
                        |> printfn "Average of rolls = %f"

                        // mode
                        let modeRoll: int * int =
                            rolls
                            |> Seq.countBy id // group items to tuple (roll, count)
                            |> Seq.sortByDescending id // sort high to low
                            |> Seq.truncate 1 // get the first (highest count)
                            |> Seq.item 0 // get the item from sequence

                        printfn $"Mode is %i{fst modeRoll}, rolled %i{snd modeRoll} times"

                        rolls |> List.min |> printfn "Lowest roll = %i" // min

                        // median
                        let sortArr: int [] =
                            rolls |> Seq.toArray |> Array.sort
                        printfn $"Median = %i{sortArr[sortArr.Length / 2]}"
                        // printfn $"Sorted Rolls: %A{sortArr}"

                        rolls |> List.max |> printfn "Highest roll = %i" // max

                        printfn $"Your Rolls: %A{rolls}"
    | 2 -> // Lucky 7's
        ()
    | _ -> failwith "Not a valid module to run..."
