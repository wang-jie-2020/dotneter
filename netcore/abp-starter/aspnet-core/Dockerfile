FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV TZ=Asia/Shanghai
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
WORKDIR "/src/host/AESC.Starter.HttpApi.Host"
RUN dotnet build "AESC.Starter.HttpApi.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AESC.Starter.HttpApi.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .    
ENTRYPOINT ["dotnet", "AESC.Starter.HttpApi.Host.dll"]

