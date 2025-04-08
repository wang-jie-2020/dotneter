#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#RUN apt-get update && apt-get install -y python3.11 libpython3.11 
#USER app
#WORKDIR /app
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY . .
#RUN dotnet restore "pythoncsharp.csproj"
#RUN dotnet build "pythoncsharp.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "pythoncsharp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#
#ENTRYPOINT ["dotnet", "pythoncsharp.dll"]

#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.



FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /root
RUN apt update \
    && apt install wget build-essential zlib1g-dev libncurses5-dev libgdbm-dev libnss3-dev libssl-dev libsqlite3-dev libreadline-dev libffi-dev curl libbz2-dev -y \
    && wget https://www.python.org/ftp/python/3.10.11/Python-3.10.11.tgz \
    && tar xzf Python-3.10.11.tgz \
    && cd Python-3.10.11 \
    && ./configure --enable-optimizations --enable-shared \
    && make altinstall \
    && cp libpython3.10.so.1.0 /usr/lib/ \
	&& ln -s /usr/local/bin/python3.10 /usr/bin/python \
	&& ln -s /usr/local/bin/pip3.10 /usr/bin/pip \
    && rm -r /root/Python-3.10.11 \
    && apt-get clean && rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/*

#FROM net8python310 AS base
#WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . .
RUN dotnet restore "pythoncsharp.csproj"
RUN dotnet build "pythoncsharp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "pythoncsharp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV PYTHONNET_PYDLL=/usr/lib/libpython3.10.so.1.0
ENV PYTHONNET_PYPATH=/usr/local/lib/python310.zip:/usr/local/lib/python3.10:/usr/local/lib/python3.10/lib-dynload:/usr/local/lib/python3.10/site-packages:/app/scripts
RUN pip install -r /app/requirements.txt
ENTRYPOINT ["dotnet", "pythoncsharp.dll"]


#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#RUN apt update && apt install python3 python3-pip python3-venv \
#    && python3 -m venv /work && source /work/bin/activate
#  
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY . .
#RUN dotnet restore "pythoncsharp.csproj"
#RUN dotnet build "pythoncsharp.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "pythoncsharp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#
#ENV PYTHONNET_PYDLL=/usr/lib/x86_64-linux-gnu/libpython3.11.so.1.0
#ENV PYTHONNET_PYPATH=/usr/local/lib/python310.zip:/usr/local/lib/python3.10:/usr/local/lib/python3.10/lib-dynload:/usr/local/lib/python3.10/site-packages:/app/scripts
#RUN pip install -r /app/requirements.txt
#ENTRYPOINT ["dotnet", "pythoncsharp.dll"]