open System

printfn "===DICE SIM==="
printfn "1 | Stats"
printfn "2 | Lucky 7's"

printf "Choose module to load: "
let input: string = Console.ReadLine()
match Int32.TryParse input with
| false, _ -> printfn $"%s{input} is not a valid selection..."
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
            | false, _ -> failwith "Invalid number of rolls..."
            | true, numRolls ->
                match numRolls > 0 with
                | false -> failwith "Invalid number of rolls..."
                | true ->  
                    let rng: Random = Random()
                    let rollDice (diceCt: int) = rng.Next(diceCt, (diceCt * 6) + 1)
                    
                    let rolls: int list = [for i in 1 .. numRolls -> rollDice numDice] // list comprehension
                    match rolls with
                    | _  -> rolls
                        |> List.average
                        |> printfn $"Average of rolls = %s"
                    | [] -> 0
    | 2 -> // Lucky 7's
        ()
    | _ -> printfn "Not a valid module to run..."
