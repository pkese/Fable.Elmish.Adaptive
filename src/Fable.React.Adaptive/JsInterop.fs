﻿namespace Fable.React.Adaptive.JsHelpers

open Fable.Core
open Fable.Core.JsInterop


module internal Performance = 
    [<Emit("performance.now()")>]
    let now() : float = jsNative

type internal Timeout = interface end
module internal Timeout = 
    [<Emit("setTimeout($1, $0)")>]
    let setTimeout (delay : int) (action : unit -> unit) : Timeout = jsNative

    [<Emit("clearTimeout($0)")>]
    let clearTimeout (t : Timeout) : unit = jsNative

module internal JsType = 
    [<Emit("typeof $0 === \"function\"")>]
    let isFunction (o : obj) : bool = jsNative
    
    [<Emit("typeof $0 === \"object\"")>]
    let isObject (o : obj) : bool = jsNative
    
    [<Emit("typeof $0 === \"string\"")>]
    let isString (o : obj) : bool = jsNative

    [<Emit("typeof $0 === \"number\"")>]
    let isNumber (o : obj) : bool = jsNative
   
[<AutoOpen>]
module internal JsHelperExtensions = 

    [<Emit("Object.defineProperty($0, $1, $2)")>]
    let private defineProperty (o : obj) (name : string) (prop : obj) : unit = jsNative

    type System.Object with
        [<Emit("Object.assign($0, $1...)")>]
        member x.Assign([<System.ParamArray>] others : obj[]) : unit = jsNative
            
        [<Emit("delete $0[$1]")>]
        member x.Delete(key : string) : unit = jsNative

        [<Emit("Object.keys($0)")>]
        static member GetKeys (o : obj) : seq<string> = jsNative

        member inline x.Keys = System.Object.GetKeys x

        member x.DefineProperty(name : string, getter : unit -> 'a, ?setter : 'a -> unit) =  
            match setter with
            | Some setter ->
                defineProperty x name (createObj ["get", box getter; "set", box setter])
            | None ->
                defineProperty x name (createObj ["get", box getter])
                

    [<Emit("arguments")>]
    let arguments : obj[] = jsNative

    type Fable.Core.JsInterop.JsFunc with
        static member inline Arguments : obj[] = arguments


