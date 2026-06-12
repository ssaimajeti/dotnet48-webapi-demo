# Build stage
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2019 AS build
WORKDIR /app

# Copy solution and project files
COPY *.sln .
COPY *.csproj .
COPY packages.config .

# Restore NuGet packages
RUN nuget restore

# Copy all source files
COPY . .

# Build the application
RUN msbuild /p:Configuration=Release /p:DeployOnBuild=true /p:PublishProfile=FolderProfile /p:PublishUrl=C:\publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2019
WORKDIR /inetpub/wwwroot

# Copy published application
COPY --from=build /publish .

# Expose port 80
EXPOSE 80

# IIS runs as a service, no explicit entrypoint needed
# The base image handles IIS startup