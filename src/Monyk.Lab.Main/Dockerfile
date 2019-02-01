FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY src/Monyk.Lab.Main/Monyk.Lab.Main.csproj src/Monyk.Lab.Main/
COPY Monyk.GroundControl.ApiClient/Monyk.GroundControl.ApiClient.csproj Monyk.GroundControl.ApiClient/
COPY Monyk.GroundControl.Models/Monyk.GroundControl.Models.csproj Monyk.GroundControl.Models/
COPY src/Monyk.Common.Models/Monyk.Common.Models.csproj src/Monyk.Common.Models/
COPY src/Monyk.Common.Startup/Monyk.Common.Startup.csproj src/Monyk.Common.Startup/
COPY src/Monyk.Common.Communicator/Monyk.Common.Communicator.csproj src/Monyk.Common.Communicator/
RUN dotnet restore src/Monyk.Lab.Main/Monyk.Lab.Main.csproj
COPY . .
WORKDIR /src/src/Monyk.Lab.Main
RUN dotnet build Monyk.Lab.Main.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Monyk.Lab.Main.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Monyk.Lab.Main.dll"]
