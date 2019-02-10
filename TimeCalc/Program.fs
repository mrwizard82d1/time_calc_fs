﻿open TimeCalc

[<EntryPoint>]
let main argv =
    Parser.parseFile argv.[0]
    |> ParserAdapter.toActivities
    |> Activity.summarize
    |> printfn "%A"
    0 // return an integer exit code

