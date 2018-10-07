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
    open System.Threading.Tasks
    open System.Threading.Tasks
    open System
    open Giraffe
    open Giraffe.HttpStatusCodeHandlers.RequestErrors
    open Microsoft.AspNetCore.Http
    open Newtonsoft.Json

    let private badRequest (ctx: HttpContext)  =
        fun error ->
            ctx.SetStatusCode 400
            error
            |> ctx.WriteJsonAsync

    let private fromOption (ctx: HttpContext) =
        fun opt -> 
            match opt with 
            | Some i -> i |> ctx.WriteJsonAsync
            | None -> "" |> badRequest ctx 


    let private getKubeContexts : HttpHandler =
        fun next ctx ->              
             DOM.getContexts () |> fromOption ctx
             
    let private getKubeNamespaces : HttpHandler =
        fun next ctx ->
            kube "." "get ns -o json" |> JsonConvert.DeserializeObject |> ctx.WriteJsonAsync
            
              
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
                        route "/current" >=> currentSchema                        
                        route "/contexts" >=> getKubeContexts                     
                        route "/namespaces" >=> getKubeNamespaces                     
                    ]
                ]
            POST >=> 
                choose [
                    route "/run" >=> runAction
                ]
            ]