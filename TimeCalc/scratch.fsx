#load "Activity.fs"
#load "Parser.fs"
#load "ParserAdapter.fs"

open TimeCalc.Activity
open System

let sampleActivities = [
   [ {Start = DateTime(2019, 2, 4, 1, 5, 0); Duration = TimeSpan.FromMinutes(20.0); Details = Details "planning"; };
     {Start = DateTime(2019, 2, 4, 1, 25, 0); Duration = TimeSpan(0, 2, 5, 0); Details = Details "learn-f#"; };
     {Start = DateTime(2019, 2, 4, 3, 30, 0); Duration = TimeSpan.Zero; Details = Details "out"; }
   ];
   [ {Start = DateTime(2019, 2, 5, 0, 0, 0); Duration = TimeSpan(0, 1, 45, 0); Details = Details "design"; };
     {Start = DateTime(2019, 2, 5, 1, 45, 0); Duration = TimeSpan(0, 6, 10, 0); Details = Details "out"; };
     {Start = DateTime(2019, 2, 5, 7, 55, 0); Duration = TimeSpan.Zero; Details = Details "planning"; }
   ];
   [ {Start = DateTime(2019, 2, 6, 8, 55, 0); Duration = TimeSpan.FromMinutes(5.0); Details = Details "email"; };
     {Start = DateTime(2019, 2, 6, 9, 0, 0); Duration = TimeSpan.FromMinutes(15.0); Details = Details "review-pr"; };
     {Start = DateTime(2019, 2, 6, 9, 15, 0); Duration = TimeSpan.Zero; Details = Details "planning"; }
   ];
   [ {Start = DateTime(2019, 2, 7, 0, 0, 0); Duration = TimeSpan.FromMinutes(35.0); Details = Details "other"; };
     {Start = DateTime(2019, 2, 7, 0, 35, 0); Duration = TimeSpan.FromHours(8.0); Details = Details "out"; };
     {Start = DateTime(2019, 2, 7, 8, 35, 0); Duration = TimeSpan.Zero; Details = Details "email"; }
   ];
   [ {Start = DateTime(2019, 2, 8, 2, 25, 0); Duration = TimeSpan.FromMinutes(30.0); Details = Details "other"; };
     {Start = DateTime(2019, 2, 8, 2, 55, 0); Duration = TimeSpan.FromMinutes(45.0); Details = Details "other"; };
     {Start = DateTime(2019, 2, 8, 3, 40, 0); Duration = TimeSpan.Zero; Details = Details "out"; }
   ]
]

let activitySummary { Details=details; Duration=duration } =
  (duration, details)

let activitySummaries = List.map activitySummary (List.head sampleActivities)
printfn "%A" activitySummaries

let mergeSummary soFar (duration, details) =
  match Map.containsKey details soFar with
  | true ->
    Map.add details ((Map.find details soFar) + duration) soFar
  | false ->
    Map.add details duration soFar
    
let summarizeDay activitySummaries =
  activitySummaries |> List.fold mergeSummary Map.empty
  
printfn "%A" (summarizeDay activitySummaries)
printfn "%A" (summarizeDay (List.map activitySummary (List.item 4 sampleActivities)))

printfn "%A" (List.map (fun summaries -> summarizeDay (List.map activitySummary summaries)) sampleActivities) 

