# SPA Salon Reservation System — Design Spec

**Date:** 2026-09-20  
**Project:** Ireq SPA Salon — Calendar-Based Reservation Website & PWA  
**Stack:** .NET Core 8 (DDD) + React 18 (Vite + TypeScript) + SQL Server  

---

## 1. Project Overview

A full-stack reservation platform for a luxury SPA salon located in Iraq. Customers can browse services, view staff profiles, and book appointments via an interactive calendar. Staff can manage their schedules and admins manage everything via a dashboard. The system supports bilingual Arabic/English with RTL layout, social login, SMS/WhatsApp/email notifications, a loyalty points system, and waitlist management.

---

## 2. Architecture

### High-Level

```
[React SPA (Vite + TypeScript)] ←→ [.NET Core 8 Web API (DDD + CQRS)]
                                              ↓
                                   [SQL Server (EF Core 8)]
                                              ↓
                           [SendGrid | Twilio SMS | WhatsApp Business API]
                                              ↓
                                [Hangfire — background job scheduler]
```

### Backend — Clean Architecture with DDD

**Layer responsibilities:**
- `API` — Controllers, middleware (auth, error handling, localization), Swagger
- `Application` — CQRS (MediatR), DTOs, validators (FluentValidation), AutoMapper
- `Domain` — Aggregates, Value Objects, Domain Events, Repository interfaces (no infra deps)
- `Infrastructure` — EF Core DbContext, SQL Server repositories, external APIs, Identity

### Frontend — Feature-Sliced Architecture

Feature folders colocate components, hooks, and API calls by domain area.

---

## 3. Bounded Contexts

### 3.1 Bookings
Core context. Manages the full reservation lifecycle.
- **Aggregate Root:** `Booking` (Id, CustomerId, StaffId, ServiceId, Date, TimeSlot, Status, Notes, LoyaltyPointsEarned)
- **Value Object:** `BookingSlot` (Date, StartTime, EndTime)
- **Domain Events:** `BookingCreated`, `BookingConfirmed`, `BookingCancelled`, `BookingCompleted`, `BookingNoShow`
- **Status FSM:** `Pending → Confirmed → Completed | Cancelled | NoShow`
- Business rules: No double-booking a staff member; slots must align to staff working hours; minimum 2h cancellation notice

### 3.2 Services
- **Aggregate Root:** `SalonService` (Id, NameAr, NameEn, DescriptionAr, DescriptionEn, Category, DurationMinutes, Price, ImageUrl, IsActive)
- **Categories:** HaircutStyling, FacialSkincare, MassageTherapy, NailCare, BodyTreatments, Waxing, MakeupBeauty, EyelashEyebrow

### 3.3 Staff
- **Aggregate Root:** `StaffMember` (Id, FullName, Bio, Photo, SpecialtyServiceIds, IsActive)
- **Entity:** `WorkingSchedule` (StaffId, DayOfWeek, StartTime, EndTime, IsOff)
- **Entity:** `TimeOff` (StaffId, Date, Reason)

### 3.4 Customers
- **Aggregate Root:** `Customer` (Id, FullName, Phone, Email, PreferredLanguage, LoyaltyPoints, ExternalAuthId, Provider)
- **Entity:** `LoyaltyTransaction` (Id, CustomerId, PointsDelta, Reason, BookingId, CreatedAt)
- Loyalty rule: 10 points per completed booking; 100 points = one free service (admin-defined)

### 3.5 Notifications
- **Entity:** `Notification` (Id, BookingId, CustomerId, Channel, Type, Status, SentAt, Error)
- **Channels:** Email (SendGrid), SMS (Twilio), WhatsApp (Twilio), Push (web-push)
- **Types:** BookingConfirmed, BookingReminder24h, BookingReminder1h, BookingCancelled, WaitlistAvailable
- Hangfire schedules reminder jobs when a booking is confirmed

### 3.6 Waitlist
- **Aggregate Root:** `WaitlistEntry` (Id, CustomerId, ServiceId, PreferredStaffId, PreferredDate, Status)
- **Status FSM:** `Waiting → Offered → Booked | Expired`
- When a booking is cancelled, system checks waitlist and notifies matching customers

### 3.7 Identity
- ASP.NET Core Identity + Google OAuth 2.0 + Facebook OAuth
- JWT Bearer tokens (15min access + 7d refresh)
- Roles: `Customer`, `Staff`, `Admin`, `SuperAdmin`

---

## 4. API Design

### Auth
- `POST /api/auth/google` — exchange Google token
- `POST /api/auth/facebook` — exchange Facebook token
- `POST /api/auth/refresh` — refresh JWT
- `POST /api/auth/logout`

### Bookings
- `GET  /api/availability?serviceId=&staffId=&date=` — available slots
- `POST /api/bookings` — create booking
- `GET  /api/bookings/{id}` — get booking
- `PUT  /api/bookings/{id}/cancel` — cancel booking
- `GET  /api/bookings/my` — customer's bookings
- `GET  /api/admin/bookings` — all bookings (admin)
- `PUT  /api/admin/bookings/{id}/confirm` — confirm booking
- `PUT  /api/admin/bookings/{id}/complete` — mark complete
- `PUT  /api/admin/bookings/{id}/no-show` — mark no-show

### Services
- `GET  /api/services` — list all active services
- `GET  /api/services/{id}` — service detail
- `POST /api/admin/services` — create
- `PUT  /api/admin/services/{id}` — update
- `DELETE /api/admin/services/{id}` — soft delete

