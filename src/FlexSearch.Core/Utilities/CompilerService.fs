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

open FlexSearch.Utility
open FlexSearch.Api
open System
open CSScriptLibrary
open FlexSearch.Api.Message
open System.ComponentModel.Composition
open System.ComponentModel.Composition.Hosting
open System.Collections.Generic
open System.Reflection

[<AutoOpen>]
module CompilerService = 
    let private codeBaseProfileSelector = 
        lazy (let path = Helpers.GenerateAbsolutePath(".\Conf\Scripts\_ProfileSelectorScriptTemplate.cs")
              Helpers.LoadFile(path))
    
    let private codeBaseComputedField = 
        lazy (let path = Helpers.GenerateAbsolutePath(".\Conf\Scripts\_ComputedFieldScriptTemplate.cs")
              Helpers.LoadFile(path))
    
    // ----------------------------------------------------------------------------
    // Concerete implementation of IScriptFactory
    // ----------------------------------------------------------------------------    
    type ScriptFactory<'a when 'a : not struct>(codeTemplate : string) = 
        interface IScriptFactory<'a> with
            member this.CompileScript(script : ScriptProperties) = 
                let sourceCode = codeTemplate.Replace("[SourceCode]", script.Source)
                try 
                    let compiledScript = CSScript.Evaluator.LoadCode(sourceCode)
                    let castMethod = compiledScript :?> 'a
                    Choice1Of2(castMethod)
                with e -> 
                    Choice2Of2
                        (OperationMessage.WithDeveloperMessage(MessageConstants.SCRIPT_CANT_BE_COMPILED, e.Message))
    
    // ----------------------------------------------------------------------------
    // Concerete implementation of IScriptFactoryCollection
    // ----------------------------------------------------------------------------                        
    type ScriptFactoryCollection() = 
        let profileSelectorScriptFactory = 
            new ScriptFactory<IFlexProfileSelectorScript>(codeBaseProfileSelector.Value) :> IScriptFactory<IFlexProfileSelectorScript>
        let computedFieldScriptFactory = 
            new ScriptFactory<IComputedFieldScript>(codeBaseComputedField.Value) :> IScriptFactory<IComputedFieldScript>
        let customScoringScriptFactory = 
            new ScriptFactory<ICustomScoringScript>("") :> IScriptFactory<ICustomScoringScript>
        interface IScriptFactoryCollection with
            member this.ProfileSelectorScriptFactory = profileSelectorScriptFactory
            member this.ComputedFieldScriptFactory = computedFieldScriptFactory
            member this.CustomScoringScriptFactory = customScoringScriptFactory
