open TimeCalc
    
[<EntryPoint>]
let main argv =
    Parser.parseFile argv.[0]
    |> ParserAdapter.asnToActivities
    |> printfn "%A"
    0 // return an integer exit code

