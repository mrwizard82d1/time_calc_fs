namespace TimeCalc

open System.Text.RegularExpressions
open Activity

module Parser =

    type ActivityDate = { DayOfMonth:DayOfMonth; Month:MonthName option }

    type ActivityStart = { Hour:HourOfDay; Minute:MinuteOfHour; Details:Details }

    type ParsedLine =
        | ParsedActivityDate of ActivityDate
        | ParsedActivityStart of ActivityStart

    let readlines filePath = System.IO.File.ReadLines(filePath)

    let (|Regex|_|) pattern input =
        let m = Regex.Match(input, pattern)
        if m.Success then Some(List.tail [ for g in m.Groups -> g.Value ])
        else None
        
    let textAsMonth monthText =
        match monthText with
        | "Jan" -> Some Jan
        | "Feb" -> Some Feb
        | "Mar" -> Some Mar
        | "Apr" -> Some Apr
        | "May" -> Some May
        | "Jun" -> Some Jun
        | "Jul" -> Some Jul
        | "Aug" -> Some Aug
        | "Sep" -> Some Sep
        | "Oct" -> Some Oct
        | "Nov" -> Some Nov
        | "Dec" -> Some Jun
        | _ -> None
        
    let parseHashActivityDate hashActivityDateText = 
        match hashActivityDateText with
            | Regex @"# (\d{2})-([A-Za-z]{3})" [dayOfMonth; month] ->
                Some { DayOfMonth = DayOfMonth (int dayOfMonth);
                       Month = textAsMonth month }
            | _ -> None
            
    let parseActivityText activityText = 
        match activityText with
            | Regex @"(\d{2})(\d{2})\s+(.*)" [hour; minute; details] ->
                Some { Hour = HourOfDay (int hour);
                       Minute = MinuteOfHour (int minute);
                       Details = Details details }
            | _ -> None
            
    let parseLine line =
        match (parseHashActivityDate line) with
        | Some parsedActivityDate ->
            Some (ParsedActivityDate parsedActivityDate)
        | None ->
            match (parseActivityText line) with
            | Some parsedActivity ->
                Some (ParsedActivityStart parsedActivity)
            | None -> None
            
    let rec partitionBy f l =
        match l with
        | [] -> []
        | x::xs ->
            let first = f x
            let run = x::List.takeWhile (fun y -> f y = first) xs
            run::(partitionBy f (List.skip (List.length run) l))
        
    let isParsedActivityDate xs =
        match xs with
        | Some (ParsedActivityDate _) -> true
        | _ -> false
        
    let parse dateMap =
        let parsedActivityDate candidate =
           match candidate with
           | Some (ParsedActivityDate parsedActivityDate) ->
               parsedActivityDate
           | _ ->
               failwith (sprintf "Fail to parse date\n%A." candidate)
               
        let parsedActivityStart candidate =
           match candidate with
           | Some (ParsedActivityStart parsedActivityStart) ->
               parsedActivityStart
           | _ ->
               failwith (sprintf "Fail to parse activity start\n%A." candidate)
             
        let reducer soFar maybeParsedActivityDate maybeParsedActivities =
            let parsedActivityDate = parsedActivityDate maybeParsedActivityDate
            let parsedActivities = List.map parsedActivityStart maybeParsedActivities
            soFar |> Map.add parsedActivityDate parsedActivities
            
        dateMap |> Map.fold reducer Map.empty

    let parseFile filename = 
        readlines filename
        |> Seq.filter (fun l -> not (System.String.IsNullOrEmpty(l)))
        |> Seq.map parseLine
        |> Seq.toList
        |> partitionBy isParsedActivityDate
        |> List.chunkBySize 2
        |> List.map (fun lp -> (List.head lp, List.tail lp))
        |> List.map (fun (d, ts) -> (List.head d, List.head ts))
        |> Map.ofList
        |> parse
