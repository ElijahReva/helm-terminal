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
    
    let inline private setInfo toolPath repositoryDir command (info:ProcStartInfo) =
        { info with
            FileName = toolPath
            WorkingDirectory = repositoryDir
            Arguments = command }
            
            
    let helm dir str = Process.execWithResult (setInfo "helm" dir str) cmdTimeout 
    let kube dir str = Process.execWithResult (setInfo "kubectl" dir str) cmdTimeout 
    
    let getContexts() =
        let processResult = kube "." "config view -o json"
        
        processResult.Messages 
        |> String.toLines
            
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