#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM registry-ap.nextgen-global.com:5000/dotnet-core/aspnet-3.1:latest AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY projectfiles.tar .
RUN tar -xvf projectfiles.tar

RUN dotnet restore "DVG.CK.OMS.PublicApi/DVG.CK.OMS.PublicApi.csproj" -s http://172.16.0.66:8083/v3/index.json ||  dotnet restore "DVG.CK.OMS.PublicApi/DVG.CK.OMS.PublicApi.csproj" -s  https://api.nuget.org/v3/index.json

COPY . .
WORKDIR "/src/DVG.CK.OMS.PublicApi"
#RUN dotnet build "DVG.CK.OMS.PublicApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DVG.CK.OMS.PublicApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV TZ=Asia/Ho_Chi_Minh
ENTRYPOINT ["dotnet", "DVG.CK.OMS.PublicApi.dll","--environment=Testing"]