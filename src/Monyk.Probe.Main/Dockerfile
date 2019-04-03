FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["Monyk.Probe.Main/Monyk.Probe.Main.csproj", "Monyk.Probe.Main/"]
COPY ["Monyk.Common.Communicator/Monyk.Common.Communicator.csproj", "Monyk.Common.Communicator/"]
COPY ["Monyk.Common.Startup/Monyk.Common.Startup.csproj", "Monyk.Common.Startup/"]
COPY ["Monyk.Probe.Checkers/Monyk.Probe.Checkers.csproj", "Monyk.Probe.Checkers/"]
COPY ["Monyk.Common.Models/Monyk.Common.Models.csproj", "Monyk.Common.Models/"]
RUN dotnet restore "Monyk.Probe.Main/Monyk.Probe.Main.csproj"
COPY . .
WORKDIR "/src/Monyk.Probe.Main"
RUN dotnet build "Monyk.Probe.Main.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Monyk.Probe.Main.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Monyk.Probe.Main.dll"]