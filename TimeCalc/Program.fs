open TimeCalc

[<EntryPoint>]
let main argv =
    Parser.parseFile argv.[0]
    |> ParserAdapter.toActivities
    |> Activity.summarize
    |> List.iter Presenter.separateSummaries
    0 // return an integer exit code

