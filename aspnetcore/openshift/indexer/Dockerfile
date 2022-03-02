# Builder
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["aspnetcore/src/ElasticSearchIndexer/ElasticSearchIndexer.csproj", "ElasticSearchIndexer/"]
COPY ["aspnetcore/src/Api/Api.csproj", "Api/"]
RUN dotnet restore
#RUN dotnet restore ElasticSearchIndexer/ElasticSearchIndexer.csproj
#RUN dotnet restore api/Api.csproj
# Copy everything else and build

COPY Api/. ./Api
COPY ElasticSearchIndexer/. ./ElasticSearchIndexer
WORKDIR /app

RUN dotnet publish -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
#EXPOSE 8080
ENTRYPOINT ["dotnet", "ElasticSearchIndexer.dll"]