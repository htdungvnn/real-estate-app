# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the official .NET SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["PropertyService.csproj", "./"]
RUN dotnet restore

# Copy the remaining source files and build the app
COPY . .
RUN dotnet build -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Create the runtime container
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PropertyService.dll"]