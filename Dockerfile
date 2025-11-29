FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY YurtBursu.Api.csproj .
RUN dotnet restore YurtBursu.Api.csproj

# Copy all source files
COPY Controllers/ ./Controllers/
COPY Data/ ./Data/
COPY DTOs/ ./DTOs/
COPY Middleware/ ./Middleware/
COPY Migrations/ ./Migrations/
COPY Models/ ./Models/
COPY Repositories/ ./Repositories/
COPY Services/ ./Services/
COPY Program.cs .
COPY appsettings.json .

# Verify Models folder is copied (for debugging)
RUN ls -la Models/ || dir Models

# Build and publish
RUN dotnet publish YurtBursu.Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENTRYPOINT ["dotnet", "YurtBursu.Api.dll"]
