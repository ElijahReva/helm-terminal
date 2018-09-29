namespace Helm.Terminal

open Microsoft.AspNetCore.Hosting
open Serilog
open Helm.Terminal.Server

[<AutoOpen>]
module EntryPoint =
    
    [<EntryPoint>]
    let main argv =
        Bootstrap.logger()
        try
            try
                Log.Information("WebHostStart");
                argv |> Bootstrap.buildWebHost |> fun x -> x.Run()
                Log.Information("WebHostStop");
                0
            with 
            | _ as ex ->
                Log.Fatal(ex, "Host terminated unexpectedly");                
                -1
        finally
            Log.CloseAndFlush();