// Note that the FParsec package may not be suitable for production since it reads all text completely into memory.
// Howewer, it uses verifiable code (no unmanaged pointers). ;)
//
// The alternative package, FParsec-Big-Data, *does not* work with .NET stardard / .NET Core.
//
// See the documentation at http://www.quanttec.com/fparsec/download-and-installation.html.

#r @"c:\Users\larry.jones\.nuget\packages\fparsec\1.0.4-rc3\lib\netstandard1.6\FParsecCS.dll"
#r @"c:\Users\larry.jones\.nuget\packages\fparsec\1.0.4-rc3\lib\netstandard1.6\FParsec.dll"

open FParsec

pfloat
