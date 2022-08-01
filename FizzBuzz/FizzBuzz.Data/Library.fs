namespace FizzBuzz.Data

open System

module Parser =
    /// <summary>Parse a user input</summary>
    /// <param name="input">Input string from user</param>
    /// <returns>Int option with either the parsed int or None</returns>
    let TryParse (input: string) : int option =
        Int32.TryParse input
        |> function // Option = union type for error handling
            | false, _ -> None // no return
            | true, num -> Some num // Some = union type for option

module Validator =
    type ValidNum = private ValidNum of int // discriminated union for a validated num with a private ctor

    module ValidNumber =
        let value (ValidNum vNum) : int = vNum // module makes it static and exposes the ctor

    /// <summary>Validate if a number is within acceptable bounds</summary>
    /// <param name="num">int to validate</param>
    /// <param name="upperBound">int for the upper bound to check against</param>
    /// <returns>Int option with either the validated ValidNum object or None</returns>
    let ValidateNum (num: int) : ValidNum option =
        match num >= 1 && num <= 4000 with // match against validation logic
        | false -> None
        | true -> Some <| ValidNum num


module FizzBuzz =
    open Validator

    /// <summary>Create the fizzbuzz string for a number</summary>
    /// <param name="num">int to FizzBuzz to</param>
    /// <returns>string with the FizzBuzz output for num param</returns>
    let GetFizzBuzz (num: ValidNum) =
        [ 1 .. ValidNumber.value num ]
        |> List.map (fun n -> (n, n % 3, n % 5))
        |> List.map (function
            | _, 0, 0 -> "FizzBuzz"
            | _, 0, _ -> "Fizz"
            | _, _, 0 -> "Buzz"
            | n, _, _ -> string n)
        |> String.concat "\n"
        |> printfn "Fizzbuzz'ing:\n%s"

[<RequireQualifiedAccess>]
module Result =
    let FromOption errorValue =
        function
        | Some x -> Ok x
        | None -> Error errorValue

module Service =
    open Validator

    type ParsedNum = string -> int option
    type ParserError = NotANumber of string

    type ValidatedNum = int -> ValidNum option
    type ValidatorError = InvalidNumber of int

    type FizzBuzzStr = ValidNum -> string

    type DomainError =
        | ParserError of ParserError
        | ValidatorError of ValidatorError

    type Run = string -> Result<string, DomainError>

    /// <summary>Run FizzBuzz app</summary>
    /// <param></param>
    /// <param></param>
    /// <param></param>
    /// <returns></returns>
    let Execute (parseNum: ParsedNum) (validateNum: ValidatedNum) (output: FizzBuzzStr) : Run =
        // internal functions to make the main logic fluent for Result
        let parseNum (input: string) =
            input
            |> parseNum
            |> Result.FromOption(NotANumber input)
            |> Result.mapError ParserError

        let validateNum (num: int) =
            num
            |> validateNum
            |> Result.FromOption(InvalidNumber num)
            |> Result.mapError ValidatorError

        //run logic
        fun input ->
            input
            |> parseNum
            |> Result.bind validateNum
            |> Result.map output
