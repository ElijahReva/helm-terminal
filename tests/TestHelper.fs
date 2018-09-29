namespace Jeff.Tests

open FsUnit.Xunit 

module Option = 
    let shouldBeSome (opt: 'a option) = 
        opt.IsSome |> should equal true
        opt.Value
        
    let shouldBeNone (opt: 'a option) = 
        opt.IsNone |> should equal true
        ()
    
    
    