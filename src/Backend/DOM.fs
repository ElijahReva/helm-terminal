namespace Helm.Terminal.Server

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
        
    let getContexts() : KubeContext list option =
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
        |> JsonValue.TryParse
        |> Option.map getCtx
            
    let getNamespaces() = kube "." "get ns -o json"
        
            
    let getCharts ns =
        if ns |> Seq.isEmpty then
            ()
        else 
            //[ for p in ns?results -> p?name.AsString() ]
            ()
  
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
                 
        
    [<CLIMutable>]
    type KubeNamespace = 
        {
            name: string        
        }