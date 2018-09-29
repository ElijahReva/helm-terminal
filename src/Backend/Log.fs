namespace Helm.Terminal.Server

open System

[<AutoOpen>]
module Log =
    
    open Serilog
    
    let a = 9
    
    let logValueInnerUsing (logger: ILogger) (str: string) (selector: 'a -> 'b) (value: 'a) = 
        logger.Information(str + " {@Response}", (value |> selector))
        value    
    
    let logValueUsing (l: ILogger) (str: string) (value: 'a) = logValueInnerUsing l str (fun x -> x) value
    
    let logValueInner a selector b = logValueInnerUsing (Log.Logger) a selector b
        
    let logValue a b = logValueInner a (fun x -> x) b 