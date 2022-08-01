open System

(*
let lim: int = 9999 
printf $"Enter a int between 1 - %i{lim}: "
let input: string = Console.ReadLine()

match Int32.TryParse input with // match against the bool return and the out int
| false, _ -> printfn $"%s{input} is not an int." // discard failed parse
| true, num ->
    match num >= 1 && num <= lim with // match against validation logic
    | false -> printfn $"%i{num} is out of range."
    | true ->
        [ 1..num ] // create list of 1 - num
        |> List.map (fun n -> (n, n % 3, n % 5)) //map to tuple of the num, and its remainders for 3 & 5
        |> List.map (function // anon func
            | _, 0, 0 -> "FizzBuzz"
            | _, 0, _ -> "Fizz"
            | _, _, 0 -> "Buzz"
            | n, _, _ -> string n) // map to strings based on what should output
        |> String.concat "\n" // collect strings into 1
        |> printfn "Fizzbuzz'ing:\n%s"
*)
