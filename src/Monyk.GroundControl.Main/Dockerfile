FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["Monyk.GroundControl.Main/Monyk.GroundControl.Main.csproj", "Monyk.GroundControl.Main/"]
COPY ["Monyk.Common.Communicator/Monyk.Common.Communicator.csproj", "Monyk.Common.Communicator/"]
COPY ["Monyk.GroundControl.Db/Monyk.GroundControl.Db.csproj", "Monyk.GroundControl.Db/"]
COPY ["Monyk.GroundControl.Models/Monyk.GroundControl.Models.csproj", "Monyk.GroundControl.Models/"]
COPY ["Monyk.Common.Models/Monyk.Common.Models.csproj", "Monyk.Common.Models/"]
COPY ["Monyk.GroundControl.Services/Monyk.GroundControl.Services.csproj", "Monyk.GroundControl.Services/"]
COPY ["Monyk.Common.Startup/Monyk.Common.Startup.csproj", "Monyk.Common.Startup/"]
COPY ["Monyk.GroundControl.Api/Monyk.GroundControl.Api.csproj", "Monyk.GroundControl.Api/"]
RUN dotnet restore "Monyk.GroundControl.Main/Monyk.GroundControl.Main.csproj"
COPY . .
WORKDIR "/src/Monyk.GroundControl.Main"
RUN dotnet build "Monyk.GroundControl.Main.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Monyk.GroundControl.Main.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Monyk.GroundControl.Main.dll"]