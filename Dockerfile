FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5073

ENV ASPNETCORE_URLS=http://+:5073
ENV DOTNET_NOLOGO=true
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o arquivo .csproj para o diretório de trabalho atual no contêiner
COPY poc.api.mongodb.csproj .

# Restaura as dependências do projeto
RUN dotnet restore poc.api.mongodb.csproj

# Copia o restante dos arquivos do projeto para o contêiner
COPY . .

# Define o diretório de trabalho e constrói o projeto
WORKDIR /src
RUN dotnet build poc.api.mongodb.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish poc.api.mongodb.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "poc.api.mongodb.dll"]
