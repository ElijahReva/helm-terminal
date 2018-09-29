namespace Helm.Terminal.Server

module App =
    open System
    open Giraffe
    
    let private getHeroesHandler : HttpHandler =
        fun next ctx ->        
             "Data" |> ctx.WriteJsonAsync
              
              
    let private currentSchema : HttpHandler =
        fun next ctx ->        
             "Schema current not implemented"
             |> ctx.WriteJsonAsync
               
    
    let api str f = subRouteCi str (choose f)
    
    let webApp (config: AppConfiguration): HttpHandler =
        choose [
            GET >=>
                choose [
                    api "/api" [                    
                        route "/current" >=> currentSchema                        
                        route "/getContexts" >=> getHeroesHandler                     
                    ]
                ]
            ]