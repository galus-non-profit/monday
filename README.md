### this app shows simply example of true CQRS and asynchronous post request with MediatR and hangfire jobs
flow of this example is shown below:
![Alt text](/assets/flow.png)

# Sonarqube
## Create sonarqube instance

- run sonqrqube locally by docker compose (create docker-compose.yaml file not in project folder - this is global docker compose):

```yaml
services:
  sonarqube:
    image: sonarqube:latest
    command: "-Dsonar.search.javaAdditionalOpts=-Dnode.store.allow_mmap=false"
    ports:
      - "9000:9000"
    environment:
      - SONAR_ES_BOOTSTRAP_CHECKS_DISABLE=true
```

in terminal use commands:
```bash
docker compose pull 
docker compose up -d
```
It could take some time. 

- visit localhost:9000 to login to sonarqube. It will require change password from default (admin, admin) credentials

## Create local project in sonarqube dashboard

![Alt text](/assets/create-project.png)

click "next" and choose Use the global setting radio button on next screen, then click "Create project"

- on next screen choose analysis method as "Locally"

- on displayed view generate project token:

![Alt text](/assets/token.png)

choose "no expiration option" and click "Generate"

- don't forget to copy your token to notepad
![Alt text](/assets/token-copy.png)

- on next screen choose .NET, so you can see commands needed to run code scan
![Alt text](/assets/commands.png)

Those commands require java installation

## Install Java and dotnet tools

```bash
# java
winget install Microsoft.OpenJDK.17 # newest on 01.2024

# dotnet 
dotnet tool install --global dotnet-sonarscanner
dotnet tool install --global dotnet-coverage # dotnet coverage can be install later
```
## run your first scan 

- run commands copied from dashboard:
```bash
dotnet sonarscanner begin /k:"Monday" /d:sonar.host.url="http://localhost:9000"  /d:sonar.token="sqp_a8f605392448431faee7931d8be3648cc27c645d"
dotnet build
dotnet sonarscanner end /d:sonar.token="sqp_a8f605392448431faee7931d8be3648cc27c645d"
```
wait to complete commands and see results in sonarqube dashboard. 

Typical results are shown below
![Alt text](/assets/r1.png)

after click on given bug/smell etc you can see details and suggested way to fix:
![Alt text](/assets/r2.png)

- rerun commands and check results ( view has switch New Code/Overall Code use it to display impact of your latest changes or actual code condition)

## Install Makefile to run scan easier

- download and install makefile from:

https://sourceforge.net/projects/gnuwin32/files/make/3.81/make-3.81.exe/download?use_mirror=deac-fra&download=

- append to PATH environment variable two items:
```
C:\Program Files (x86)\GnuWin32\bin
%JAVA_HOME%\bin
```
To edit environment variables easy, you can use free Rapid Environment Editor. 

- open terminal and type "make" to check if tool works properly 
- create file name "MakeFile" (without extension) in project main directory
Makefile can not use four space instead of tab. To create/edit Makefile use simple text editor like notepad. 

MakeFile should look like this:
```
all:
	dotnet sonarscanner begin /k:"Monday" /d:sonar.host.url="http://localhost:9000"  /d:sonar.token="sqp_a8f605392448431faee7931d8be3648cc27c645d"
	dotnet build
	dotnet sonarscanner end /d:sonar.token="sqp_a8f605392448431faee7931d8be3648cc27c645d"
```

go to project main directory in terminal and run simply command "make". It will run new scan. 

## Ignore warnings from scan

in some cases we want to consciously ignore error warnings. Each warnings/smell or bug has its code:
![Alt text](/assets/smell.png)

If you want to ignore this warning add xml to projects csproj file of specific project

```xml
<PropertyGroup>
    <NoWarn>S1125;</NoWarn>
</PropertyGroup>
```

or add its globally to file named Directory.Packages.props in main directory


## Add unit tests to sonarqube

- create test project, test solution in project main path and add project references

```bash
dotnet new mstest --output WebApi.UnitTests --name Monday.WebApi.UnitTests

dotnet new sln --name Monday

dotnet sln add .\tests\WebApi.UnitTests\

dotnet sln add .\src\WebApi.UnitTests\
```

- to upload test results to sonarqube we need to modify our begin command in Makefile:

by adding 
```/d:sonar.cs.vscoveragexml.reportsPaths=coverage.xml```

Makefile file should looks like this:

```bash
all:
	dotnet sonarscanner begin /k:"Monday" /d:sonar.host.url="http://localhost:9000"  /d:sonar.token="sqp_a8f605392448431faee7931d8be3648cc27c645d" /d:sonar.cs.vscoveragexml.reportsPaths=coverage.xml
	dotnet build --no-incremental
	dotnet-coverage collect 'dotnet test' -f xml  -o 'coverage.xml'
	dotnet sonarscanner end /d:sonar.token="sqp_a8f605392448431faee7931d8be3648cc27c645d"
	
```

Click test coverage to see suggestions:

![Alt text](/assets/tests_suggestions.png)

Adding few simple tests increase test coverage:

![Alt text](/assets/coverage.png)

# ignore sonarqube data in git:

- add to .gitignore two lines:

```
# Sonar
.sonarqube
```