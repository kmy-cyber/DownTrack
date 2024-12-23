##### HOW TO RUN IT?

```bash
\dotnet new sln -n DownTrack
dotnet new webapi -n DownTrack.API
dotnet new classlib -n DownTrack.Domain
dotnet new classlib -n DownTrack.Application
dotnet new classlib -n DownTrack.Infrastructure

dotnet sln add (ls -r **\*.csproj)

dotnet add .\DownTrack.Api\ reference .\DownTrack.Application\ .\DownTrack.Infrastructure\
dotnet add .\DownTrack.Infrastructure\ reference .\DownTrack.Application\ 
dotnet add .\DownTrack.Application\ reference .\DownTrack.Domain\

dotnet build


```
