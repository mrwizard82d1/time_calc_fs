open System.Text.RegularExpressions

type DayOfMonth = DayOfMonth of int

type Month =
    | Jan
    | Feb
    
type Date = { DayOfMonth:DayOfMonth; Month:Month option }

type HourOfDay = HourOfDay of int

type MinuteOfHour = MinuteOfHour of int
type Details = Details of string

type ActivityStart = { Hour:HourOfDay; Minute:MinuteOfHour; Details:Details }

type ParsedLine =
    | ParsedDate of Date
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
    | _ -> None
    
let parseHashDate hashDateText = 
    match hashDateText with
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
    match (parseHashDate line) with
    | Some parsedDate ->
        Some (ParsedDate parsedDate)
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
    
let isParsedDate xs =
    match xs with
    | Some (ParsedDate _) -> true
    | _ -> false

let parseFile filename = 
    readlines filename
    |> Seq.filter (fun l -> not (System.String.IsNullOrEmpty(l)))
    |> Seq.map parseLine
    |> Seq.toList
    |> partitionBy isParsedDate
    |> List.pairwise
    |> Map.ofList
    
[<EntryPoint>]
let main argv =
    parseFile argv.[0]
    |> printfn "%A"
    0 // return an integer exit code

