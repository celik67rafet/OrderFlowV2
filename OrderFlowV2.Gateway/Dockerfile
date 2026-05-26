# 1. Aşama: Derleme
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Gateway'de shared projesi olmadığı için sadece kendi dosyasını kopyalıyoruz:
COPY ["OrderFlowV2.Gateway/OrderFlowV2.Gateway.csproj", "OrderFlowV2.Gateway/"]

# Paketleri geri yüklüyoruz:
RUN dotnet restore "OrderFlowV2.Gateway/OrderFlowV2.Gateway.csproj"

# Tüm kodları kopyalıyoruz:
COPY . .
WORKDIR "/src/OrderFlowV2.Gateway"

# Projeyi derliyoruz:
RUN dotnet build "OrderFlowV2.Gateway.csproj" -c Release -o /app/build

# 2. Aşama: Yayınlama:
FROM build AS publish
RUN dotnet publish "OrderFlowV2.Gateway.csproj" -c Release -o /app/publish

# 3. Aşama: Çalıştırma ( Runtime )
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=publish /app/publish . 
ENTRYPOINT ["dotnet", "OrderFlowV2.Gateway.dll"]

