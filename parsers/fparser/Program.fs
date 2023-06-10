open System

type Parser<'T> = P of (string -> int -> Option<'T * int>)

let pchar: Parser<char> = 
    P <| fun (s: string) (i: int)->
        if (i < s.Length) then
            Some(s.[i], i+1)
        else    
            None

let prun (P p) (s: string) = p s 0

let preturn (v: 'T) : Parser<'T> = 
    P <| fun (s: string) (i: int) -> Some(v, i)

let pfail () : Parser<'T> =
    P <| fun (s: string) (i: int) -> None

let pbind (uf : 'T -> Parser<'U>) (P t) : Parser<'U> =
    P <| fun (s: string) (i: int) -> 
        match t s i with
        | None -> None
        | Some (tv: 'T, ti: int) ->
            let (P u) = uf tv
            u s ti

let pcombine u t = t |> pbind (fun _ -> u)

let pmany (P t) : Parser<'T list> = 
    P <| fun (s: string) (pos: int) ->
        let rec loop (vs: 'T list) (currentPos: int) =
            match t s currentPos with
            | None -> Some (List.rev vs, currentPos)
            | Some (tvalue: 'T, tpos: int) -> loop (tvalue::vs) tpos
        loop [] pos


type ParserBuilder () =
    class
        // Enables let!
        member x.Bind (t: Parser<'a>, uf) = pbind uf t
        // Enables do!
        member x.Combine (t: Parser<'a>, u: Parser<'a>) = pcombine u t 
        // Enables return
        member x.Return (v: 'a) = preturn v
        // Enables return!
        member x.ReturnFrom (p: Parser<'T>) = p : Parser<'T> 
        // Allows if x then expr with no else
        member x.Zero = preturn ()
    end


let parser: ParserBuilder = ParserBuilder ()

let psatisfy (satisfy: char -> bool): Parser<char> = parser {
    let! (char: char) = pchar
    if satisfy char then 
        return char
    else 
        return! pfail()
}

let pmany1 (t: Parser<'a>) = parser {
    let! (head: 'a) = t
    let! (tail: 'a list) = pmany t
    return head::tail
}

let pmap map (t: Parser<'a>) = parser {
    let! (v: 'a) = t
    return map v
}

let makeint (vs: char list) =
    let folder (aggr: int) (ch: char) = 
        aggr * 10 + (int ch - int '0')

    vs |> List.fold folder 0

let pdigit: Parser<char> = psatisfy Char.IsDigit

let pint: Parser<int> = pmany1 pdigit |> pmap makeint

let pletter: Parser<char> = psatisfy Char.IsLetter

let pwhitespace: Parser<char> = psatisfy Char.IsWhiteSpace

let pskipchar ch = parser {
    let! char = pchar
    if ch = char then   
        return ()
    else
        return! pfail ()
}

let pstring (t : Parser<char>) : Parser<string> =
    pmany1 t |> pmap (fun charList -> charList |> List.toArray |> String)

let pkeyvalue = parser {
    let! left = pstring pletter
    do! pskipchar '='
    let! right = pint
    return (left, right)
}

let pkeyvalues = parser {
    let helper = parser {
        let! ws = pmany1 pwhitespace
        return! pkeyvalue 
    }

    let! head = pkeyvalue
    let! tail = pmany helper
    return (head, tail)
}

[<EntryPoint>]
let main _ =
    prun pkeyvalues """dockerPort=8080
        postgresPort=5045
        universe=42
    """ |> printfn "%A"

    0 // exit code
