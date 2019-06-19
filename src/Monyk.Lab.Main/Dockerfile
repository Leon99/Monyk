FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/Monyk.Lab.Main/Monyk.Lab.Main.csproj", "src/Monyk.Lab.Main/"]
COPY ["src/Monyk.Common.Db/Monyk.Common.Db.csproj", "src/Monyk.Common.Db/"]
COPY ["src/Monyk.Common.Startup/Monyk.Common.Startup.csproj", "src/Monyk.Common.Startup/"]
COPY ["src/Monyk.Common.Communicator/Monyk.Common.Communicator.csproj", "src/Monyk.Common.Communicator/"]
COPY ["src/Monyk.Common.Models/Monyk.Common.Models.csproj", "src/Monyk.Common.Models/"]
COPY ["src/Monyk.Lab.Db/Monyk.Lab.Db.csproj", "src/Monyk.Lab.Db/"]
COPY ["src/Monyk.Lab.Models/Monyk.Lab.Models.csproj", "src/Monyk.Lab.Models/"]
COPY ["src/Monyk.GroundControl.ApiClient/Monyk.GroundControl.ApiClient.csproj", "src/Monyk.GroundControl.ApiClient/"]
COPY ["src/Monyk.GroundControl.Models/Monyk.GroundControl.Models.csproj", "src/Monyk.GroundControl.Models/"]
RUN dotnet restore "src/Monyk.Lab.Main/Monyk.Lab.Main.csproj"
COPY . .
WORKDIR "/src/src/Monyk.Lab.Main"
RUN dotnet build "Monyk.Lab.Main.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Monyk.Lab.Main.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Monyk.Lab.Main.dll"]