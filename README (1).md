# YasmahCraft — Handmade Crochet E-Commerce Store

A full-stack e-commerce web application built for a real crochet fashion brand. YasmahCraft allows customers to browse handcrafted crochet pieces, add items to cart, place orders, and pay securely online. The store owner manages products, categories, and orders through a custom admin dashboard.

**Live Site:** [yasmahcraft-production.up.railway.app](https://yasmahcraft-production.up.railway.app)

---

## Screenshots

### Homepage — Hero Slideshow
![Homepage Hero](screenshots/Proof_2.png)

### Shop Page — Product Grid
![Shop Page](screenshots/Proof_1.png)

### Product Details — Add to Cart
![Product Details](screenshots/Proof_3.png)

### Admin Dashboard
![Admin Dashboard](screenshots/Proof_4.png)

### Checkout — Paystack Payment
![Paystack Checkout](screenshots/Proof_5.png)

---

## Features

- **Product Catalogue** — Browse bags and clothing with images, sizes, colours, and stock status
- **Add to Cart** — Session-based cart supporting multiple products before checkout
- **Buy Now** — Direct single-product checkout flow
- **Custom Order Page** — Customers describe their dream piece, form pre-fills a WhatsApp message to the store owner
- **Secure Payments** — Integrated with Paystack (NGN) with payment verification
- **User Authentication** — Register, login, logout via ASP.NET Identity
- **Admin Dashboard** — Add/edit/delete products and categories, view and manage orders
- **Mobile Responsive** — Fully responsive across all screen sizes with hamburger nav
- **Order Confirmation** — Customers receive order details after successful payment

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Backend | ASP.NET Core MVC, C# |
| ORM | Entity Framework Core |
| Database | PostgreSQL |
| Payments | Paystack |
| Auth | ASP.NET Core Identity |
| Frontend | Razor Views, Bootstrap 5, Bootstrap Icons |
| Fonts | Cormorant Garamond, Jost (Google Fonts) |
| Hosting | Railway |
| Image Hosting | ImgBB |

---

## Project Structure

```
YasmahCraft/
├── Controllers/
│   ├── HomeController.cs
│   ├── ShopController.cs
│   ├── CartController.cs
│   ├── OrderController.cs
│   └── AdminController.cs
├── Models/
│   ├── Entities/          # Product, Category, Order, OrderItem
│   └── CartItem.cs
├── Services/
│   ├── IProductService / ProductService
│   ├── IOrderService / OrderService
│   └── ICartService / CartService
├── ViewModels/
│   └── PlaceOrderViewModel.cs
├── Data/
│   ├── ApplicationDbContext.cs
│   └── Migrations/
└── Views/
    ├── Home/              # Index, About, Contact, CustomOrder
    ├── Shop/              # Index, Details
    ├── Cart/              # Index
    └── Order/             # Checkout, PaymentPage, OrderConfirmation
```

---

## Getting Started (Local Development)

### Prerequisites
- .NET 8 SDK
- SQL Server or PostgreSQL
- Paystack account (for payment keys)

### Setup

1. Clone the repository:
```bash
git clone https://github.com/hassanahmad444/YasmahCraft.git
cd YasmahCraft
```

2. Update `appsettings.json` with your connection string:
```json
"ConnectionStrings": {
  "DefaultConnection": "Your connection string here"
}
```

3. Add your Paystack keys to `appsettings.json`:
```json
"Paystack": {
  "PublicKey": "pk_test_xxxx",
  "SecretKey": "sk_test_xxxx"
}
```

4. Run migrations:
```bash
dotnet ef database update
```

5. Run the application:
```bash
dotnet run
```

### Default Admin Account
- **Email:** admin@yasmahcraft.com
- **Password:** Admin@123

---

## Deployment

The application is deployed on **Railway** with a managed **PostgreSQL** database. Migrations run automatically on startup.

Environment variables set on Railway:
- `ConnectionStrings__DefaultConnection` — PostgreSQL connection string
- `ASPNETCORE_ENVIRONMENT` — `Production`
- `Paystack__PublicKey` — Paystack public key
- `Paystack__SecretKey` — Paystack secret key

---

## Built By

**Hassan Ahmad** — Junior .NET Backend Developer  
GitHub: [@hassanahmad444](https://github.com/hassanahmad444)
