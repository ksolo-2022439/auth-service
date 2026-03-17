# Etapa de Base - Runtime de .NET 8
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Etapa de Construcción - SDK de .NET 8
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto (.csproj) primero para aprovechar el cache de Docker
COPY ["AuthService.Api/AuthService.Api.csproj", "AuthService.Api/"]
COPY ["AuthService.Application/AuthService.Application.csproj", "AuthService.Application/"]
COPY ["AuthService.Domain/AuthService.Domain.csproj", "AuthService.Domain/"]
COPY ["AuthService.Persistence/AuthService.Persistence.csproj", "AuthService.Persistence/"]

# Restaurar dependencias
RUN dotnet restore "AuthService.Api/AuthService.Api.csproj"

# Copiar el resto del código fuente
COPY . .

# Compilar el proyecto de la API
WORKDIR "/src/AuthService.Api"
RUN dotnet build "AuthService.Api.csproj" -c Release -o /app/build

# Etapa de Publicación
FROM build AS publish
RUN dotnet publish "AuthService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa Final - Imagen de ejecución
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthService.Api.dll"]