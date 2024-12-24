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


##### Steps to Add the Entities

**1. create a new branch from the 'api' branch**

**2. create the entity 'T' in the domain layer as child class  'GenericEntity'**

**3. create the interface 'ITService' in the application layer as child interface 'IGenericService'**

**4. create the service 'TService' in the application layer**

**5. inject the new service into the application layer in the 'DependencyInjection' file**

**6. create the repository 'TRepository' in the infrastructure layer** 

**7. create the controller for the new service in the Api layer**

**8. perform the migrations and check the dataBase**

**9. create a new pull request and wait for the merge to be approved ;)**
