all:
	dotnet sonarscanner begin /k:"Monday" /d:sonar.host.url="http://localhost:9000"  /d:sonar.token="sqp_a8f605392448431faee7931d8be3648cc27c645d" /d:sonar.cs.vscoveragexml.reportsPaths=coverage.xml
	dotnet build --no-incremental
	dotnet-coverage collect 'dotnet test' -f xml  -o 'coverage.xml'
	dotnet sonarscanner end /d:sonar.token="sqp_a8f605392448431faee7931d8be3648cc27c645d"
	