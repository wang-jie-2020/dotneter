#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Yi.Admin/Yi.Admin.csproj", "src/Yi.Admin/"]
COPY ["src/Yi.AspNetCore/Yi.AspNetCore.csproj", "src/Yi.AspNetCore/"]
COPY ["src/Yi.Framework/Yi.Framework.csproj", "src/Yi.Framework/"]
COPY ["src/Yi.System/Yi.System.csproj", "src/Yi.System/"]


RUN dotnet restore "./src/Yi.Admin/Yi.Admin.csproj"
COPY . .

RUN dotnet build "./src/Yi.Admin/Yi.Admin.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./src/Yi.Admin/Yi.Admin.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Yi.Admin.dll"]