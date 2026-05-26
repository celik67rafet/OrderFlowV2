# 🛒 OrderFlow v2 – Event-Driven Mikroservis Tabanlı Sipariş Sistemi

**OrderFlow v2**, modern mikroservis mimarilerini, servisler arası **asenkron iletişimi** ve dağıtık sistemlerde veri tutarlılığı (**Eventual Consistency**) yaklaşımını uygulamalı olarak modellemek amacıyla geliştirilmiş bir backend projesidir.

Sistem, gerçek dünyadaki e-ticaret sipariş süreçlerini **Saga Pattern (Choreography)** ve **Event-Driven Architecture** kullanarak simüle eder.

---

## 🚀 Mimari Yaklaşım

Bu projede servisler birbirleriyle doğrudan iletişim kurmaz. Tüm haberleşme **RabbitMQ üzerinden yayınlanan event'ler** aracılığıyla gerçekleştirilir.

Sistem aşağıdaki prensipler üzerine tasarlanmıştır:

- **Event-Driven Architecture**
- **Saga Pattern (Choreography)**
- **Eventual Consistency**
- **Database Per Service Pattern**
- **Loose Coupling Between Services**
- **Asynchronous Communication**

---

## 🛠️ Kullanılan Teknolojiler

| Kategori | Teknoloji |
|----------|------------|
| Framework | .NET 8 / C# |
| ORM | Dapper |
| Message Broker | RabbitMQ + MassTransit |
| Database | MySQL / MariaDB |
| Caching | Redis |
| API Gateway | YARP (Reverse Proxy) |
| Containerization | Docker & Docker Compose |
| Client | Console Application |

---

## 📦 Sistem Bileşenleri

### 1. Order Service
Sipariş oluşturma ve sipariş yaşam döngüsünü yönetir.

**Sorumlulukları**
- Sipariş oluşturma
- Sipariş durumu takibi
- Saga sürecini başlatma

---

### 2. Inventory Service
Ürün stoklarını kontrol eder ve rezervasyon işlemlerini yürütür.

**Sorumlulukları**
- Stok doğrulama
- Stok rezervasyonu
- Başarısız siparişlerde stok geri bırakma

---

### 3. Payment Service
Ödeme sürecini simüle eder.

**Sorumlulukları**
- Ödeme işlemleri
- Başarılı / başarısız ödeme event’leri üretme

---

### 4. Shipping Service
Siparişin kargo sürecini yönetir.

**Sorumlulukları**
- Kargo süreci başlatma
- Sipariş gönderim event’i üretme

---

### 5. Basket Service
Kullanıcı sepet verilerini **Redis** üzerinde yönetir.

**Sorumlulukları**
- Sepet yönetimi
- Geçici veri saklama
- Performans optimizasyonu

---

### 6. Notification Service
Sistem genelindeki tüm event’leri dinleyerek bildirim üretir.

**Sorumlulukları**
- Merkezi event dinleme
- Sistem log/bildirim üretimi

---

### 7. Gateway
Sistemin merkezi giriş noktasıdır.

**Sorumlulukları**
- Request forwarding
- Reverse proxy yönetimi
- Mikroservis yönlendirmesi

---

## 🔄 Sipariş Akışı (Saga Flow)

Sistem, **Choreography-based Saga Pattern** kullanır.

### Başarılı Sipariş Akışı

```text
OrderCreated
      │
      ▼
StockReserved
      │
      ▼
PaymentCompleted
      │
      ▼
OrderShipped
```

### Hata Durumu (Compensating Transactions)

Ödeme başarısız olduğunda sistem otomatik rollback uygular.

```text
PaymentFailed
      │
      ▼
OrderCancelled
      │
      ▼
StockReleased
```

Bu yapı sayesinde sistem, dağıtık servisler arasında **eventual consistency** sağlayarak veri tutarlılığını korur.

---

## 🏗️ Sistem Mimarisi

```text
                Client (Console App)
                         │
                         ▼
                    API Gateway
                         │
    ┌────────────────────┼────────────────────┐
    ▼                    ▼                    ▼
Order Service      Basket Service      Notification Service
    │                    │
    ▼                    ▼
RabbitMQ <──────────── Event Bus ────────────►
    │
    ├────────► Inventory Service
    │
    ├────────► Payment Service
    │
    └────────► Shipping Service
```

---

## 🛠️ Kurulum ve Çalıştırma

### 1. Projeyi Klonlayın

```bash
git clone <repository-url>
cd OrderFlowV2
```

---

### 2. Docker Servislerini Başlatın

```bash
docker-compose up -d
```

Bu işlem aşağıdaki servisleri ayağa kaldırır:

- RabbitMQ
- MySQL / MariaDB
- Redis

---

### 3. Veritabanlarını Hazırlayın

MySQL Workbench üzerinden ilgili portlara bağlanarak tabloları oluşturun.

Örnek bağlantı portları:

- `3307`
- `3308`

---

### 4. Uygulamayı Çalıştırın

Visual Studio üzerinden **ConsoleClient** projesini çalıştırın.

Terminal arayüzü üzerinden sipariş oluşturabilir ve Saga akışını gözlemleyebilirsiniz.

---

## 🎯 Projenin Amacı

Bu proje aşağıdaki backend ve distributed systems konseptlerini pratiğe dökmeyi amaçlamaktadır:

- Mikroservis Mimarisi
- Event-Driven Communication
- Saga Pattern
- Eventual Consistency
- Distributed Transactions
- Redis Caching
- API Gateway Pattern
- Loose Coupling
- Asynchronous Messaging

---

## 🚧 Yol Haritası

Planlanan geliştirmeler:

- [ ] JWT Authentication
- [ ] Distributed Logging
- [ ] Observability (OpenTelemetry)
- [ ] Circuit Breaker Pattern
- [ ] Kubernetes Deployment
- [ ] CI/CD Pipeline
- [ ] Integration Testing

---

## 📄 Lisans

Bu proje eğitim ve portföy amaçlı geliştirilmektedir.