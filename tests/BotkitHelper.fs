// Auto-Generated by FAKE; do not edit
namespace System

open System.Reflection
open Xunit

[<assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly, MaxParallelThreads = 1, DisableTestParallelization = true)>]
do ()
    
namespace Jeff.Tests


open Serilog

open System   
open System.IO
open System.Diagnostics

module BotkitHelper =
    
    [<Literal>]
    let neo4jVersion = "3.3.1"
