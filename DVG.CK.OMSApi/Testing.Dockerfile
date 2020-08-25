#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM registry-ap.nextgen-global.com:5000/dotnet-core/aspnet-3.1:latest AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src

COPY ["DVG.CK.OMSApi/DVG.CK.OMSApi.csproj", "DVG.CK.OMSApi/"]
COPY ["Base.Logging/Base.Logging.csproj", "Base.Logging/"]
COPY ["DVG.WIS.Entities/DVG.WIS.Entities.csproj", "DVG.WIS.Entities/"]
COPY ["DVG.WIS.Core/DVG.WIS.Core.csproj", "DVG.WIS.Core/"]
COPY ["DVG.WIS.Utilities/DVG.WIS.Utilities.csproj", "DVG.WIS.Utilities/"]
COPY ["DVG.WIS.PublicModel/DVG.WIS.PublicModel.csproj", "DVG.WIS.PublicModel/"]
COPY ["DVG.WIS.Business/DVG.WIS.Business.csproj", "DVG.WIS.Business/"]
COPY ["DVG.WIS.DAL/DVG.WIS.DAL.csproj", "DVG.WIS.DAL/"]
COPY ["DVG.WIS.Caching/DVG.WIS.Caching.csproj", "DVG.WIS.Caching/"]
RUN dotnet restore "DVG.CK.OMSApi/DVG.CK.OMSApi.csproj" -s http://172.16.0.66:8083/v3/index.json ||dotnet restore "DVG.CK.OMSApi/DVG.CK.OMSApi.csproj" -s  https://api.nuget.org/v3/index.json

COPY . .
WORKDIR "/src/DVG.CK.OMSApi"
#RUN dotnet build "DVG.CK.OMSApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DVG.CK.OMSApi.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV TZ=Asia/Ho_Chi_Minh
ENTRYPOINT ["dotnet", "DVG.CK.OMSApi.dll","--environment=Testing"]