namespace TimeCalc

open TimeCalc.Activity
open TimeCalc.Parser
open System

module ParserAdapter = 

    let monthInt monthName =
        match monthName with
        | Some Jan -> 1
        | Some Feb -> 2
        | Some Mar -> 3
        | Some Apr -> 4
        | Some May -> 5
        | Some Jun -> 6
        | Some Jul -> 7
        | Some Aug -> 8
        | Some Sep -> 9
        | Some Oct -> 10
        | Some Nov -> 11
        | Some Dec -> 12
        | None ->
            failwith (sprintf "Unrecognized month %A" monthName)

    let makeActivityTimeStamp date activity =
        let { DayOfMonth=(DayOfMonth dayOfMonth); Month=monthName } = date
        let { Hour=(HourOfDay hour); Minute=(MinuteOfHour minute); Details=details } = activity
        let timestamp =
            match hour with
            | 24 ->
                let unadjustedDate = DateTime(System.DateTime.Now.Year, (monthInt monthName), dayOfMonth)
                let adjustedDate = unadjustedDate + TimeSpan.FromDays(1.0)
                DateTime(adjustedDate.Year, adjustedDate.Month, adjustedDate.Day, hour - 24,  minute, 0)
            | _ -> DateTime(System.DateTime.Now.Year, (monthInt monthName), dayOfMonth, hour, minute, 0)
        { Start=timestamp; Duration=TimeSpan.Zero; Details=details }
        
    let distributeDate (date, activities) =
        let projector = makeActivityTimeStamp date
        activities |> List.map projector
        
    let activityStart {Start=start} =
        start

    let changeActivityEnd endTime activity =
        let {Start=startTime} = activity
        { activity with Duration=(endTime - startTime) }
        
    let fixUpActivities unfixedActivities =
        let shortEndTimes =
            unfixedActivities
            |> List.map (fun {Start=start} -> start)
            |> List.skip 1
        let endTimes = List.append shortEndTimes (List.take 1 (List.rev shortEndTimes))
        endTimes
        |> List.zip (List.truncate (List.length endTimes) unfixedActivities)
        |> List.map (fun (activity, endTime) -> changeActivityEnd endTime activity)

    let toActivities asn =
        asn
        |> Map.toList
        |> List.map distributeDate
        |> List.map fixUpActivities
