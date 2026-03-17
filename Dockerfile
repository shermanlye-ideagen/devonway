FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src/DevonWay.Api/ ./DevonWay.Api/
RUN dotnet publish DevonWay.Api/DevonWay.Api.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet", "DevonWay.Api.dll"]
