# Builder
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["aspnetcore/PublicApi.sln", "./"]
COPY ["aspnetcore/src/ElasticSearchIndexer/ElasticSearchIndexer.csproj", "src/ElasticSearchIndexer/"]
COPY ["aspnetcore/src/Api/Api.csproj", "src/Api/"]
COPY ["aspnetcore/Api.Test/Api.Test.csproj", "./Api.Test/"]
RUN dotnet restore
#RUN dotnet restore ElasticSearchIndexer/ElasticSearchIndexer.csproj
#RUN dotnet restore api/Api.csproj
# Copy everything else and build

COPY ["aspnetcore/src/Api/.", "src/Api/"]
COPY ["aspnetcore/src/ElasticSearchIndexer/.", "src/ElasticSearchIndexer/"]
COPY ["aspnetcore/src/Api.Test/.", "src/Api.Test/"]
WORKDIR /app

RUN dotnet publish "src/ElasticSearchIndexer/ElasticSearchIndexer.csproj" -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
#EXPOSE 8080
ENTRYPOINT ["dotnet", "ElasticSearchIndexer.dll"]