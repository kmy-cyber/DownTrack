npm run dev --prefix frontend/ | grep -oP http://localhost:.* &
dotnet run --project api/src/DownTrack.API | grep -oP http://localhost:[0-9]+ | xargs -n 1 xdg-open