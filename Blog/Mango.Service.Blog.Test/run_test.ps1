Remove-Item -Path "TestResults" -Recurse
dotnet test --collect:"XPlat Code Coverage"
reportgenerator "-reports:TestResults\*\coverage.cobertura.xml" "-targetdir:TestResults\coveragereport" -reporttypes:Html