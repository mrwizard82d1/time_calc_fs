#load "Parser.fs"

open TimeCalc.Parser

let validParsedDate =
   Some (ParsedDate {DayOfMonth = DayOfMonth 4;
                     Month = Some Feb;})

let validParsedActivities =
   [Some (ParsedActivityStart {Hour = HourOfDay 1;
                                Minute = MinuteOfHour 5;
                                Details = Details "plan day";});
     Some (ParsedActivityStart {Hour = HourOfDay 1;
                                Minute = MinuteOfHour 25;
                                Details = Details "learn FParsec";});
     Some (ParsedActivityStart {Hour = HourOfDay 3;
                                Minute = MinuteOfHour 30;
                                Details = Details "out";})]
let validParsedActivity = List.head validParsedActivities
   
let parsedDate candidate =
   match candidate with
   | Some (ParsedDate parsedDate) ->
       parsedDate
   | _ ->
       failwith "Fail to parse date."

printfn "parsedDate %A = %A" validParsedDate (parsedDate validParsedDate)

try
    printfn "parsedDate %A" validParsedActivity
    parsedDate validParsedActivity |> ignore
    printfn "threw *no* exception."
with
| _ ->
    printfn "expectedly threw exception"

let parsedActivityStart candidate =
   match candidate with
   | Some (ParsedActivityStart parsedActivityStart) ->
       parsedActivityStart
   | _ ->
       failwith "Fail to parse activity start."
       
printfn "parsedActivityStart %A = %A" validParsedActivity (parsedActivityStart validParsedActivity)

try
    printfn "parsedActivityStart %A" validParsedDate
    parsedActivityStart validParsedDate |> ignore
    printfn "threw *no* exception."
with
| _ ->
    printfn "expectedly threw exception"
    
let parse dateMap =
    let parsedDate candidate =
       match candidate with
       | Some (ParsedDate parsedDate) ->
           parsedDate
       | _ ->
           failwith "Fail to parse date."
           
    let parsedActivityStart candidate =
       match candidate with
       | Some (ParsedActivityStart parsedActivityStart) ->
           parsedActivityStart
       | _ ->
           failwith "Fail to parse activity start."
         
    let reducer soFar maybeParsedDate maybeParsedActivities =
        let parsedDate = parsedDate maybeParsedDate
        let parsedActivities = List.map parsedActivityStart maybeParsedActivities
        soFar |> Map.add parsedDate parsedActivities
        
    dateMap |> Map.fold reducer Map.empty
    
Map.add validParsedDate validParsedActivities Map.empty
|> parse
|> printfn "parse valid map = %A" 
