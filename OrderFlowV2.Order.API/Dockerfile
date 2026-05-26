# 1. Aşama: Derleme ( Build ) --dotnet sdk içeren bir ortam başlat
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Proje dosyalarını kopyalıyoruz ( Önce bağımlılıkları çözmek için )
COPY ["OrderFlowV2.Order.API/OrderFlowV2.Order.API.csproj", "OrderFlowV2.Order.API/"]
COPY ["OrderFlowV2.Shared/OrderFlowV2.Shared.csproj", "OrderFlowV2.Shared/"]

# Paketleri geri yüklüyoruz:
RUN dotnet restore "OrderFlowV2.Order.API/OrderFlowV2.Order.API.csproj"

# Tüm kodları kopyalıyoruz:
COPY . .
WORKDIR "/src/OrderFlowV2.Order.API"

# Projeyi derliyoruz
RUN dotnet build "OrderFlowV2.Order.API.csproj" -c Release -o /app/build

# 2. Aşama: Yayınlama (Publish)
FROM build AS publish
RUN dotnet publish "OrderFlowV2.Order.API.csproj" -c Release -o /app/publish

# 3. Aşama: Çalıştırma ( Runtime )
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderFlowV2.Order.API.dll"]

# İlk aşamada:

# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# WORKDIR /src

# Bu satırlarda imaj seçti dotnet 8 bir ortam olacak,

# İkinci aşamada:

# COPY ["OrderFlowV2.Order.API/OrderFlowV2.Order.API.csproj", "OrderFlowV2.Order.API/"]
# COPY ["OrderFlowV2.Shared/OrderFlowV2.Shared.csproj","OrderFlowV2.Shared/"]

# Buralarda ise csprojları src altında "OrderFlowV2.Order.API" ve "OrderFlowV2.Shared" adlarında klasörler yoksa açar
# varsa içine csprojları atar...

# Üçüncü aşamada:
# RUN dotnet restore "OrderFlowV2.Order.API/OrderFlowV2.Order.API.csproj"
# OrderFlowV2.Order.API.csproj ile bağımlılıkları restore eder yani yükler.

# Dördüncü aşamada:
# COPY . . 
# WORKDIR "/src/OrderFlowV2.Order.API"
# Burada ise copy . . diyerek hosttaki tüm dosyaları alıp /src dizinine kopyalıyoruz,
# Çalışma dizini artık OrderFlowV2.Order.API
# 

# Beşinci aşamada:
# RUN dotnet build "OrderFlowV2.Order.API.csproj" -c Release -o /app/build
# burada csproj ile projeyi derliyoruz çünkü artık hosttan dosyaları aldık...

# Altıncı aşamada:
# FROM build AS publish
# Burada build ortamını al ve publish diye bir ortam oluştur diyoruz.
# RUN dotnet publish "OrderFlowV2.Order.API.csproj" -c Release -o /app/publish
# Burada da publish ortamı için csproj üzerinden publish alıp buraya veriyoruz. Yani build üzerinden publish
# almış oluyoruz...

# Yedinci aşamada:
# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
# WORKDIR /app
# Burada da çalıştırmaya uygun hafif bir .net runtime ortam hazırlar adı da final olur.
# ardından çalışma klasörü de app dir.

# Sekizinci aşamada:
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "OrderFlowV2.Order.API.dll"]
# Burada da publish ortamındaki app/publish içindeki dosyaları alıp final ortamda app dizini içine kopyalar.
# Dolayısıyla entrypoint yani giriş noktamız artık runtime da ki dll.


