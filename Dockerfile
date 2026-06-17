# ===== Estágio de Build =====
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Build args do frontend (Blazor WASM é estático: o que precisa ir no bundle
# tem que ser definido em tempo de build).
ARG BACKEND_URL=http://localhost:5204
ARG STRIPE_PUBLIC_KEY

# Otimização de cache: restaura as dependências primeiro
COPY ["Dima.sln", "./"]
COPY ["Dima.Api/Dima.Api.csproj", "Dima.Api/"]
COPY ["Dima.Web/Dima.Web.csproj", "Dima.Web/"]
COPY ["Dima.Core/Dima.Core.csproj", "Dima.Core/"]
COPY ["Dima.Tests/Dima.Tests.csproj", "Dima.Tests/"]
RUN dotnet restore

# Copia o restante e injeta as configs do WASM antes do publish
COPY . .
RUN sed -i "s|\"BackendUrl\": \".*\"|\"BackendUrl\": \"$BACKEND_URL\"|g" Dima.Web/wwwroot/appsettings.json
RUN if [ ! -z "$STRIPE_PUBLIC_KEY" ]; then \
    sed -i "s|\"StripePublicKey\": \".*\"|\"StripePublicKey\": \"$STRIPE_PUBLIC_KEY\"|g" Dima.Web/wwwroot/appsettings.json; \
    fi

# Publica o WASM e a API
RUN dotnet publish "Dima.Web/Dima.Web.csproj" -c Release -o /app/web
RUN dotnet publish "Dima.Api/Dima.Api.csproj" -c Release -o /app/api

# ===== Runtime único: a API serve o WASM (front + back num serviço só) =====
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# libgssapi-krb5-2: usada pelo Npgsql na negociação de autenticação. Sem ela o
# Npgsql só emite um aviso (a conexão funciona), mas instalar deixa os logs limpos.
RUN apt-get update \
    && apt-get install -y --no-install-recommends libgssapi-krb5-2 \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/api ./
# Embute o WASM publicado no wwwroot da API
COPY --from=build /app/web/wwwroot ./wwwroot

ENV ASPNETCORE_ENVIRONMENT=Production

# A porta real vem da variável PORT do Railway (lida no Program.cs).
EXPOSE 8080
ENTRYPOINT ["dotnet", "Dima.Api.dll"]
