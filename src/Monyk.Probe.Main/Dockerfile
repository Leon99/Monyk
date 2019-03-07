FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["src/Monyk.Probe.Main/Monyk.Probe.Main.csproj", "src/Monyk.Probe.Main/"]
COPY ["src/Monyk.Common.Communicator/Monyk.Common.Communicator.csproj", "src/Monyk.Common.Communicator/"]
COPY ["src/Monyk.Common.Startup/Monyk.Common.Startup.csproj", "src/Monyk.Common.Startup/"]
COPY ["src/Monyk.Probe.Checkers/Monyk.Probe.Checkers.csproj", "src/Monyk.Probe.Checkers/"]
COPY ["src/Monyk.Common.Models/Monyk.Common.Models.csproj", "src/Monyk.Common.Models/"]
RUN dotnet restore "src/Monyk.Probe.Main/Monyk.Probe.Main.csproj"
COPY . .
WORKDIR "/src/src/Monyk.Probe.Main"
RUN dotnet build "Monyk.Probe.Main.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Monyk.Probe.Main.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Monyk.Probe.Main.dll"]