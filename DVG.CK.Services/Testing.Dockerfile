#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM registry-ap.nextgen-global.com:5000/dotnet-core/aspnet-3.1:latest AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["DVG.CK.Services/DVG.CK.Services.csproj", "DVG.CK.Services/"]
COPY ["DVG.WIS.Business/DVG.WIS.Business.csproj", "DVG.WIS.Business/"]
COPY ["DVG.WIS.Core/DVG.WIS.Core.csproj", "DVG.WIS.Core/"]
COPY ["DVG.WIS.Utilities/DVG.WIS.Utilities.csproj", "DVG.WIS.Utilities/"]
COPY ["DVG.WIS.PublicModel/DVG.WIS.PublicModel.csproj", "DVG.WIS.PublicModel/"]
COPY ["DVG.WIS.Entities/DVG.WIS.Entities.csproj", "DVG.WIS.Entities/"]
COPY ["DVG.WIS.DAL/DVG.WIS.DAL.csproj", "DVG.WIS.DAL/"]
COPY ["DVG.WIS.Caching/DVG.WIS.Caching.csproj", "DVG.WIS.Caching/"]
RUN dotnet restore "DVG.CK.Services/DVG.CK.Services.csproj" -s http://172.16.0.66:8083/v3/index.json ||  dotnet restore "DVG.CK.Services/DVG.CK.Services.csproj" -s  https://api.nuget.org/v3/index.json

COPY . .
WORKDIR "/src/DVG.CK.Services"
#RUN dotnet build "DVG.CK.Services.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DVG.CK.Services.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV TZ=Asia/Ho_Chi_Minh
ENTRYPOINT ["dotnet", "DVG.CK.Services.dll","--environment=Testing"]