# Builder
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /build

# Copy csproj and restore as distinct layers
COPY ["packages","./packages"]
COPY ["aspnetcore/nuget.config","./Indexer/nuget.config"]
COPY ["aspnetcore/src/Indexer/Indexer.csproj","./Indexer/Indexer.csproj"]
COPY ["aspnetcore/src/ElasticService/ElasticService.csproj","./ElasticService/ElasticService.csproj"]
COPY ["aspnetcore/src/DatabaseContext/DatabaseContext.csproj","./DatabaseContext/DatabaseContext.csproj"]
COPY ["aspnetcore/src/Repositories/Repositories.csproj","./Repositories/Repositories.csproj"]
COPY ["aspnetcore/src/Service.Models/Service.Models.csproj","./Service.Models/Service.Models.csproj"]
COPY ["aspnetcore/src/Logging/Logging.csproj","./Logging/Logging.csproj"]
RUN dotnet restore "Indexer/Indexer.csproj"

# Copy everything else and build
COPY ["aspnetcore/src/Indexer/","./Indexer"]
COPY ["aspnetcore/src/ElasticService/","./ElasticService"]
COPY ["aspnetcore/src/DatabaseContext/","./DatabaseContext"]
COPY ["aspnetcore/src/Repositories/","./Repositories"]
COPY ["aspnetcore/src/Service.Models/","./Service.Models"]
COPY ["aspnetcore/src/Logging/","./Logging"]
RUN dotnet publish "Indexer/Indexer.csproj" -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=build-env /build/out .
ENTRYPOINT ["dotnet", "CSC.PublicApi.Indexer.dll"]