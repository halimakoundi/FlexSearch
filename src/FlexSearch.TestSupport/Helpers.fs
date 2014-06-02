﻿namespace FlexSearch.TestSupport

open Xunit
open FlexSearch.Api
open FlexSearch.Api.Message
open FlexSearch.Core
open FlexSearch.Core.Validator
open NSubstitute
open Ploeh.AutoFixture
open Ploeh.AutoFixture.AutoNSubstitute
open Ploeh.AutoFixture.DataAnnotations
open Ploeh.AutoFixture.Xunit
open System
open System.Linq
open Xunit.Extensions
open Xunit.Sdk

[<AutoOpen>]
module Helpers = 
    let TestChoice choice expectedChoice1 = 
        match choice with
        | Choice1Of2(success) -> 
            if expectedChoice1 then Assert.Equal(1, 1)
            else Assert.Equal(1, 2)
        | Choice2Of2(error) -> 
            if expectedChoice1 then Assert.Equal(1, 2)
            else Assert.Equal(1, 1)
    
    /// <summary>
    /// Gets the Choice1 (success) option
    /// </summary>
    /// <param name="choice"></param>
    let GetSuccessChoice(choice : Choice<'T, 'U>) = 
        match choice with
        | Choice1Of2(success) -> success
        | Choice2Of2(error) -> failwith "Expected the result to be success but received failure."
    
    /// <summary>
    /// Returns success if Choice1 is present
    /// </summary>
    /// <param name="choice"></param>
    let ExpectSuccess(choice : Choice<'T, OperationMessage>) = 
        match choice with
        | Choice1Of2(success) -> Assert.True(true)
        | Choice2Of2(error) -> 
            Assert.True(false, "Expected the result to be success but received failure: " + error.DeveloperMessage)
    
    /// <summary>
    /// Expects the DU to contains Choice2 (Error)
    /// </summary>
    /// <param name="choice"></param>
    /// <param name="operationMessage"></param>
    let ExpectErrorCode (operationMessage : OperationMessage) (choice : Choice<'T, OperationMessage>) = 
        match choice with
        | Choice1Of2(success) -> Assert.True(1 = 2, "Expecting error but received success")
        | Choice2Of2(error) -> Assert.Equal(operationMessage.ErrorCode, error.ErrorCode)