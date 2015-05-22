#nuget spec

nuget pack RoutePlannerLib.csproj -Symbols -prop Configuration=Release

#nuget push RoutePlannerLib.1.0.0.0.nupkg