<div align="center">

<img src="LuxNest.Web/wwwroot/images/villa_logo.png" alt="LuxNest Logo" width="120"/>

<h1>LuxNest</h1>

<h3><em>Luxury Villa Booking Platform</em></h3>

<p>A full-stack ASP.NET Core 8.0 MVC application for discovering, booking, and managing luxury villa accommodations — with Stripe payments, smart refund logic, and a real-time admin analytics dashboard.</p>

[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12-239120?logo=csharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![EF Core](https://img.shields.io/badge/EF%20Core-8.0-512BD4?logo=dotnet&logoColor=white)](https://docs.microsoft.com/en-us/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-LocalDB-CC2927?logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/en-us/sql-server)
[![Stripe](https://img.shields.io/badge/Stripe-test%20mode-635BFF?logo=stripe&logoColor=white)](https://stripe.com/)
[![Bootstrap](https://img.shields.io/badge/Bootstrap-5-7952B3?logo=bootstrap&logoColor=white)](https://getbootstrap.com/)

</div>

---

> [!NOTE]
> **Portfolio / Demo Project** — Stripe operates in test mode (no real charges are processed), the database uses SQL Server LocalDB, and credentials are stored in `appsettings.json` for convenience. This project is not intended for production deployment as-is.

---

## Table of Contents

- [📸 Screenshots](#-screenshots)
- [🌴 Overview](#-overview)
- [🏛️ Architecture](#️-architecture)
- [🛠️ Tech Stack](#️-tech-stack)
- [✨ Features](#-features)
- [🗺️ Booking Flow](#️-booking-flow)
- [📅 Availability System](#-availability-system)
- [💳 Stripe Integration](#-stripe-integration)
- [💸 Refund Policy](#-refund-policy)
- [📊 Dashboard & Analytics](#-dashboard--analytics)
- [🧾 Invoice Generation](#-invoice-generation)
- [🛡️ Role-Based Authorization](#️-role-based-authorization)
- [📂 Project Structure](#-project-structure)
- [🗄️ Data Model](#️-data-model)
- [⚠️ Demo Limitations](#️-demo-limitations)
- [🚀 Running the Project](#-running-the-project)
- [⚙️ Configuration](#️-configuration)
- [👩‍💻 Author](#-author)

---

## 📸 Screenshots

### Public Experience

<table>
  <tr>
    <td align="center"><img src="screenshots/landing_guests1.png" alt="Landing Page" width="100%"/><br/><sub><b>Landing Page — Hero Search</b></sub></td>
    <td align="center"><img src="screenshots/public_villas.png" alt="Villa Listing" width="100%"/><br/><sub><b>Villa Listing — Public View</b></sub></td>
  </tr>
  <tr>
    <td align="center"><img src="screenshots/villa_details.png" alt="Villa Details" width="100%"/><br/><sub><b>Villa Details Modal</b></sub></td>
    <td align="center"><img src="screenshots/register_user.png" alt="Register" width="100%"/><br/><sub><b>Customer Registration</b></sub></td>
  </tr>
</table>

### Availability & Room Search

<table>
  <tr>
    <td align="center"><img src="screenshots/check_availability.png" alt="Check Availability" width="100%"/><br/><sub><b>Availability Search Form</b></sub></td>
    <td align="center"><img src="screenshots/availability_search_section.png" alt="Search Section" width="100%"/><br/><sub><b>Date & Room Filters</b></sub></td>
  </tr>
  <tr>
    <td align="center"><img src="screenshots/room_configuration_results.png" alt="Room Config Results" width="100%"/><br/><sub><b>Available Rooms by Type</b></sub></td>
    <td align="center"><img src="screenshots/villa_availability_results1.png" alt="Villa Availability Results" width="100%"/><br/><sub><b>Villa Availability Results</b></sub></td>
  </tr>
  <tr>
    <td align="center"><img src="screenshots/no_free_2person_rooms_for_royal_villa.png" alt="No 2-person rooms" width="100%"/><br/><sub><b>No Available 2-Person Rooms</b></sub></td>
    <td align="center"><img src="screenshots/not_enough_capacity.png" alt="Not enough capacity" width="100%"/><br/><sub><b>Insufficient Capacity Warning</b></sub></td>
  </tr>
  <tr>
    <td align="center"><img src="screenshots/invalid_room_configuration.png" alt="Invalid config" width="100%"/><br/><sub><b>Invalid Room Configuration</b></sub></td>
    <td align="center"><img src="screenshots/checkavailability_free_room_same_interval_days.png" alt="Free rooms same interval" width="100%"/><br/><sub><b>Overlapping Interval Check</b></sub></td>
  </tr>
</table>

### Booking Flow

<table>
  <tr>
    <td align="center"><img src="screenshots/booking_checkout_page.png" alt="Booking Checkout" width="100%"/><br/><sub><b>Booking Summary & Checkout</b></sub></td>
    <td align="center"><img src="screenshots/stripe_payment.png" alt="Stripe Payment" width="100%"/><br/><sub><b>Stripe Checkout — Secure Payment</b></sub></td>
  </tr>
  <tr>
    <td align="center"><img src="screenshots/booking_confirmation_page.png" alt="Booking Confirmation" width="100%"/><br/><sub><b>Booking Confirmation Page</b></sub></td>
    <td align="center"><img src="screenshots/confirmation_booking.png" alt="Confirmation" width="100%"/><br/><sub><b>Approved Booking Details</b></sub></td>
  </tr>
  <tr>
    <td align="center"><img src="screenshots/checkin_day_current_day_select_room.png" alt="Check-in Room" width="100%"/><br/><sub><b>Admin — Check-In & Room Assignment</b></sub></td>
    <td align="center"><img src="screenshots/check_in_completed.png" alt="Check-In Completed" width="100%"/><br/><sub><b>Check-In Completed</b></sub></td>
  </tr>
</table>

### Cancellation & Refunds

<table>
  <tr>
    <td align="center"><img src="screenshots/cancel_booking_before_7days.png" alt="Cancel before 7 days" width="100%"/><br/><sub><b>100% Refund Eligible</b></sub></td>
    <td align="center"><img src="screenshots/50percent_refund_applies.png" alt="50% Refund" width="100%"/><br/><sub><b>50% Refund Applied (3–7 Days)</b></sub></td>
  </tr>
  <tr>
    <td align="center"><img src="screenshots/no_refund_button_less_than_3days.png" alt="No Refund Button" width="100%"/><br/><sub><b>No Refund — Less Than 3 Days</b></sub></td>
    <td align="center"><img src="screenshots/no_refund_less_than_3days.png" alt="No Refund" width="100%"/><br/><sub><b>No Refund Confirmation</b></sub></td>
  </tr>
  <tr>
    <td align="center"><img src="screenshots/refunded_applies_50percent.png" alt="Refunded status" width="100%"/><br/><sub><b>Refunded — Stripe Processed</b></sub></td>
    <td align="center"><img src="screenshots/cancel_booking.png" alt="Cancel modal" width="100%"/><br/><sub><b>Cancel Booking — Confirmation Modal</b></sub></td>
  </tr>
</table>

### Admin Dashboard & Management

<table>
  <tr>
    <td align="center"><img src="screenshots/dashboard_admin.png" alt="Dashboard" width="100%"/><br/><sub><b>Admin Dashboard — Full Analytics View</b></sub></td>
    <td align="center"><img src="screenshots/booking_list_filtered_by_status.png" alt="Booking List" width="100%"/><br/><sub><b>Booking List — Filtered by Status</b></sub></td>
  </tr>
  <tr>
    <td align="center"><img src="screenshots/villa_list.png" alt="Villa List" width="100%"/><br/><sub><b>Villa Management</b></sub></td>
    <td align="center"><img src="screenshots/villa_number_list.png" alt="Villa Numbers" width="100%"/><br/><sub><b>Room Number Management</b></sub></td>
  </tr>
  <tr>
    <td align="center"><img src="screenshots/amenity_list.png" alt="Amenity List" width="100%"/><br/><sub><b>Amenities Management</b></sub></td>
    <td align="center"><img src="screenshots/booking_one_villa_doesnt_have_3persone_rooms.png" alt="No 3-person rooms" width="100%"/><br/><sub><b>Villa Without 3-Person Rooms — Disabled</b></sub></td>
  </tr>
</table>

### Invoice Generation

<table>
  <tr>
    <td align="center"><img src="screenshots/generating_invoice_pdf.png" alt="Generating Invoice" width="100%"/><br/><sub><b>Generating Invoice — Word & PDF Options</b></sub></td>
    <td align="center"><img src="screenshots/invoice_pdf.png" alt="Invoice PDF" width="100%"/><br/><sub><b>Generated PDF Invoice</b></sub></td>
  </tr>
</table>

---

## 🌴 Overview

LuxNest is a full-stack **luxury villa booking platform** built with ASP.NET Core 8.0 MVC. It simulates a real-world hospitality management system where guests can search for available villas by date, select room types (2-person or 3-person), complete a secure payment via Stripe, and manage their bookings — while administrators oversee the entire operation through a dedicated dashboard with analytics, check-in/check-out workflows, and automated invoice generation.

The project goes significantly beyond a basic CRUD application:

- **Per-room availability** — each villa contains multiple individually managed rooms; availability is tracked at the room level, not the villa level
- **Room type differentiation** — 2-person and 3-person rooms are tracked separately, and the search engine validates requested room types against actual availability
- **Smart refund engine** — cancellations trigger automatic Stripe Refund API calls with the percentage calculated based on days remaining until check-in
- **Auto room assignment** — a specific room number is assigned at payment confirmation, before physical check-in
- **Real-time analytics** — the admin dashboard computes month-over-month metrics, booking status distribution, and daily trends from live database queries

---

## 🏛️ Architecture

LuxNest follows a **layered architecture inspired by Clean Architecture principles**, organized across four separate C# projects. While the separation of concerns and dependency direction closely follow Clean Architecture conventions, some pragmatic decisions (such as the Application layer referencing Web-layer ViewModels) mean it is best described as Clean Architecture-inspired rather than a strict implementation.

```
┌─────────────────────────────────────────────────┐
│                  LuxNest.Web                    │  ← Presentation Layer
│     Controllers · Views · ViewModels · wwwroot  │
└────────────────────┬────────────────────────────┘
                     │ depends on
┌────────────────────▼────────────────────────────┐
│              LuxNest.Application                │  ← Application Layer
│   Service Interfaces · Implementations · DTOs  │
│              SD Constants · Utility             │
└─────────────┬──────────────────┬────────────────┘
              │                  │
   depends on │                  │ depends on
              │                  │
┌─────────────▼──────┐  ┌────────▼───────────────┐
│   LuxNest.Domain   │  │ LuxNest.Infrastructure  │  ← Data Layer
│  Entities · Enums  │  │  EF Core · Repositories │
│                    │  │  UnitOfWork · Migrations│
└────────────────────┘  └─────────────────────────┘
```

**Key architectural patterns:**

| Pattern | Implementation |
| --- | --- |
| Repository Pattern | Generic `IRepository<T>` with LINQ-based filtering and eager loading |
| Unit of Work | `IUnitOfWork` coordinates all repositories under a single `SaveChanges()` call |
| Service Layer | All business logic encapsulated in `Application/Services/Implementation/` |
| Dependency Injection | All services and repositories registered in `Program.cs` |
| Background Service | `BookingAutoCompleteService` (`IHostedService`) auto-completes overdue bookings |

---

## 🛠️ Tech Stack

| Category | Technology |
| --- | --- |
| Language | C# 12 |
| Framework | ASP.NET Core 8.0 MVC |
| ORM | Entity Framework Core 8.0 |
| Database | SQL Server (LocalDB for development) |
| Authentication | ASP.NET Core Identity with role-based access control |
| Payments | Stripe.net — Checkout Sessions + Refund API |
| Documents | Syncfusion DocIO (Word `.docx`) + Syncfusion PDF Renderer |
| Charts | ApexCharts (RadialBar, Pie, Line) |
| Frontend | Bootstrap 5, Bootstrap Icons, jQuery, DataTables.net |

---

## ✨ Features

**Customer**

- Availability search by check-in date, nights, number of rooms, total guests, and room types
- Room type selection (2-person vs 3-person) validated against per-type availability
- Live capacity feedback based on the selected room configuration
- Villa details modal with amenities, room configuration badges, and pricing
- Stripe Checkout with redirect flow and server-side payment confirmation
- My Bookings with color-coded status badges, guest count, and total cost
- Smart cancellation with refund eligibility shown before confirmation

**Admin**

- Analytics dashboard with month-over-month KPI cards, booking status pie chart, and daily trend lines
- Booking list with filter tabs: All / Approved / CheckedIn / Completed / Cancelled / Refunded
- Check-in workflow — confirm guest arrival, view pre-assigned room, record actual date
- Check-out workflow — mark booking as Completed with actual check-out date
- Invoice generation in Word (.docx) or PDF format per booking
- Villa CRUD, room number management, and amenity management

---

## 🗺️ Booking Flow

```
1. SEARCH
   Customer enters: check-in date · nights · rooms · guests · room types
         ↓
2. AVAILABILITY CHECK
   System computes available rooms per villa and per type
   Villas without enough rooms of the requested type → shown as disabled
         ↓
3. BOOKING SUMMARY  (FinalizeBooking GET)
   Villa details · selected rooms · price breakdown · total cost
   Price = Σ(room base price + capacity surcharge) × nights
         ↓
4. STRIPE CHECKOUT  (FinalizeBooking POST)
   Booking saved with Status: Pending
   Customer redirected to Stripe-hosted Checkout page
         ↓
5. PAYMENT CONFIRMATION  (BookingConfirmation GET)
   Server calls SessionService.Get(stripeSessionId)
   PaymentStatus == "paid" → Status: Approved
   First available room auto-assigned → VillaNumber set immediately
         ↓
6. CHECK-IN  (Admin)
   Guest arrives physically → Admin clicks Check In
   Status: CheckedIn · ActualCheckInDate recorded
         ↓
7. CHECK-OUT  (Admin)
   Guest departs → Admin clicks Check Out
   Status: Completed · ActualCheckOutDate recorded
```

**Cancellation flow:**

```
Customer cancels → System calculates daysUntilCheckIn = CheckInDate - Today

  > 7 days  → 100% refund → Stripe RefundService.Create() → Status: Refunded
  3–7 days  →  50% refund → Stripe RefundService.Create() → Status: Refunded
  < 3 days  →   no refund →                                  Status: Cancelled
```

---

## 📅 Availability System

Rather than treating a villa as a single bookable unit, LuxNest tracks **individual rooms** and their capacity types separately.

**Overlap detection:**

```csharp
int bookingInDate = bookedVillas
    .Where(x => x.VillaId == villa.Id
        && x.CheckInDate < checkInDate.AddDays(nights)
        && checkInDate < x.CheckOutDate)
    .Count();

int roomAvailable = totalRooms - bookingInDate;
```

Each active booking consumes exactly one room slot. A villa with 4 rooms can serve 4 simultaneous bookings in the same date range.

**Per-type availability:**

```csharp
villa.Available2p = Math.Max(0, villa.Rooms2p - taken2p);
villa.Available3p = Math.Max(0, villa.Rooms3p - taken3p);
```

To determine which rooms are taken, the engine inspects assigned `VillaNumber` values and their capacities. A villa can appear in four distinct states:

| State | Condition |
| --- | --- |
| Available to book | Enough rooms of all requested types |
| Not enough 2-person rooms | `Available2p < requested2p` |
| Not enough 3-person rooms | `Available3p < requested3p` |
| Sold out | `roomAvailable == 0` |

Rooms are blocked by bookings with statuses **Pending**, **Approved**, and **CheckedIn**. Cancelled and Refunded bookings release their room slot immediately.

When a booking transitions Pending → Approved (payment confirmed), a room is automatically assigned by selecting the first room number not already allocated to any Approved or CheckedIn booking.

---

## 💳 Stripe Integration

LuxNest uses Stripe Checkout Sessions — the industry-standard hosted payment page that handles PCI compliance, card validation, and 3D Secure automatically.

```
FinalizeBooking (POST)
  → Booking saved with Status: Pending
  → SessionCreateOptions built (villa name, total amount in cents)
  → HTTP 303 redirect to Stripe-hosted checkout

BookingConfirmation (GET) — after Stripe redirect back
  → SessionService.Get(bookingFromDb.StripeSessionId)
  → PaymentStatus == "paid" → UpdateStatus(Approved, assignedRoomNumber)
  → UpdateStripePaymentID(session.Id, session.PaymentIntentId)

CancelBookingUser — refund path
  → refundAmount = TotalCost × refundPercent
  → RefundCreateOptions { PaymentIntent = StripePaymentIntentId, Amount in cents }
  → RefundService.Create(refundOptions)
  → Stripe processes refund to original payment method (5–10 business days)
```

> [!NOTE]
> Test card: `4242 4242 4242 4242` — any future expiry, any CVC.

---

## 💸 Refund Policy

A tiered, time-based refund policy enforced **server-side** via the Stripe Refund API. The refund amount is calculated on the server — customers cannot manipulate it client-side.

| Days Until Check-In | Refund | Status After |
| --- | --- | --- |
| More than 7 days | 100% | Refunded |
| 3 to 7 days | 50% | Refunded |
| Less than 3 days | No refund | Cancelled |
| Check-in day or past | No refund | Cancelled |

The applicable percentage and exact dollar amount are shown to the customer before they confirm the cancellation — both on the Booking Details page and as an inline note in My Bookings.

---

## 📊 Dashboard & Analytics

Real-time operational metrics computed from live database queries on every page load — no caching, no pre-aggregated tables.

**Radial bar KPI cards**

Each card displays the all-time total, the current-month count with a directional indicator vs. the previous month, and a radial percentage representing the month-over-month ratio.

| KPI | Included Statuses |
| --- | --- |
| Total Bookings | Approved + CheckedIn + Completed |
| Total Users | All registrations with the Customer role |
| Total Revenue | Sum of `TotalCost` for Approved + CheckedIn + Completed |

**Booking status pie chart**

Proportional breakdown of bookings from the last 30 days across all statuses: Approved, CheckedIn, Completed, Cancelled, Refunded.

**Daily trend line charts**

Two ApexCharts line charts for the current calendar month — new bookings per day and new customer registrations per day. Both use a fixed date range with zero-fill for days with no activity, producing a clean continuous line.

---

## 🧾 Invoice Generation

Administrators can generate a professional booking invoice in two formats directly from the Booking Details page:

- **Word (.docx)** — generated via Syncfusion DocIO by loading a `.docx` template and replacing placeholder tokens (e.g. `xx_customer_name`, `xx_villa_name`) with live booking data
- **PDF** — converted from the populated Word document using Syncfusion's PDF renderer

The invoice includes: Booking ID, booking date, customer name, phone, email, villa name, number of nights, price per night, check-in/check-out dates, and booking total.

> [!NOTE]
> Invoice generation uses a Syncfusion trial license — a watermark is visible on generated documents. Register for a free community license at [syncfusion.com](https://www.syncfusion.com/products/communitylicense) and replace the key in `Program.cs` to remove it.

---

## 🛡️ Role-Based Authorization

ASP.NET Core Identity with two roles seeded at startup:

| Role | Access Scope |
| --- | --- |
| `Admin` | Dashboard, full booking management, check-in/out workflow, invoice generation, all CRUD |
| `Customer` | Landing page, villa search, booking flow, My Bookings, cancellations |

Authorization is enforced at the controller and action level using `[Authorize(Roles = SD.Role_Admin)]` and `[Authorize]` attributes. The admin sidebar renders conditionally via `User.IsInRole()` in the shared layout. Customers attempting to access admin-only actions receive a **403 Forbidden** response.

---

## 📂 Project Structure

```
LuxNest/
├── LuxNest.Domain/
│   └── Entities/
│       ├── Villa.cs                  # Core villa entity + [NotMapped] computed properties
│       ├── VillaNumber.cs            # Individual room with capacity (2 or 3 persons)
│       ├── Booking.cs                # Full booking lifecycle entity
│       ├── Amenity.cs
│       └── ApplicationUser.cs        # Extended IdentityUser (Name, CreatedAt)
│
├── LuxNest.Application/
│   ├── Common/
│   │   ├── Interfaces/
│   │   │   ├── IUnitOfWork.cs
│   │   │   └── IRepository.cs
│   │   └── Utility/
│   │       └── SD.cs                 # Status constants, role names, helper methods
│   └── Services/
│       ├── Interface/
│       └── Implementation/
│           ├── VillaService.cs       # Availability logic, per-type room computation
│           ├── BookingService.cs     # CRUD + status transitions
│           ├── DashboardService.cs   # KPI metrics, chart data aggregation
│           ├── VillaNumberService.cs
│           └── AmenityService.cs
│
├── LuxNest.Infrastructure/
│   ├── Data/
│   │   └── ApplicationDbContext.cs
│   ├── Repository/
│   │   ├── Repository.cs             # Generic repository implementation
│   │   └── UnitOfWork.cs
│   └── Migrations/
│
└── LuxNest.Web/
    ├── Controllers/
    │   ├── HomeController.cs         # Landing page, availability search
    │   ├── BookingController.cs      # Booking lifecycle, Stripe, invoices
    │   ├── DashboardController.cs    # Analytics JSON API endpoints
    │   ├── VillaController.cs
    │   ├── VillaNumberController.cs
    │   └── AmenityController.cs
    ├── Views/
    ├── ViewModels/
    └── wwwroot/
        └── js/
            └── dashboard/            # One JS file per chart type (radial, pie, line)
```

---

## 🗄️ Data Model

**Villa**
```
Id · Name · Description · Price · Sqft · Occupancy · ImageUrl · CreatedDate · UpdatedDate
[NotMapped]: IsAvailable · TotalCapacity · UnavailableReason
             Rooms2p · Rooms3p · Available2p · Available3p · AvailableRoomsCount
```

**VillaNumber**
```
Id · Villa_Number (e.g. 301) · VillaId (FK) · Capacity (default: 2) · SpecialDetails
```

**Booking**
```
Id · VillaId · UserId · Name · Phone · Email
CheckInDate · CheckOutDate · Nights · NumberOfRooms · NumberOfGuests
TotalCost · Status · VillaNumber (assigned room)
BookingDate · ActualCheckInDate · ActualCheckOutDate
StripeSessionId · StripePaymentIntentId · IsPaymentSuccessful · PaymentDate
```

**ApplicationUser** *(extends `IdentityUser`)*
```
Name · CreatedAt
```

**Booking status lifecycle:**

```
Pending ──(payment confirmed)──► Approved ──(guest arrives)──► CheckedIn ──(guest departs)──► Completed
                                     │
                                     └──(customer cancels)──► Cancelled / Refunded
```

---

## ⚠️ Demo Limitations

| Limitation | Details |
| --- | --- |
| Stripe test mode | Use test card `4242 4242 4242 4242` — no real charges are processed |
| LocalDB only | Data exists only on the local machine |
| Syncfusion trial | PDF and Word invoices display a watermark |
| No email notifications | Booking confirmations and cancellation receipts are not sent |
| No image upload | Villa images are configured via URL string, not file upload |
| Single currency | All prices are denominated in USD |
| No secrets management | Stripe keys are stored in `appsettings.json` for demo convenience |

---

## 🚀 Running the Project

**Prerequisites:**
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) — included with Visual Studio
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code with the C# extension
- A free [Stripe account](https://dashboard.stripe.com/register) for test API keys

```bash
# 1. Clone the repository
git clone https://github.com/YOUR_USERNAME/LuxNest.git
cd LuxNest

# 2. Add your Stripe keys to appsettings.json (see Configuration below)

# 3. Apply database migrations
cd LuxNest.Web
dotnet ef database update

# 4. Run the application
dotnet run
```

The application seeds an Admin account and sample data (3 villas, room numbers, amenities) automatically on first run.

**Default admin credentials:**
```
Email:    admin@gmail.com
Password: Admin123*
```

---

## ⚙️ Configuration

In `LuxNest.Web/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=LuxNestDb;Trusted_Connection=True;"
  },
  "Stripe": {
    "PublishableKey": "pk_test_YOUR_PUBLISHABLE_KEY",
    "SecretKey":      "sk_test_YOUR_SECRET_KEY"
  }
}
```

> [!NOTE]
> Get your test keys from [Stripe Dashboard → Developers → API Keys](https://dashboard.stripe.com/test/apikeys).

---

## 👩‍💻 Author

**Draga Monica**  
GitHub: [@DraganMonica](https://github.com/DraganMonica)

---

<div align="center">

*Built with ❤️ using ASP.NET Core 8.0*

</div>
