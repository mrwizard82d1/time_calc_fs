#load "Activity.fs"
#load "Parser.fs"

open System
open TimeCalc.Activity
open TimeCalc.Parser

let parsedActivities = [ { Hour = HourOfDay 1; Minute = MinuteOfHour 5; Details = Details "plan day"; }; 
                         { Hour = HourOfDay 1; Minute = MinuteOfHour 25; Details = Details "learn FParsec"; };
                         { Hour = HourOfDay 3; Minute = MinuteOfHour 30; Details = Details "out"; };
                         { Hour = HourOfDay 9; Minute = MinuteOfHour 10; Details = Details "email"; }; ]

let activityDate = { DayOfMonth=DayOfMonth 4; Month=Some Feb; }

let monthInt monthName =
    match monthName with
    | Some Jan -> 1
    | Some Feb -> 2
    | None ->
        failwith (sprintf "Unrecognized month %A" monthName)

let makeActivityTimeStamp date parsedActivity =
    let { DayOfMonth=(DayOfMonth dayOfMonth); Month=monthName } = date
    let { Hour=(HourOfDay hour); Minute=(MinuteOfHour minute); Details=details } = parsedActivity
    let timestamp = DateTime(System.DateTime.Now.Year, (monthInt monthName), dayOfMonth, hour, minute, 0)
    { Start=timestamp; Duration=TimeSpan.Zero; Details=details }
    
let distributeDate date parsedActivities =
    let projector = makeActivityTimeStamp date
    parsedActivities |> List.map projector
    
let activityStart {Start=start} =
    start

let changeActivityEnd endTime activity =
    let {Start=startTime} = activity
    { activity with Duration=(endTime - startTime) }
    
let unfixedActivities = (distributeDate activityDate parsedActivities)
let fixUpActivities unfixedActivities =
    let shortEndTimes =
        unfixedActivities
        |> List.map (fun {Start=start} -> start)
        |> List.skip 1
    let endTimes = List.append shortEndTimes (List.take 1 (List.rev shortEndTimes))
    endTimes
    |> List.zip (List.truncate (List.length endTimes) unfixedActivities)
    |> List.map (fun (activity, endTime) -> changeActivityEnd endTime activity)
    
printfn "%A" (fixUpActivities unfixedActivities)
