FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
ARG mainFileName
ENV MainFileName=$mainFileName

WORKDIR /app
COPY . .

EXPOSE 443
ENTRYPOINT dotnet ${MainFileName}.dll