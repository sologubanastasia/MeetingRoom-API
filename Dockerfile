FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["MeetingRoom.Domain/MeetingRoom.Domain.csproj", "MeetingRoom.Domain/"]
COPY ["MeetingRoom.Application/MeetingRoom.Application.csproj", "MeetingRoom.Application/"]
COPY ["MeetingRoom.Infrastructure/MeetingRoom.Infrastructure.csproj", "MeetingRoom.Infrastructure/"]
COPY ["MeetingRoom.WebApi/MeetingRoom.WebApi.csproj", "MeetingRoom.WebApi/"]
RUN dotnet restore "MeetingRoom.WebApi/MeetingRoom.WebApi.csproj"
COPY . .
RUN dotnet publish "MeetingRoom.WebApi/MeetingRoom.WebApi.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MeetingRoom.WebApi.dll"]
