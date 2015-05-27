#nuget spec

nuget pack RoutePlannerLib.csproj -Symbols -prop Configuration=Release

nuget push FHNW-Lab11-Test.1.0.0.0.nupkg
nuget push FHNW-Lab11-Test.1.0.0.0.symbols.nupkg