namespace Helm.Terminal

module Bootstrap = 
    
    open Helm.Terminal.Server

    open System
    open System.IO

    open Giraffe
    
    open Microsoft.AspNetCore
    open Microsoft.AspNetCore.Hosting
    open Microsoft.AspNetCore.Builder
    open Microsoft.AspNetCore.SignalR
    open Microsoft.AspNetCore.HttpOverrides
    open Microsoft.AspNetCore.Cors.Infrastructure
    open Microsoft.AspNetCore.SpaServices.Webpack

    open Microsoft.Extensions
    open Microsoft.Extensions.Configuration
    open Microsoft.Extensions.DependencyInjection
    open Serilog
    open Serilog.Events  
    open System.Diagnostics
    
    type ServerError =    
        {
            Message: string
            Stack: string
        }
    
    let errorHandler (ex : Exception) (logger : Logging.ILogger) =
        Log.Error(ex, "An unhandled exception has occurred while executing the request. {EventId}", Logging.EventId())
        clearResponse >=> setStatusCode 500 >=> (fun a ctx -> 
                let ex = ex.Demystify()
                { Message = ex.Message; Stack = ex.StackTrace} |> ctx.WriteJsonAsync
            )
        
    type Startup(configuration: IConfiguration) =
        member this.Configuration = configuration

        member this.ConfigureServices (services:IServiceCollection) =
            services
                .AddGiraffe()
                .AddSignalR()
                |> ignore
        
        member this.Configure (app: IApplicationBuilder , env: IHostingEnvironment) : unit =
            if env.IsDevelopment()  then
                
                let webPackOptions = new WebpackDevMiddlewareOptions() 
                webPackOptions.HotModuleReplacement <- true
                
                app .UseDeveloperExceptionPage()
                    .UseWebpackDevMiddleware(webPackOptions)
                    |> ignore                   
            
                
            let headers = new ForwardedHeadersOptions()
            headers.ForwardedHeaders <- (ForwardedHeaders.XForwardedFor + ForwardedHeaders.XForwardedProto)
            
            let api = 
                this
                    .Configuration
                    .GetSection("app")
                    .Get<AppConfiguration>()
                    |> App.webApp
                    
            app.UseForwardedHeaders(headers)
               .UseGiraffeErrorHandler(errorHandler)
               .UseSignalR(fun route -> route.MapHub<ManagerHub>(Http.PathString("/managerhub")))
               .UseStaticFiles()
               .UseMiddleware<GiraffeMiddleware>(api)
               .UseSpa(fun s -> s |> ignore);
    
    let createConfiguration (args: string[]) =
            let basePath = logValue "ConfgDirectory" <| Directory.GetCurrentDirectory() 
            (new ConfigurationBuilder())
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.Development.json", true, true)
                .AddCommandLine(args)
                .Build()
    
    let logger() =
            Log.Logger <- 
                (new LoggerConfiguration())
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
                    .MinimumLevel.Debug()                                    
                    .Enrich.WithDemystifiedStackTraces()
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .CreateLogger();
            ()
        
    let builderWebHost cfg args : IWebHostBuilder = 
        
        WebHost.CreateDefaultBuilder(args)
            .UseConfiguration(cfg)
            .UseStartup<Startup>()
            .UseSerilog()
    
    
    let buildWebHost args : IWebHost = 
       let cfg = createConfiguration args
       args |> builderWebHost cfg |> fun x -> x.Build()   
