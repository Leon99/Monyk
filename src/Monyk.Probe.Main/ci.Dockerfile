FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
ARG mainFileName
ENV MainFileName=$mainFileName

WORKDIR /app
COPY . .

EXPOSE 443
ENTRYPOINT dotnet ${MainFileName}.dll