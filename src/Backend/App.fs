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

    
    let private getKubeContexts : HttpHandler =
        fun next ctx ->        
             DOM.getContexts() |> ctx.WriteJsonAsync
              
    let private runAction : HttpHandler =
        fun next ctx ->        
             DOM.getContexts() |> ctx.WriteJsonAsync
              
              
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
                    ]
                ]
            POST >=> 
                choose [
                    route "/run" >=> runAction
                ]
            ]