### Staff
- `GET  /api/staff` — list active staff
- `GET  /api/staff/{id}` — staff detail + schedule
- `POST /api/admin/staff` — create
- `PUT  /api/admin/staff/{id}` — update
- `PUT  /api/admin/staff/{id}/schedule` — update working hours

### Customers
- `GET  /api/customers/me` — profile
- `PUT  /api/customers/me` — update profile
- `GET  /api/customers/me/loyalty` — loyalty points & history
- `GET  /api/admin/customers` — all customers (admin)

### Waitlist
- `POST /api/waitlist` — join waitlist
- `GET  /api/waitlist/my` — my waitlist entries
- `DELETE /api/waitlist/{id}` — leave waitlist

---

## 5. Frontend Pages & Features

### Public / Customer-Facing

| Page | Description |
|------|-------------|
| **Home** | Hero with salon branding, services overview, staff showcase, CTA to book |
| **Services** | Filterable grid of all services by category, with images & pricing |
| **Staff** | Staff profile cards with specialties |
| **Book** | Single-page calendar: pick date → see available slots → pick staff → confirm |
| **My Bookings** | List of upcoming/past bookings, cancel option |
| **Loyalty** | Points balance, transaction history, how-to-earn guide |
| **Waitlist** | My waitlist entries |
| **Login** | Google / Facebook OAuth buttons |

### Admin Panel (/admin)

| Page | Description |
|------|-------------|
| **Dashboard** | Today's bookings, revenue, upcoming slots, quick stats |
| **Bookings** | Full calendar view + list, filter by staff/date, confirm/cancel/complete |
| **Services** | CRUD services, upload images, set pricing & duration |
| **Staff** | CRUD staff, set working hours, time off management |
| **Customers** | Customer list, loyalty adjustment, booking history |
| **Waitlist** | Active waitlist entries, manual offer |
| **Analytics** | Bookings by period, popular services, staff utilization (recharts) |

---

## 6. UI/UX Design System

### Theme
- **Background:** Deep charcoal (#0D0D0D, #1A1A2E)
- **Primary accent:** Electric purple (#7B2FBE) → magenta (#C44DFF)
- **Text:** Off-white (#F5F5F5), muted gray (#A0A0B0)
- **Cards:** Glassmorphism (rgba(255,255,255,0.05), backdrop-filter: blur(12px))
- **Gradients:** Purple-to-magenta on CTAs and highlights
- **Typography:** Cairo (Arabic), Inter (English) from Google Fonts
- **Animations:** Framer Motion — smooth page transitions, card hovers, slot selection

### RTL Support
- i18next with ar and en namespaces
- CSS dir="rtl" toggle on root element
- RTL-aware calendar and form layouts

### Calendar UX (Booking Page)
1. FullCalendar in dayGrid month view — shows available/busy days
2. Click a date → slide-in panel shows time slots for that day
3. Select a slot → service picker appears
4. Select service → staff auto-suggested or manually chosen
5. Review summary → confirm button → success animation + email/SMS confirmation

---

## 7. Background Jobs (Hangfire)

| Job | Schedule |
|-----|----------|
| Send booking reminders 24h before | Daily at 8 AM |
| Send booking reminders 1h before | Every hour |
| Expire waitlist entries older than 48h | Daily |
| Process notification retry queue | Every 15 min |

---

## 8. Key Business Rules

1. A staff member cannot have overlapping bookings
2. Slots are generated from staff working hours (30-min intervals unless service > 30min)
3. Booking cannot be cancelled less than 2 hours before appointment
4. Loyalty points awarded only on Completed bookings (not confirmed)
5. Waitlist notification sent to first matching customer; entry moves to Offered (24h to accept)
6. Admin can override any business rule via admin panel

---

## 9. Tech Stack Summary

### Backend
| Package | Purpose |
|---------|---------|
| .NET 8 + ASP.NET Core | API framework |
| Entity Framework Core 8 | ORM |
| MediatR | CQRS implementation |
| FluentValidation | Command/query validation |
| AutoMapper | DTO mapping |
| ASP.NET Core Identity | Auth base |
| JWT Bearer | Token auth |
| Hangfire | Background job scheduling |
| Twilio | SMS + WhatsApp |
| SendGrid | Email |
| Serilog | Structured logging |
| Swagger / Scalar | API documentation |

### Frontend
| Package | Purpose |
|---------|---------|
| Vite + React 18 + TypeScript | Build tooling |
| @fullcalendar/react | Calendar component |
| TanStack Query (React Query) | Server state |
| Zustand | Client state |
| React Hook Form + Zod | Forms & validation |
| Framer Motion | Animations |
| i18next + react-i18next | Internationalization (AR/EN) |
| React Router v6 | Routing |
| Axios | HTTP client |
| Recharts | Admin charts |
| React Toastify | Toast notifications |
| @mui/x-date-pickers | Date/time picker UI |

---

## 10. Deployment (Out of Scope for Phase 1)

- Backend: Docker container → Azure App Service or VPS
- Frontend: Static hosting (Azure Static Web Apps / Vercel)
- DB: Azure SQL or self-hosted SQL Server
- Hangfire: In-process with SQL Server storage

---

## 11. Out of Scope (Phase 1)

- Online payment processing
- Customer mobile apps (native iOS/Android)
- Multi-branch / franchise support
- Video consultation
- Inventory / product sales
