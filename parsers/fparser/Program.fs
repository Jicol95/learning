open System

type Parser<'T> = P of (string -> int -> Option<'T * int>)

let pchar: Parser<char> = 
    P <| fun s i->
        if (i < s.Length) then
            Some(s.[i], i+1)
        else    
            None

let prun (P p) s = p s 0

let preturn v : Parser<'T> = 
    P <| fun s i -> Some(v, i)

let pfail () : Parser<'T> =
    P <| fun s i -> None

let pbind (uf : 'T -> Parser<'U>) (P t) : Parser<'U> =
    P <| fun s i -> 
        match t s i with
        | None -> None
        | Some (tv, ti) ->
            let (P u) = uf tv
            u s ti

let pcombine u t = t |> pbind (fun _ -> u)

type ParserBuilder () =
    class
        // Enables let!
        member x.Bind (t, uf) = pbind uf t
        // Enables do!
        member x.Combine (t, u) = pcombine u t 
        // Enables return
        member x.Return v = preturn v
        // Enables return!
        member x.ReturnFrom p = p : Parser<'T> 
        // Allows if x then expr with no else
        member x.Zero = preturn ()
    end

let parser = ParserBuilder ()

[<EntryPoint>]
let main argv =
    let p = parser {
        let! first = pchar
        let! second = pchar
        let! third = pchar
        return first, second, third
    }

    prun p "Hello jicol95, you are parsing a char" |> printfn "%A"

    0 // exit code
