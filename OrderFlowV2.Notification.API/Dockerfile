# 1. Aşama: Derleme ( Build ) --dotnet sdk içeren bir ortam başlat
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Proje dosyalarını kopyalıyoruz ( Önce bağımlılıkları çözmek için )
COPY ["OrderFlowV2.Notification.API/OrderFlowV2.Notification.API.csproj", "OrderFlowV2.Notification.API/"]
COPY ["OrderFlowV2.Shared/OrderFlowV2.Shared.csproj", "OrderFlowV2.Shared/"]

# Paketleri geri yüklüyoruz:
RUN dotnet restore "OrderFlowV2.Notification.API/OrderFlowV2.Notification.API.csproj"

# Tüm kodları kopyalıyoruz:
COPY . .
WORKDIR "/src/OrderFlowV2.Notification.API"

# Projeyi derliyoruz:
RUN dotnet build "OrderFlowV2.Notification.API.csproj" -c Release -o /app/build

# 2. Aşama: Yayınlama ( Publish )
FROM build AS publish
RUN dotnet publish "OrderFlowV2.Notification.API.csproj" -c Release -o /app/publish

# 3. Aşama: Çalıştırma ( Runtime )
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderFlowV2.Notification.API.dll"]