FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY TaskManagerApi/*.csproj ./TaskManagerApi/
COPY TaskManagerApi.Tests/*.csproj ./TaskManagerApi.Tests/
RUN dotnet restore ./TaskManagerApi/TaskManagerApi.csproj

COPY . .
WORKDIR /src/TaskManagerApi
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TaskManagerApi.dll"]