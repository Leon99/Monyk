FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY src/Monyk.GroundControl.Main/Monyk.GroundControl.Main.csproj src/Monyk.GroundControl.Main/
COPY src/Monyk.Common.Communicator/Monyk.Common.Communicator.csproj src/Monyk.Common.Communicator/
COPY src/Monyk.GroundControl.Db/Monyk.GroundControl.Db.csproj src/Monyk.GroundControl.Db/
COPY src/Monyk.Common.Models/Monyk.Common.Models.csproj src/Monyk.Common.Models/
COPY Monyk.GroundControl.Models/Monyk.GroundControl.Models.csproj Monyk.GroundControl.Models/
COPY src/Monyk.GroundControl.Services/Monyk.GroundControl.Services.csproj src/Monyk.GroundControl.Services/
COPY src/Monyk.Common.Startup/Monyk.Common.Startup.csproj src/Monyk.Common.Startup/
RUN dotnet restore src/Monyk.GroundControl.Main/Monyk.GroundControl.Main.csproj
COPY . .
WORKDIR /src/src/Monyk.GroundControl.Main
RUN dotnet build Monyk.GroundControl.Main.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Monyk.GroundControl.Main.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Monyk.GroundControl.Main.dll"]