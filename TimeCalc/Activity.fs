namespace TimeCalc

open System
    
module Activity =
    
    type Year = Year of int
    
    type DayOfMonth = DayOfMonth of int

    type MonthName =
        | Jan
        | Feb
        
    type Month = Month of int
        
    type HourOfDay = HourOfDay of int

    type MinuteOfHour = MinuteOfHour of int
    
    type SecondOfMinute = SecondOfMinute of int
    
    type Details = Details of string
    
    type Activity = { Start:DateTime; Duration:TimeSpan; Details:Details }
    
    let activitySummary { Details=details; Duration=duration } =
      (duration, details)

    let mergeSummary soFar (duration, details) =
      match Map.containsKey details soFar with
      | true ->
        Map.add details ((Map.find details soFar) + duration) soFar
      | false ->
        Map.add details duration soFar
        
    let summarizeDay activitySummaries =
      activitySummaries |> List.fold mergeSummary Map.empty
      
    let summarize activities =
      List.map (fun summaries -> summarizeDay (List.map activitySummary summaries)) activities 
        
