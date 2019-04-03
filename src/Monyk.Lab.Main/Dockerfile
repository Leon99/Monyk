FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["Monyk.Lab.Main/Monyk.Lab.Main.csproj", "Monyk.Lab.Main/"]
COPY ["Monyk.Common.Models/Monyk.Common.Models.csproj", "Monyk.Common.Models/"]
COPY ["Monyk.Common.Startup/Monyk.Common.Startup.csproj", "Monyk.Common.Startup/"]
COPY ["Monyk.Common.Communicator/Monyk.Common.Communicator.csproj", "Monyk.Common.Communicator/"]
COPY ["Monyk.GroundControl.ApiClient/Monyk.GroundControl.ApiClient.csproj", "Monyk.GroundControl.ApiClient/"]
COPY ["Monyk.GroundControl.Models/Monyk.GroundControl.Models.csproj", "Monyk.GroundControl.Models/"]
RUN dotnet restore "Monyk.Lab.Main/Monyk.Lab.Main.csproj"
COPY . .
WORKDIR "/src/Monyk.Lab.Main"
RUN dotnet build "Monyk.Lab.Main.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Monyk.Lab.Main.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Monyk.Lab.Main.dll"]