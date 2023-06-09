open System

type Parser<'T> = P of (string -> int -> Option<'T * int>)

let pchar: Parser<char> = 
    P <| fun s i->
        if (i < s.Length) then
            Some(s.[i], i+1)
        else    
            None

let prun (P p) s = p s 0

[<EntryPoint>]
let main argv =
    prun pchar "Hello jicol95, you are parsing a char" |> printfn "%A"
    0 // exit code
