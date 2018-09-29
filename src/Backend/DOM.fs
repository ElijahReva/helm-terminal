namespace Helm.Terminal.Server

[<AutoOpen>]
module DOM =

    open System
  
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
    