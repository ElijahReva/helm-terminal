namespace Helm.Terminal.Server

open System.Diagnostics

type ServerError =    
    {
        Message: string
        Stack: string
    }
    
type WebError = 
    | Client of string
    | Server of ServerError


module Result = 
    let inline tryUsing f x =
        try
            x |> f |> Ok
        with 
        | _ as ex -> 
            let ex = ex.Demystify()
            { Message = ex.Message; Stack = ex.StackTrace} |> Server |> Error      
        
    let inline fromStringError x = Result.mapError Client x    


[<AutoOpen>]
module DOM =

    open System
    open System.Diagnostics
    open FSharp.Data    
    open Fake.Core
    open FSharp.Data.JsonExtensions
    open Serilog
      
    type HelmChart = JsonProvider<"""
    {
        "Name": "dev-ui",
        "Revision": 3,
        "Updated": "Fri Sep 28 13:46:48 2018",
        "Status": "DEPLOYED",
        "Chart": "sym-verse-second-0.1.0",
        "AppVersion": "1.0",
        "Namespace": "dev-ui"
    }""">
    
    [<CLIMutable>]
    type KubeContext = 
        {
            name: string
            address: string        
            defaultNamespace: string        
        }        
        
    [<CLIMutable>]
    type KubeNamespace = 
        {
            name: string        
        }
                
    [<CLIMutable>]
    type YamlRequest = 
        {
            ``namespace``: string        
            context: string        
            chart: string        
            fromRelease: string        
        }
     
    [<CLIMutable>]
    type Chart = 
        {
            Name: string
            Chart: string
            Revision: int
            AppVersion: string                        
            Status: string                        
            Updated: string                        
        }   
    
    let cmdTimeout = TimeSpan.FromSeconds(30.)
    
    let inline private setInfo tool dir args (info:ProcStartInfo) =
        { info with
            FileName = tool
            WorkingDirectory = dir
            Arguments = args }
    
    let getTime() = Stopwatch.GetTimestamp()    
    let getElapser (start: int64) (stop: int64) = int64((stop - start) * int64(1000)) / Stopwatch.Frequency;
    
    let private runCmd tool dir args =
        
        Log.Information("RunningCmd {Tool} {Args}", tool, args) 
        let start = getTime()
        let processResult = Process.execWithResult (setInfo tool dir args) cmdTimeout
        let elapsedMs = getElapser start (getTime())
        if not processResult.OK then failwith <| String.toLines processResult.Errors else
        
        Log.Information("RunningCmdFinished {Tool} {Args} {Elapsed}", tool, args, elapsedMs)
        processResult.Messages 
        |> String.toLines    
            
    let helm = runCmd "helm"
    let kube = runCmd "kubectl"
        
    let getContexts() =
        let getCtx json : KubeContext list =
            let contexts = 
                [
                    for c in json?contexts ->
                    { 
                        name = c?name.AsString()
                        address = c?context?cluster.AsString() 
                        defaultNamespace = c?context?``namespace``.AsString() 
                    } 
                ]
            contexts
                 
    
        "config view -o json" 
        |> kube "."  
        |> Result.tryUsing JsonValue.Parse
        |> Result.map getCtx
            
    let getNamespaces context =
        let toNamespaces json : KubeNamespace list =
            [ for c in json?items -> c]
            |> List.filter (fun ns -> ns?status?phase.AsString() = "Active" )
            |> List.map (fun ns -> { name = ns?metadata?name.AsString() })
            
        context    
        |> sprintf "get ns -o json --context %s"         
        |> kube "." 
        |> Result.tryUsing JsonValue.Parse
        |> Result.map toNamespaces
        
            
    let getCharts context ns =
        let toCharts json =
            [for r in json?Releases -> r]
            |> List.map 
                (fun (x: JsonValue) -> 
                    {
                        Name = x?Name.AsString()
                        Chart = x?Chart.AsString()
                        Revision = x?Revision.AsInteger()
                        AppVersion = x?AppVersion.AsString()                     
                        Status = x?Status.AsString()             
                        Updated = x?Updated.AsString()
                    })
    
        let output = 
            sprintf "ls --output json --namespace %s --kube-context %s " ns context |> helm "."
        
        
        if String.isNullOrEmpty output then 
            List.empty |> Ok
        else 
            output 
            |> Result.tryUsing JsonValue.Parse 
            |> Result.map toCharts
  
    [<CLIMutable>]
    type KubeSettings = 
        { 
            Address: string
            User: string
            Password: string   
        }
    
    [<CLIMutable>]
    type AppConfiguration = 
        {
            Kube : KubeSettings
        }        
    
    let memoize f =
        let cache = ref Map.empty    
        fun x ->    
            match (!cache).TryFind(x) with    
            | Some res -> res    
            | None ->    
                 let res = f x    
                 cache := (!cache).Add(x,res)    
                 res