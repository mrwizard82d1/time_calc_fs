namespace TimeCalc

open TimeCalc.Activity
open System

module Presenter =
    
    let printSummary (Details details) (duration:TimeSpan) =
        printfn "%12s  %.02f" details duration.TotalHours
        
    let separateSummaries summaries =
        summaries
        |> Map.iter printSummary
        printfn ""
