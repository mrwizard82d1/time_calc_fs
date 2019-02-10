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
    
