namespace Helm.Terminal.Server

[<AutoOpen>]
module DOM =

    open System
    open FSharp.Data    
    open Fake.Core
    open FSharp.Data.JsonExtensions
  
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
    
    let cmdTimeout = TimeSpan.FromSeconds(30.)
    
    let inline private setInfo toolPath workDir command (info:ProcStartInfo) =
        { info with
            FileName = toolPath
            WorkingDirectory = workDir
            Arguments = command }
    
    let private runCmd tool dir args = 
        let processResult = Process.execWithResult (setInfo tool dir args) cmdTimeout
        if not processResult.OK then failwith <| String.toLines processResult.Errors else
        processResult.Messages 
        |> String.toLines    
            
    let helm = runCmd "helm"
    let kube = runCmd "kbuectl"
        
    let getContexts() = kube "config view -o json" "."
        
            
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
    type KubeContext = 
        {
            name: string
            address: string        
        }
        
    [<CLIMutable>]
    type KubeNamespace = 
        {
            name: string        
        }