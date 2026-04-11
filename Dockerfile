# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Argumentos de build para configurar o Frontend (Blazor WASM é estático)
ARG BACKEND_URL=http://localhost:5204
ARG STRIPE_PUBLIC_KEY

# Otimização de cache: Restaura as dependências primeiro
COPY ["Dima.sln", "./"]
COPY ["Dima.Api/Dima.Api.csproj", "Dima.Api/"]
COPY ["Dima.Web/Dima.Web.csproj", "Dima.Web/"]
COPY ["Dima.Core/Dima.Core.csproj", "Dima.Core/"]
COPY ["Dima.Tests/Dima.Tests.csproj", "Dima.Tests/"]
RUN dotnet restore

# Copia o restante dos arquivos
COPY . .

# Atualiza as configurações do Blazor WASM antes do publish
RUN sed -i "s|\"BackendUrl\": \".*\"|\"BackendUrl\": \"$BACKEND_URL\"|g" Dima.Web/wwwroot/appsettings.json
RUN if [ ! -z "$STRIPE_PUBLIC_KEY" ]; then \
    sed -i "s|\"StripePublicKey\": \".*\"|\"StripePublicKey\": \"$STRIPE_PUBLIC_KEY\"|g" Dima.Web/wwwroot/appsettings.json; \
    fi

# Compila e publica
RUN dotnet publish "Dima.Api/Dima.Api.csproj" -c Release -o /app/publish/api
RUN dotnet publish "Dima.Web/Dima.Web.csproj" -c Release -o /app/publish/web

# Runtime da API (ASP.NET Core)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime_api
WORKDIR /app
COPY --from=build /app/publish/api .

# Variáveis de ambiente padrão para a API (Podem ser sobrescritas no docker-compose)
ENV ConnectionStrings__DefaultConnection=""
ENV BackendUrl="http://localhost:5204"
ENV FrontendUrl="http://localhost:5182"
ENV StripeApiKey=""

EXPOSE 80
ENTRYPOINT ["dotnet", "Dima.Api.dll"]

# Runtime do Web (Nginx para arquivos estáticos)
FROM nginx:alpine AS runtime_web
WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/web/wwwroot .
EXPOSE 80