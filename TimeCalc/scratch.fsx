#load "Activity.fs"
#load "Parser.fs"
#load "ParserAdapter.fs"

open TimeCalc

let dailySummaries =
    Parser.parseFile "./time.999.md"
    |> ParserAdapter.toActivities
    |> Activity.summarize

printfn "%A" dailySummaries

open TimeCalc.Activity
open System

let printSummary (Details details) (duration:TimeSpan) =
    printfn "%12s  %.02f" details duration.TotalHours
    
dailySummaries
|> List.head
|> Map.iter printSummary

let separateSummaries summaries =
    summaries
    |> Map.iter printSummary
    printfn ""
    
dailySummaries
|> List.iter separateSummaries

