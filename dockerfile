# Etapa 1: Base para el entorno de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Etapa 2: Construcción (SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar la solución y los archivos de proyecto (.csproj) primero.
# Esto aprovecha el caché de las capas de Docker para no tener que descargar 
# los paquetes NuGet cada vez que cambias un archivo de código fuente.
COPY ["AuthService2.sln", "./"]
COPY ["src/AuthService.Api/AuthService.Api.csproj", "src/AuthService.Api/"]
COPY ["src/AuthService.Application/AuthService.Application.csproj", "src/AuthService.Application/"]
COPY ["src/AuthService.Domain/AuthService.Domain.csproj", "src/AuthService.Domain/"]
COPY ["src/AuthService.Persistence/AuthService.Persistence.csproj", "src/AuthService.Persistence/"]

# Restaurar dependencias
RUN dotnet restore "src/AuthService.Api/AuthService.Api.csproj"

# Copiar el resto del código fuente
COPY . .

# Cambiar el directorio de trabajo al proyecto principal y compilar
WORKDIR "/src/src/AuthService.Api"
RUN dotnet build "AuthService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa 3: Publicación
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AuthService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa 4: Imagen final
FROM base AS final
WORKDIR /app
# Copiar los binarios publicados desde la etapa de publicación a la imagen final
COPY --from=publish /app/publish .

# Definir el punto de entrada de la aplicación
ENTRYPOINT ["dotnet", "AuthService.Api.dll"]