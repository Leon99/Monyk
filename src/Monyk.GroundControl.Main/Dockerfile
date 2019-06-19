FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/Monyk.GroundControl.Main/Monyk.GroundControl.Main.csproj", "src/Monyk.GroundControl.Main/"]
COPY ["src/Monyk.Common.Db/Monyk.Common.Db.csproj", "src/Monyk.Common.Db/"]
COPY ["src/Monyk.Common.Startup/Monyk.Common.Startup.csproj", "src/Monyk.Common.Startup/"]
COPY ["src/Monyk.Common.Communicator/Monyk.Common.Communicator.csproj", "src/Monyk.Common.Communicator/"]
COPY ["src/Monyk.GroundControl.Db/Monyk.GroundControl.Db.csproj", "src/Monyk.GroundControl.Db/"]
COPY ["src/Monyk.GroundControl.Models/Monyk.GroundControl.Models.csproj", "src/Monyk.GroundControl.Models/"]
COPY ["src/Monyk.Common.Models/Monyk.Common.Models.csproj", "src/Monyk.Common.Models/"]
COPY ["src/Monyk.GroundControl.Db.Migrations/Monyk.GroundControl.Db.Migrations.csproj", "src/Monyk.GroundControl.Db.Migrations/"]
COPY ["src/Monyk.GroundControl.Services/Monyk.GroundControl.Services.csproj", "src/Monyk.GroundControl.Services/"]
COPY ["src/Monyk.GroundControl.Api/Monyk.GroundControl.Api.csproj", "src/Monyk.GroundControl.Api/"]
RUN dotnet restore "src/Monyk.GroundControl.Main/Monyk.GroundControl.Main.csproj"
COPY . .
WORKDIR "/src/src/Monyk.GroundControl.Main"
RUN dotnet build "Monyk.GroundControl.Main.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Monyk.GroundControl.Main.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Monyk.GroundControl.Main.dll"]