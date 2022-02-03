# Builder
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
# Copy csproj and restore as distinct layers
#COPY aspnetcore/src/api/api.csproj ./
COPY ["aspnetcore/src/Api/Api.csproj", "./"]
#RUN dotnet restore
RUN dotnet restore
# Copy everything else and build
#COPY aspnetcore/src/api/ ./
COPY aspnetcore/src/api/ ./
RUN dotnet publish -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Api.dll"]