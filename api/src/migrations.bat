
dotnet ef migrations add InitialMigration99090 --project DownTrack.Infrastructure --startup-project DownTrack.API

dotnet ef database update --project DownTrack.Infrastructure --startup-project DownTrack.API 