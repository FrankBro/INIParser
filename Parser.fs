module INIParser

open System
open FParsec

type SectionName= string
type Key = string
type Value = string

module internal Parser =
    let ws = spaces
    let str p = pstring p
    let strWs p = pstring p .>> ws

    type Parser<'t> = Parser<'t, unit>

    let identifier : Parser<string> =
        many1Satisfy2 Char.IsLetter Char.IsLetterOrDigit

    let comment : Parser<unit> =
        strWs "#" >>. skipRestOfLine true .>> ws

    let sectionName : Parser<SectionName> =
        between (str "[") (str "]") (many1 (noneOf [']']))
        |>> (Array.ofList >> String)

    let anyText s =
        many1Satisfy (fun ch -> (not <| Char.IsWhiteSpace(ch))
                                 && not ( ch = ')'
                                          ||  ch = '('
                                          ||  ch = ']'
                                          ||  ch = '['
                                          ||  ch = ','
                                          ||  ch = ';'
                                          ||  ch = '='
                                          )
                      ) s

    let sectionEntry : Parser<Key * Value> =
        identifier .>> strWs "=" .>>. anyText .>> ws

    let noise : Parser<unit> =
        skipMany (comment <|> spaces1)

    let section : Parser<SectionName * Map<Key, Value>> =
        sectionName .>> noise .>>.
        ((many (sectionEntry .>> noise)) |>> Map.ofList)

    let ini = noise >>. many section |>> Map.ofList

let read str =
    match run Parser.ini str with
    | Success (result, _, _) -> Some result
    | Failure (_msg, _, _) -> None

let readFile name =
    let str = System.IO.File.ReadAllText(name)
    read str
