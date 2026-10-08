FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Backend/UMS.Api/UMS.Api.csproj Backend/UMS.Api/
RUN dotnet restore Backend/UMS.Api/UMS.Api.csproj

COPY Backend/UMS.Api/ Backend/UMS.Api/
RUN dotnet publish Backend/UMS.Api/UMS.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "UMS.Api.dll"]
