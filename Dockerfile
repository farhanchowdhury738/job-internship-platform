# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["HireLink.csproj", "./"]
RUN dotnet restore "HireLink.csproj"

COPY . .
RUN dotnet publish "HireLink.csproj" --configuration Release --output /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render supplies PORT at runtime. 10000 is a useful local-container default.
EXPOSE 10000
ENTRYPOINT ["sh", "-c", "dotnet HireLink.dll --urls http://0.0.0.0:${PORT:-10000}"]
