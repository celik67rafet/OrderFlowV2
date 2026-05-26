using System.Text;
using Newtonsoft.Json;
using OrderFlowV2.ConsoleClient;

Console.ForegroundColor = ConsoleColor.Green;
string gatewayUrl = "http://localhost:5000";
HttpClient client = new HttpClient();

while (true) // Ana uygulama döngüsü
{
    Console.Clear();
    Console.WriteLine("******************************************");
    Console.WriteLine("**      ORDERFLOW V2 - TERMINAL 1984     ");
    Console.WriteLine("******************************************");


    Console.Write("\nLUTFEN KULLANICI ADINIZI GIRIN: (\"exit\" girerseniz çıkış yaparsınız...)");
    string buyerId = Console.ReadLine() ?? "anonim";

    if( buyerId == "exit" ) break;

    var basketItems = new List<dynamic>();

    List<StockDto> remoteStocks = new List<StockDto>();

    while (true) // Kullanıcı oturum döngüsü
    {
        Console.WriteLine($"\n--- HOŞGELDİN, {buyerId.ToUpper()} ---");
        Console.WriteLine("1 [LIST] - URUNLERI GOR");
        Console.WriteLine("2 [ADD]  - SEPETE EKLE");
        Console.WriteLine("3 [ORDER] - SEPETI ONAYLA (ODEME)");
        Console.WriteLine("4 [TRACK] - SIPARIŞLERIMI TAKIP ET");
        Console.WriteLine("5 [LOGOUT] - OTURUMU KAPAT");
        Console.WriteLine("6 [EXIT] - PROGRAMDAN CIK");
        Console.Write("\nSECINIZ> ");


        var choice = Console.ReadLine();

        if (choice == "1")
        {
            try
            {
                var response = await client.GetStringAsync($"{gatewayUrl}/inventory-api/api/stocks");
                remoteStocks = JsonConvert.DeserializeObject<List<StockDto>>(response);
                Console.WriteLine("\nID\t URUN\t\t\t| STOK\t FIYAT");
                foreach (var s in remoteStocks)
                {
                    Console.WriteLine($"{s.productId}\t| {s.productName.PadRight(15)}\t| {s.count}\t| {s.price} TL");
                }
            }
            catch { Console.WriteLine("!! STOK SERVİSİNE BAĞLANILAMADI !!"); }
        }
        else if (choice == "2")
        {
            Console.Write("URUN ID: ");
            int id = int.Parse(Console.ReadLine() ?? "0");

            var prod = remoteStocks.FirstOrDefault(x => x.productId == id);

            if (prod != null)
            {
                basketItems.Add(new { productId = id, productName = prod.productName, count = 1, price = prod.price });
                Console.WriteLine($">> {prod.productName} SEPETE EKLENDI.");

            }
        }
        else if (choice == "3")
        {
            if (basketItems.Count == 0) { Console.WriteLine("!! SEPET BOS !!"); continue; }

            // --- SEPET ÖZETİ EKRANI ---
            Console.WriteLine("\n================ SEPETİNİZ ================");
            decimal total = 0;
            foreach (var item in basketItems)
            {
                decimal subTotal = (decimal)item.price * (int)item.count;
                Console.WriteLine($"> {item.productName.ToString().PadRight(20)} | {item.count} Adet | Birim: {item.price} TL | Toplam: {subTotal} TL");
                total += subTotal;
            }
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine($"GENEL TOPLAM: {total} TL");
            Console.WriteLine("===========================================");

            Console.Write("\nBU SİPARİŞİ ONAYLIYOR MUSUNUZ? (E/H): ");
            if (Console.ReadLine()?.ToUpper() != "E")
            {
                Console.WriteLine(">> ISLEM IPTAL EDILDI.");
                continue;
            }

            Console.WriteLine("\nMERKEZE ILETILIYOR... LUTFEN BEKLEYIN...");
            // API'ye gönderme kısmı
            var orderDto = new { buyerId = buyerId, orderItems = basketItems };
            var content = new StringContent(JsonConvert.SerializeObject(orderDto), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{gatewayUrl}/order-api/api/Orders", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("\n>> SIPARIS ALINDI! SAGA AKISI BASLADI.");
                basketItems.Clear();
            }
            else
            {
                // HATA VARSA BURASI CALISACAK
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("\n!! HATA OLUŞTU !!");
                Console.WriteLine($"Durum Kodu: {response.StatusCode}");
                Console.WriteLine($"Mesaj: {errorContent}");
                Console.WriteLine("Devam etmek için bir tuşa basın...");
                Console.ReadKey();
            }
        }
        else if (choice == "4")
        {
            Console.WriteLine("\n--- SIPARIS GECMISINIZ ---");
            var response = await client.GetStringAsync($"{gatewayUrl}/order-api/api/Orders/user/{buyerId}");
            var orders = JsonConvert.DeserializeObject<List<dynamic>>(response);

            if (orders.Count == 0) Console.WriteLine("Henuz siparisiniz yok.");
            else
            {
                // Listeyi ekrana basarken başına numara koyuyoruz
                int counter = 1;
                foreach (var o in orders)
                {
                    Console.WriteLine($"{counter}. Sipariş: {o.id.ToString().Substring(0, 8)}... | Tutar: {o.totalPrice} TL | Durum: {o.status}");
                    counter++;
                }

                // ID yerine sıra numarası soruyoruz
                Console.Write("\nDETAYINI GORMEK ISTEDIGINIZ SIRA NO (VEYA ANA MENU ICIN 0)> ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int orderIndex) && orderIndex > 0 && orderIndex <= orders.Count)
                {
                    // Kullanıcının seçtiği numaradan gerçek (uzun) ID'yi buluyoruz
                    var selectedOrderId = orders[orderIndex - 1].id;

                    try
                    {
                        Console.WriteLine($"\nID: {selectedOrderId} DETAYLARI CEKILIYOR...");
                        var detailResp = await client.GetStringAsync($"{gatewayUrl}/order-api/api/Orders/{selectedOrderId}");
                        var d = JsonConvert.DeserializeObject<dynamic>(detailResp);

                        Console.WriteLine("\n=================================================");
                        Console.WriteLine("                SİPARİŞ DETAY FİŞİ                 ");
                        Console.WriteLine("===================================================");
                        Console.WriteLine($"SIPARIS NO: {selectedOrderId}");
                        Console.WriteLine($"DURUM      : {d.status}");
                        Console.WriteLine($"TARIH      : {d.createdDate}");
                        Console.WriteLine("-------------------------------------------------");
                        Console.WriteLine("URUN LISTESI:");

                        foreach (var item in d.items)
                        {
                            Console.WriteLine($"> Urun ID: {item.productId} | Adet: {item.count} | Fiyat: {item.price} TL");
                        }

                        Console.WriteLine("-------------------------------------------");
                        Console.WriteLine($"GENEL TOPLAM: {d.totalPrice} TL");
                        Console.WriteLine("===================================================");
                        Console.WriteLine("Ana menuye donmek icin bir tusa basin...");
                        Console.ReadKey();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("!! DETAY CEKILEMEDI: " + ex.Message);
                    }
                }
            }
        }
        else if (choice == "5") break; // İç döngüden çıkar, tekrar isim sorar
        else if (choice == "6") Environment.Exit(0);
    }
}