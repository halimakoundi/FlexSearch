﻿// ----------------------------------------------------------------------------
// (c) Seemant Rajvanshi, 2013
//
// This source code is subject to terms and conditions of the Apache License, Version 2.0. A 
// copy of the license can be found in the License.txt file at the root of this distribution. 
// By using this source code in any fashion, you are agreeing to be bound 
// by the terms of the Apache License, Version 2.0.
//
// You must not remove this notice, or any other, from this software.
// ----------------------------------------------------------------------------
namespace FlexSearch.Core

open System
open System.IO

/// Contains all the flex constants which do not change per instance
[<AutoOpen>]
[<RequireQualifiedAccess>]
module Constants = 
    // Flex root folder path
    let private rootFolder = AppDomain.CurrentDomain.SetupInformation.ApplicationBase
    let private confFolder = Path.Combine(rootFolder, "Conf")
    let private pluginFolder = Path.Combine(rootFolder, "Plugins")
    
    /// Flex index folder
    let ConfFolder = 
        Directory.CreateDirectory(confFolder) |> ignore
        confFolder
    
    /// Flex plug-in folder
    let PluginFolder = 
        Directory.CreateDirectory(pluginFolder) |> ignore
        pluginFolder

    /// Extension to be used by settings file
    let SettingsFileExtension = ".yml"
