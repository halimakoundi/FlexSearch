﻿// ----------------------------------------------------------------------------
// Flexsearch settings (Settings.fs)
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

open FlexSearch.Api
open FlexSearch.Core
open FlexSearch.Utility
open System
open System.Collections.Generic
open System.Net
open System.Xml
open System.Xml.Linq

[<AutoOpen>]
module Store = 
    open System.Data.SQLite
    open System.IO
    open System.Linq
    
    /// A reusable key value persistance store build on top of sqlite
    type PersistanceStore(path : string, isMemory : bool) = 
        let sqlCreateTable = """
            CREATE TABLE [keyvalue] (
            [id] INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
            [key] TEXT  NOT NULL,
            [value] TEXT  NOT NULL,
            [type] TEXT  NOT NULL,
            [timestamp] TIMESTAMP  NOT NULL
            );

            CREATE INDEX [keyvalue_idx] ON [keyvalue](
            [key]  DESC,
            [type]  DESC
            );
            """
        let mutable db = None
        
        do 
            let sqlLiteDbPath = path
            
            let connectionString = 
                if isMemory then "Data Source=:memory:;Version=3;New=True;UseUTF16Encoding=True;"
                else sprintf "Data Source=%s;Version=3;UseUTF16Encoding=True;" sqlLiteDbPath
            
            let dbConnection = 
                if isMemory then 
                    let dbConnection = new SQLiteConnection(connectionString)
                    dbConnection.Open()
                    let command = new SQLiteCommand(sqlCreateTable, dbConnection)
                    command.ExecuteNonQuery() |> ignore
                    dbConnection
                elif File.Exists(sqlLiteDbPath) then 
                    let dbConnection = new SQLiteConnection(connectionString)
                    dbConnection.Open()
                    dbConnection
                else 
                    SQLiteConnection.CreateFile(sqlLiteDbPath)
                    let dbConnection = new SQLiteConnection(connectionString)
                    dbConnection.Open()
                    let command = new SQLiteCommand(sqlCreateTable, dbConnection)
                    command.ExecuteNonQuery() |> ignore
                    dbConnection
            
            db <- Some(dbConnection)
        
        member private this.get<'T> (key) = 
            let instanceType = typeof<'T>.FullName
            if key = "" then None
            else 
                let sql = sprintf "select * from keyvalue where type = '%s' and key = '%s' limit 1" instanceType key
                let command = new SQLiteCommand(sql, db.Value)
                let reader = command.ExecuteReader()
                let mutable result : 'T = Unchecked.defaultof<'T>
                while reader.Read() do
                    result <- Newtonsoft.Json.JsonConvert.DeserializeObject<'T>(reader.GetString(2))
                Some(result)
        
        interface IPersistanceStore with
            member this.Get<'T>(key) = this.get<'T> (key)
            
            member this.GetAll<'T>() = 
                let sql = sprintf "select * from keyvalue where type= '%s'" typeof<'T>.FullName
                let command = new SQLiteCommand(sql, db.Value)
                let reader = command.ExecuteReader()
                let mutable result : List<'T> = new List<'T>()
                while reader.Read() do
                    result.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<'T>(reader.GetString(2)))
                result :> IEnumerable<'T>
            
            member this.Delete<'T>(key) = 
                let instanceType = typeof<'T>.FullName
                if key = "" then false
                else 
                    let sql = sprintf "DELETE FROM keyvalue WHERE type='%s' and key='%s'" instanceType key
                    let command = new SQLiteCommand(sql, db.Value)
                    command.ExecuteNonQuery() |> ignore
                    true
            
            member this.Put<'T> (key) (instance : 'T) = 
                let instanceType = typeof<'T>.FullName
                if key = "" then false
                else 
                    let sql = sprintf "select * from keyvalue where type='%s' and key='%s' limit 1" instanceType key
                    let command = new SQLiteCommand(sql, db.Value)
                    let reader = command.ExecuteReader()
                    let mutable result = false
                    while reader.Read() do
                        result <- true
                    let value = Newtonsoft.Json.JsonConvert.SerializeObject(instance)
                    if result then 
                        // Exists so lets update it 
                        let sql = "update keyvalue SET type = @type, key = @key, value = @value, timestamp = @timestamp where type = @type and key = @key" 
                        let command = new SQLiteCommand(sql, db.Value)
                        command.Parameters.Add("@type", Data.DbType.String).Value <- instanceType
                        command.Parameters.Add("@key", Data.DbType.String).Value <- key
                        command.Parameters.Add("@value", Data.DbType.String).Value <- value
                        command.Parameters.Add("@timestamp", Data.DbType.String).Value <- DateTime.Now.ToString()
                        command.ExecuteNonQuery() |> ignore
                    else 
                        // Does not exist so create it
                        let sql = "insert into keyvalue (type, key, value, timestamp) values (@type, @key, @value, @timestamp)" 
                        let command = new SQLiteCommand(sql, db.Value)
                        command.Parameters.Add("@type", Data.DbType.String).Value <- instanceType
                        command.Parameters.Add("@key", Data.DbType.String).Value <- key
                        command.Parameters.Add("@value", Data.DbType.String).Value <- value
                        command.Parameters.Add("@timestamp", Data.DbType.String).Value <- DateTime.Now.ToString()
                        try
                            command.ExecuteNonQuery() |> ignore
                        with e -> ()
                    true
