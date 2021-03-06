﻿open FlexSearch.Core
open System
open System.IO
open Topshelf
open FlexSearch.Logging

[<EntryPoint>]
let main argv = 
    let logger = LogService.GetLogger(false)
    // Capture all unhandled errors
    AppDomain.CurrentDomain.UnhandledException.Subscribe
        (fun x -> logger.TraceError(sprintf "Unhandled exception in application: %A" x.ExceptionObject)) 
    |> ignore
    // Load server settings
    let settings = 
        match ServerSettings.GetFromFile(Path.Combine(Constants.ConfFolder, "Config.yml"), new YamlFormatter() :> IFormatter) with
        | Choice1Of2(s) -> s
        | Choice2Of2(e) -> 
            logger.TraceError("Error parsing 'Config.yml' file.", e)
            failwithf "%s" e.UserMessage
    // Load all plug-in DLLs
    for file in Directory.EnumerateFiles(Constants.PluginFolder, "*.dll", SearchOption.TopDirectoryOnly) do
        try 
            System.Reflection.Assembly.LoadFile(file) |> ignore
        with e -> logger.TraceError("Error loading plug-in library.", e)
    let TopShelfConfiguration() = 
        HostFactory.Run(fun conf -> 
            conf.RunAsLocalSystem() |> ignore
            conf.SetDescription("FlexSearch Server")
            conf.SetDisplayName("FlexSearch Server")
            conf.SetServiceName("FlexSearch-Server")
            conf.StartAutomatically() |> ignore
            conf.EnableServiceRecovery(fun rc -> rc.RestartService(1) |> ignore) |> ignore
            conf.Service<NodeService>(fun factory -> 
                ServiceConfiguratorExtensions.ConstructUsing(factory, fun () -> new NodeService(settings, logger, false)) 
                |> ignore
                ServiceConfiguratorExtensions.WhenStarted(factory, fun tc -> tc.Start()) |> ignore
                ServiceConfiguratorExtensions.WhenStopped(factory, fun tc -> tc.Stop()) |> ignore)
            |> ignore)
        |> int
    TopShelfConfiguration()
