namespace Helm.Terminal.Server

open Microsoft.AspNetCore.SignalR

type ManagerHub() =
        inherit Hub()       
        
        override this.OnConnectedAsync() = 
            this.Groups.AddToGroupAsync(this.Context.ConnectionId, "SignalR Users")
            |> Async.AwaitTask 
            |> Async.RunSynchronously
            Serilog.Log.Logger.Information("UserConnected {@Test}", this.Context.UserIdentifier)
            base.OnConnectedAsync()
        
        override this.OnDisconnectedAsync(ex) = 
           this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, "SignalR Users")
           |> Async.AwaitTask 
           |> Async.RunSynchronously
           Serilog.Log.Logger.Information("UserDisconnected {@Test}", this.Context.UserIdentifier)
           base.OnDisconnectedAsync(ex)



module App =
    open FSharp.Data
    open System.Threading.Tasks
    open System.Threading.Tasks
    open System
    open Giraffe
    open Giraffe.HttpStatusCodeHandlers.RequestErrors
    open Microsoft.AspNetCore.Http
    open Newtonsoft.Json
    
    module Result =    
        let private badRequest (ctx: HttpContext) error =
            ctx.SetStatusCode 400
            ctx.WriteJsonAsync error
            
        let private serverError (ctx: HttpContext) error =
            ctx.SetStatusCode 500
            ctx.WriteJsonAsync error
                                     
        let writeJson (ctx: HttpContext) result =        
            match result with 
            | Ok result -> result |> ctx.WriteJsonAsync
            | Error err -> 
                match err with 
                | Client err -> badRequest ctx err
                | Server err -> serverError ctx err
                                                     
        let inline getQuery (ctx: HttpContext) = ctx.GetQueryStringValue >> Result.fromStringError 


    let private getKubeContexts : HttpHandler =
        fun next ctx ->              
             DOM.getContexts () |> Result.writeJson ctx
             
    let private getKubeNamespaces : HttpHandler =
        fun next ctx ->
            "context"
            |> Result.getQuery ctx             
            |> Result.map getNamespaces 
            |> Result.writeJson ctx
            
              
    let private getHelmCharts : HttpHandler =
        fun next ctx ->
            "context"
            |> Result.getQuery ctx
            |> Result.bind (fun x -> "namespace" |> Result.getQuery ctx |> Result.map (fun y -> x, y))
            |> Result.map (fun (c, n) -> getCharts c n)  
            |> Result.writeJson ctx            
              
    let private getHelmYaml : HttpHandler =
        fun next ctx ->
            ctx.ReadBodyFromRequestAsync() 
            |> Async.AwaitTask
            |> Async.RunSynchronously
            |> Result.tryUsing JsonValue.Parse
            |> Result.bind (fun json -> json |> Result.tryUsing (fun j -> j?))  
            |> Result.writeJson ctx
                       
    let private runAction : HttpHandler =
        fun next ctx ->        
             kube "." "get ns -o json" |> ctx.WriteJsonAsync
              
              
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
                        route "/charts" >=> getHelmCharts                           
                        route "/current" >=> currentSchema                        
                        route "/contexts" >=> getKubeContexts                     
                        route "/namespaces" >=> getKubeNamespaces          
                        route "/yaml" >=> getHelmYaml          
                    ]
                ]
            POST >=> 
                choose [
                    route "/run" >=> runAction
                ]
            ]