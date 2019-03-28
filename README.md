# INIParser

Very simple INI parser. Supports comments and empty lines as well as whitespace before key/values.

Everything needs to be in a section. Every key/value are strings. This is the structure once parsed.

```fsharp
type SectionName = string
type Key = string
type Value = string
Map<SectionName, Map<Key, Value>>
```

```
[section1]
key1=value1
key2=value2
[section2]
key3=value3
```


## Build

```
dotnet build
```

## Test

```
cd Tests
dotnet test
```