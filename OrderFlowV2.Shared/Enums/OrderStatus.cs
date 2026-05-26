using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlowV2.Shared.Enums
{

    // Enum'lar integer olarak değer alır aslında, mesela DB'ye bu değerler 0,1,2,3,4,5,6 ... [ Id, Name ] olarak gider. Ama proje tarafında integer değere karşılık gelen name değeri.
    public enum OrderStatus
    {
        Pending, // İlk oluşturulduğunda ( Beklemede )
        StockReserved, // Stoklar ayrıldığında 
        StockNotAvailable, // Stok yetersizse
        Paid, // Ödeme başarıyla yapıldığında
        PaymentFailed, // Ödeme başarısızsa
        Preparing, // Hazırlanıyor
        Shipped, // Kargoya verildi
        Delivered, // Teslim edildi
        Cancelled // İptal edildi
    }
}
