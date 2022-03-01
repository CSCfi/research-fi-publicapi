# Builder
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
# Copy csproj and restore as distinct layers
COPY ["aspnetcore/src/ElasticSearchIndexer/ElasticSearchIndexer.csproj", "./"]
COPY ["aspnetcore/src/Api/Api.csproj", "api/"]
RUN dotnet restore ElasticSearchIndexer.csproj
RUN dotnet restore api/Api.csproj
# Copy everything else and build
COPY . .
RUN dotnet publish "ElasticSearchIndexer.csproj" -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
#EXPOSE 8080
ENTRYPOINT ["dotnet", "ElasticSearchIndexer.dll"]