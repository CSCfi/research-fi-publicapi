# Builder
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /build

# Copy csproj and restore as distinct layers
COPY ["packages","./packages"]
COPY ["aspnetcore/nuget.config","./Interface/nuget.config"]
COPY ["aspnetcore/src/Interface/Interface.csproj","./Interface/Interface.csproj"]
COPY ["aspnetcore/src/ElasticService/ElasticService.csproj","./ElasticService/ElasticService.csproj"]
COPY ["aspnetcore/src/Service.Models/Service.Models.csproj","./Service.Models/Service.Models.csproj"]
COPY ["aspnetcore/src/ApiModels/ApiModels.csproj","./ApiModels/ApiModels.csproj"]
COPY ["aspnetcore/src/Logging/Logging.csproj","./Logging/Logging.csproj"]
RUN dotnet restore "Interface/Interface.csproj"

# Copy everything else and build
COPY ["aspnetcore/src/Interface/","./Interface"]
COPY ["aspnetcore/src/ElasticService/","./ElasticService"]
COPY ["aspnetcore/src/Service.Models/","./Service.Models"]
COPY ["aspnetcore/src/ApiModels/","./ApiModels"]
COPY ["aspnetcore/src/Logging/","./Logging"]
RUN dotnet publish "Interface/Interface.csproj" -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /build/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "CSC.PublicApi.Interface.dll"]