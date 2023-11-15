# PoCs
Proof of Concept for C# Code

## Content

| Name | Comments |
|------|----------|
| Utf8Json vs Newtonsoft | Comparison in the Serialization / Deserialization speed between Utf8Json and Newtonsoft |
| Libsodium integration | A Proof of Concept to use libsodium on c# for a secure user / password managment |

### Utf8Json vs Newtonsoft

```
BenchmarkDotNet v0.13.10, macOS Sonoma 14.0 (23A344) [Darwin 23.0.0]
Apple M1, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD
```
| Method                                   | Mean       | Error    | StdDev   | Ratio | Gen0   | Gen1   | Allocated | Alloc Ratio |
|----------------------------------------- |-----------:|---------:|---------:|------:|-------:|-------:|----------:|------------:|
| ReadObjectFromStringWithNewtonsoft       | 4,899.8 ns |  8.99 ns |  7.97 ns |  1.00 | 1.8158 | 0.0458 |  11.13 KB |        1.00 |
| ReadObjectFromStringWithUtf8Json         | 1,148.8 ns |  2.59 ns |  2.16 ns |  0.23 | 0.5817 |      - |   3.57 KB |        0.32 |
| ReadObjectFromMemoryStreamWithNewtonsoft | 6,256.1 ns | 14.95 ns | 12.48 ns |  1.28 | 2.3270 | 0.0534 |   14.3 KB |        1.29 |
| ReadObjectMemoryStreamWithUtf8Json       |   930.0 ns | 18.57 ns | 25.41 ns |  0.19 | 0.1726 |      - |   1.06 KB |        0.10 |
