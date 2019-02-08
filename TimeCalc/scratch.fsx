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
        
let parse line =
    match (parseHashDate line) with
    | Some parsedDate ->
        Some (ParsedDate parsedDate)
    | None ->
        match (parseActivityText line) with
        | Some parsedActivity ->
            Some (ParsedActivityStart parsedActivity)
        | None -> None
        
//let validDate = "# 05-Feb"
//printfn "Valid date %A" (parse validDate)
//let invalidDate = "# 5-Feb"
//printfn "Invalid date %A" (parse invalidDate)
//let validActivityStart = "2249 write time_calc_fs"
//printfn "Valid activity start %A" (parse validActivityStart)
//let invalidActivityStart = "249 write time_calc_fs"
//printfn "Invalid activity start %A" (parse invalidActivityStart)

let parsedLines = [
          ParsedDate {DayOfMonth=DayOfMonth 5; Month=Some Feb};
          ParsedActivityStart{Hour=HourOfDay 22; Minute= MinuteOfHour 49; Details=Details "foo"};
          ParsedActivityStart{Hour=HourOfDay 22; Minute= MinuteOfHour 53; Details=Details "bar"};
          ParsedDate {DayOfMonth=DayOfMonth 5; Month=Some Feb};
          ParsedActivityStart{Hour=HourOfDay 22; Minute= MinuteOfHour 55; Details=Details "baz"};
          ]

let rec partitionBy f l =
    match l with
    | [] -> []
    | x::xs ->
        let first = f x
        let run = x::List.takeWhile (fun y -> f y = first) xs
        run::(partitionBy f (List.skip (List.length run) l))
    
//printfn "partitionBy id [] = %A" (partitionBy id [])
//printfn "partitionBy id [ 1 ] = %A" (partitionBy id [ 1 ])
//let isOdd x = (x % 2 = 0)
//printfn "partitionBy isOdd [] = %A" (partitionBy isOdd [])
//printfn "partitionBy isOdd [ 1 ] = %A" (partitionBy isOdd [ 1 ])
//printfn "partitionBy isOdd [ 1 2 ] = %A" (partitionBy isOdd [ 1; 2 ])
//let isPrime x =
//    List.contains x [2; 3; 5; 7; 11; 13; 17]
//printfn "partitionBy isPrime [2 .. 17] = %A" (partitionBy isPrime [2..17])

let isDate xs =
    match xs with
    | ParsedDate _ -> true
    | _ -> false
printfn "partitioned parsed lines = %A" (partitionBy isDate parsedLines)
