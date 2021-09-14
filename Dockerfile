#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY *.sln .
COPY BetterDaysContactBook.API/*.csproj BetterDaysContactBook.API/


COPY BetterDaysContactBook.Database/*.csproj BetterDaysContactBook.Database/
COPY BetterDaysContactBook.Models/*.csproj BetterDaysContactBook.Models/
COPY BetterDaysContactBook.Core/*.csproj BetterDaysContactBook.Core/
COPY BetterDaysContactBook.Common/*.csproj BetterDaysContactBook.Common/

RUN dotnet restore BetterDaysContactBook.API/*.csproj

COPY . .
WORKDIR /src/BetterDaysContactBook.API
RUN dotnet build
# RUN dotnet build "BetterDaysContactBook.API.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR /src/BetterDaysContactBook.API
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


# ENTRYPOINT ["dotnet", "BetterDaysContactBook.API.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet BetterDaysContactBook.API.dll