```powershell

dotnet ef migrations add InitialMigration --project DownTrack.Infrastructure --startup-project DownTrack.API

dotnet ef database update --project DownTrack.Infrastructure --startup-project DownTrack.API 

dotnet ef migrations list --project DownTrack.Infrastructure --startup-project DownTrack.API 

dotnet ef migrations remove --project DownTrack.Infrastructure --startup-project DownTrack.API

dotnet ef database update InitialMigration_1 --project DownTrack.Infrastructure --startup-project DownTrack.API

dotnet run --project .\DownTrack.API\

```