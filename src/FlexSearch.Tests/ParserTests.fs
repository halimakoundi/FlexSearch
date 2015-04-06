﻿module ParserTests

open FParsec
open FlexSearch.Core

let parser = new FlexParser() :> IFlexParser

let test p str = 
    match FParsec.CharParsers.run p str with
    | Success(_, _, _) -> ()
    | Failure(errorMsg, _, _) -> raise <| invalidOp (errorMsg)

let test2 str = 
    match parser.Parse(str) with
    | Choice1Of2(_) -> ()
    | Choice2Of2(errorMsg) -> raise <| invalidOp (errorMsg.ToString())

type SearchParserTests() = 
    member __.``Single escape character should be accepted``() = 
        test FlexSearch.Core.Parsers.stringLiteral "'abc \\' pqr'"
    
    [<InlineData("['abc']")>]
    [<InlineData("['abc','pqr']")>]
    [<InlineData("['abc'  ,  'pqr']")>]
    [<InlineData("[         'abc'          ]")>]
    [<InlineData("[    'abc'    ]")>]
    member __.``Input should be parsed for the 'List of Values'`` (sut : string) = 
        test FlexSearch.Core.Parsers.listOfValues sut
    
    [<InlineData("abc eq 'a'")>]
    [<InlineData("not abc eq 'a'")>]
    [<InlineData("(abc eq '1')")>]
    [<InlineData("abc eq 'as' {boost: '21'}")>]
    [<InlineData("abc eq 'as' {boost:'21',applydelete:'true'}")>]
    [<InlineData("abc eq 'a' and pqr eq 'b'")>]
    [<InlineData("abc eq 'a' or pqr eq 'b'")>]
    [<InlineData("abc eq 'a' and ( pqr eq 'b')")>]
    [<InlineData("(abc eq 'a') and pqr eq 'b'")>]
    [<InlineData("((((((abc eq 'a'))))) and (pqr eq 'b'))")>]
    [<InlineData("abc eq 'a' and pqr eq 'b' or abc eq '1'")>]
    [<InlineData("abc eq ['sdsd', '2', '3']")>]
    [<InlineData("abc > '12'")>]
    [<InlineData("abc >= '12'")>]
    [<InlineData("abc >= '1\\'2'")>]
    [<InlineData("not (abc eq 'sdsd' and abc eq 'asasa') and pqr eq 'asas'")>]
    [<InlineData("abc eq 'a' AND pr eq 'b'")>]
    member __.``Simple expression should parse`` (sut : string) = test2 sut
    
    [<InlineData("f1: 'v1',f2 : 'v2'", 2)>]
    [<InlineData(" f1:  'v1' , f2 : 'v2'", 2)>]
    [<InlineData("   f1           : 'v1'     ", 1)>]
    [<InlineData("        f1: 'v1',f2:'v2',f3 : 'v3'", 3)>]
    [<InlineData("f1 : 'v\\'1',f2 : 'v2'", 2)>]
    member __.``Search Profile QueryString should parse`` (sut : string, expected : int) = 
        match ParseQueryString(sut, false) with
        | Choice1Of2(result) -> <@ result.Count = expected @>
        | Choice2Of2(_) -> raise <| invalidOp ("Expected query string to pass")
    
    [<InlineData("abc ='1234'")>]
    [<InlineData("abc ='a1234'")>]
    member __.``Expressions with spacing issues should parse`` (sut : string) = test2 sut