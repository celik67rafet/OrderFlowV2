# 1. Aşamada: Derleme ( Build ) --dotnet sdk içeren bir ortam başlat:
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Proje dosyalarını kopyalıyoruz ( Önce bağımlılıkları çözmek için )
COPY ["OrderFlowV2.Inventory.API/OrderFlowV2.Inventory.API.csproj", "OrderFlowV2.Inventory.API/"]
COPY ["OrderFlowV2.Shared/OrderFlowV2.Shared.csproj", "OrderFlowV2.Shared/"]

# Paketleri geri yüklüyoruz:
RUN dotnet restore "OrderFlowV2.Inventory.API/OrderFlowV2.Inventory.API.csproj"

# Tüm kodları kopyalıyoruz:
COPY . .
WORKDIR "/src/OrderFlowV2.Inventory.API"

# Projeyi derliyoruz
RUN dotnet build "OrderFlowV2.Inventory.API.csproj" -c Release -o /app/build

# 2. Aşama: Yayınlama (publish)
FROM build AS publish
RUN dotnet publish "OrderFlowV2.Inventory.API.csproj" -c Release -o /app/publish

# 3. Aşama: Çalıştırma ( Runtime )
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet","OrderFlowV2.Inventory.API.dll"]
