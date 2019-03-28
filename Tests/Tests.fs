module Tests

open System
open Xunit

open INIParser

let map = Map.ofList
let topmap = map >> Some
let namedmap s v = s, map v

module Assert =
    let strictEqual expected actual = Assert.StrictEqual(expected, actual)

[<Fact>]
let ``Normal text`` () =
    [
        "[main]"
        "field1=characters"
        "field2=12345"
        "field3=/with/slashes.and.dot"
    ]
    |> String.concat "\n"
    |> INIParser.read
    |> Assert.strictEqual (
        [
            [
                "field1", "characters"
                "field2", "12345"
                "field3", "/with/slashes.and.dot"
            ]
            |> namedmap "main"
        ]
        |> topmap
    )

[<Fact>]
let ``Comments and new lines in the middle`` () =
    [
        ""
        "# This is a comment"
        ""
        "[main]"
        ""
        "field1=characters"
        "# Another comment"
        "field2=12345"
        "field3=/with/slashes.and.dot"
        "# Another comment"
        ""
    ]
    |> String.concat "\n"
    |> INIParser.read
    |> Assert.strictEqual (
        [
            [
                "field1", "characters"
                "field2", "12345"
                "field3", "/with/slashes.and.dot"
            ]
            |> namedmap "main"
        ]
        |> topmap
    )

[<Fact>]
let ``Whitespaces before more keyvals is legit`` () =
    [
        "[main]"
        "  field1=characters"
        "field2=12345"
        "               field3=/with/slashes.and.dot"
    ]
    |> String.concat "\n"
    |> INIParser.read
    |> Assert.strictEqual (
        [
            [
                "field1", "characters"
                "field2", "12345"
                "field3", "/with/slashes.and.dot"
            ]
            |> namedmap "main"
        ]
        |> topmap
    )
