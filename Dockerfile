FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /App

# Copy only the project file first
COPY *.csproj ./

# Restore as distinct layers
RUN dotnet restore

# Copy the rest of the app's files
COPY . ./

COPY NuGet.Config /root/.nuget/NuGet/

# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App

# Set environment
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 8080

COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "ControllerLayer.dll"]