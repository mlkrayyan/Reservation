# Graph Report - .  (2026-09-22)

## Corpus Check
- Corpus is ~18,630 words - fits in a single context window. You may not need a graph.

## Summary
- 852 nodes · 1233 edges · 84 communities (48 shown, 36 thin omitted)
- Extraction: 80% EXTRACTED · 18% INFERRED · 3% AMBIGUOUS · INFERRED: 217 edges (avg confidence: 0.87)
- Token cost: 663,570 input · 0 output

## Community Hubs (Navigation)
- [[_COMMUNITY_Frontend API Surface|Frontend API Surface]]
- [[_COMMUNITY_React Component Modules|React Component Modules]]
- [[_COMMUNITY_Infrastructure Composition & Persistence|Infrastructure Composition & Persistence]]
- [[_COMMUNITY_Frontend Dependency Manifest|Frontend Dependency Manifest]]
- [[_COMMUNITY_Repository Queries & DB Schema|Repository Queries & DB Schema]]
- [[_COMMUNITY_API Endpoints & CQRS Wiring|API Endpoints & CQRS Wiring]]
- [[_COMMUNITY_Domain Aggregates & Business Rules|Domain Aggregates & Business Rules]]
- [[_COMMUNITY_Launch Profiles & Hosting Config|Launch Profiles & Hosting Config]]
- [[_COMMUNITY_MVC Controller Classes|MVC Controller Classes]]
- [[_COMMUNITY_Frontend Shell & Auth Guard|Frontend Shell & Auth Guard]]
- [[_COMMUNITY_EF Entity Type Configurations|EF Entity Type Configurations]]
- [[_COMMUNITY_Architecture Spec & SharedKernel|Architecture Spec & SharedKernel]]
- [[_COMMUNITY_Browser TypeScript Config|Browser TypeScript Config]]
- [[_COMMUNITY_MediatR Handler Classes|MediatR Handler Classes]]
- [[_COMMUNITY_Node Tooling TypeScript Config|Node Tooling TypeScript Config]]
- [[_COMMUNITY_Vite Template Branding Assets|Vite Template Branding Assets]]
- [[_COMMUNITY_Entity Base & Child Entities|Entity Base & Child Entities]]
- [[_COMMUNITY_Hero Brand Visual Identity|Hero Brand Visual Identity]]
- [[_COMMUNITY_App Settings & JWT Config|App Settings & JWT Config]]
- [[_COMMUNITY_Booking Reminder Job|Booking Reminder Job]]
- [[_COMMUNITY_StaffMember Aggregate Methods|StaffMember Aggregate Methods]]
- [[_COMMUNITY_Customer Repository Contract|Customer Repository Contract]]
- [[_COMMUNITY_Service Catalog & Pagination|Service Catalog & Pagination]]
- [[_COMMUNITY_Waitlist Repository Queries|Waitlist Repository Queries]]
- [[_COMMUNITY_Booking Aggregate Tests|Booking Aggregate Tests]]
- [[_COMMUNITY_Waitlist & Notification Contexts|Waitlist & Notification Contexts]]
- [[_COMMUNITY_WaitlistEntry Aggregate|WaitlistEntry Aggregate]]
- [[_COMMUNITY_Booking State Methods|Booking State Methods]]
- [[_COMMUNITY_Generic Repository Base|Generic Repository Base]]
- [[_COMMUNITY_Notification Repository|Notification Repository]]
- [[_COMMUNITY_Customer Aggregate Methods|Customer Aggregate Methods]]
- [[_COMMUNITY_Notification Jobs (Unwired)|Notification Jobs (Unwired)]]
- [[_COMMUNITY_Oxlint Rule Config|Oxlint Rule Config]]
- [[_COMMUNITY_Exception Handling Middleware|Exception Handling Middleware]]
- [[_COMMUNITY_InitialCreate Migration|InitialCreate Migration]]
- [[_COMMUNITY_SalonService Aggregate Methods|SalonService Aggregate Methods]]
- [[_COMMUNITY_Booking Repository Impl|Booking Repository Impl]]
- [[_COMMUNITY_Service Repository Impl|Service Repository Impl]]
- [[_COMMUNITY_Staff Repository Impl|Staff Repository Impl]]
- [[_COMMUNITY_Customer Loyalty Tests|Customer Loyalty Tests]]
- [[_COMMUNITY_EF Model Snapshot|EF Model Snapshot]]
- [[_COMMUNITY_Notification Repository Contract|Notification Repository Contract]]
- [[_COMMUNITY_Unit of Work|Unit of Work]]
- [[_COMMUNITY_Notification Entity Methods|Notification Entity Methods]]
- [[_COMMUNITY_Booking Repository Contract|Booking Repository Contract]]
- [[_COMMUNITY_Customer Repository Impl|Customer Repository Impl]]
- [[_COMMUNITY_Service Repository Contract|Service Repository Contract]]
- [[_COMMUNITY_Staff Repository Contract|Staff Repository Contract]]
- [[_COMMUNITY_Development Logging Config|Development Logging Config]]
- [[_COMMUNITY_DbContext Model Build|DbContext Model Build]]
- [[_COMMUNITY_Slot Value Object & Cancellation Rule|Slot Value Object & Cancellation Rule]]
- [[_COMMUNITY_Migration Designer Snapshot|Migration Designer Snapshot]]
- [[_COMMUNITY_Create Booking Handler Test|Create Booking Handler Test]]
- [[_COMMUNITY_Create Booking Validator|Create Booking Validator]]
- [[_COMMUNITY_Database Seeder|Database Seeder]]
- [[_COMMUNITY_TypeScript Solution References|TypeScript Solution References]]
- [[_COMMUNITY_Paged Result Envelope|Paged Result Envelope]]
- [[_COMMUNITY_Unit of Work Contract|Unit of Work Contract]]
- [[_COMMUNITY_Application Test Scaffold|Application Test Scaffold]]
- [[_COMMUNITY_Domain Test Scaffold|Domain Test Scaffold]]
- [[_COMMUNITY_Infrastructure DI Extension|Infrastructure DI Extension]]
- [[_COMMUNITY_Input UI Primitive|Input UI Primitive]]
- [[_COMMUNITY_Domain Event Marker|Domain Event Marker]]
- [[_COMMUNITY_Domain Class1 Placeholder|Domain Class1 Placeholder]]
- [[_COMMUNITY_Infrastructure Class1 Placeholder|Infrastructure Class1 Placeholder]]
- [[_COMMUNITY_Application Class1 Placeholder|Application Class1 Placeholder]]
- [[_COMMUNITY_Application Layer Placeholder|Application Layer Placeholder]]

## God Nodes (most connected - your core abstractions)
1. `Booking Aggregate` - 31 edges
2. `compilerOptions` - 18 edges
3. `API Composition Root (Program.cs)` - 18 edges
4. `Booking Page (multi-step wizard)` - 16 edges
5. `Customer Aggregate` - 16 edges
6. `compilerOptions` - 15 edges
7. `SPA Salon Reservation System Design Spec` - 15 edges
8. `ApplicationDbContext` - 15 edges
9. `SalonService Entity` - 14 edges
10. `AddInfrastructure Composition Root` - 14 edges

## Surprising Connections (you probably didn't know these)
- `No overlapping bookings per staff member` --semantically_similar_to--> `StaffMember.IsAvailableOn (availability check)`  [INFERRED] [semantically similar]
  docs/superpowers/specs/2026-09-20-spa-salon-reservation-design.md → src/SpaReservation.Domain/Staff/StaffMember.cs
- `Slot generation from staff working hours` --semantically_similar_to--> `StaffMember.IsAvailableOn (availability check)`  [INFERRED] [semantically similar]
  docs/superpowers/specs/2026-09-20-spa-salon-reservation-design.md → src/SpaReservation.Domain/Staff/StaffMember.cs
- `Staff bounded context` --semantically_similar_to--> `StaffMember.IsAvailableOn (availability check)`  [INFERRED] [semantically similar]
  docs/superpowers/specs/2026-09-20-spa-salon-reservation-design.md → src/SpaReservation.Domain/Staff/StaffMember.cs
- `CreateBookingCommandHandlerTests` --conceptually_related_to--> `No overlapping bookings per staff member`  [AMBIGUOUS]
  tests/SpaReservation.Application.Tests/Bookings/CreateBookingCommandHandlerTests.cs → docs/superpowers/specs/2026-09-20-spa-salon-reservation-design.md
- `Slot generation from staff working hours` --references--> `SalonService Entity`  [INFERRED]
  docs/superpowers/specs/2026-09-20-spa-salon-reservation-design.md → src/SpaReservation.Domain/Services/SalonService.cs

## Hyperedges (group relationships)
- **Authentication Session and Role-Gated Routing Flow** — authmodal_AuthModal, useauthstore_AuthStore, protectedroute_ProtectedRoute, app_App, mainlayout_MainLayout, adminlayout_AdminLayout [INFERRED 0.85]
- **SPA Shell Bootstrap Chain (HTML to Layout)** — indexhtml_HtmlEntry, main_Bootstrap, app_App, i18n_I18nConfig, mainlayout_MainLayout [INFERRED 0.85]
- **Frontend Build and Typecheck Toolchain** — package_FrontendPackageManifest, vite_ViteConfig, tsconfig_SolutionTsConfig, tsconfigapp_AppTsConfig, tsconfignode_NodeTsConfig, oxlintrc_OxlintConfig [INFERRED 0.85]
- **End-to-End Booking Confirmation Flow** — booking_page, endpoint_services_get, endpoint_staff_get, booking_confirmpayload, endpoint_bookings_post, useauthstore_updateloyaltypoints, loyalty_rules, mybookings_page [EXTRACTED 1.00]
- **Admin Console Surface (dashboard hub + five management screens)** — admindashboard_page, adminbookings_page, adminservices_page, adminstaff_page, adminwaitlist_page, adminanalytics_page [EXTRACTED 1.00]
- **Backendless Demo Resilience (every page degrades to seeded fixtures)** — pattern_offline_fallback, pattern_optimistic_mutation, concept_demo_dataset, services_page, staff_page, booking_page, mybookings_page, loyalty_page, waitlist_page, adminbookings_page [INFERRED 0.85]
- **Booking Creation Request Flow (HTTP -> MediatR -> Domain -> Persistence)** — bookingscontroller_BookingsController, createbookingcommand_CreateBookingCommand, createbookingcommandhandler_CreateBookingCommandHandler, bookingslot_BookingSlot, booking_Booking, ibookingrepository_IBookingRepository, iunitofwork_IUnitOfWork [EXTRACTED 1.00]
- **MediatR Assembly-Scan Handler Registration** — program_main, createbookingcommand_CreateBookingCommand, createbookingcommandhandler_CreateBookingCommandHandler, confirmbookingcommandhandler_ConfirmBookingCommandHandler, cancelbookingcommandhandler_CancelBookingCommandHandler, getbookingqueryhandler_GetBookingQueryHandler [INFERRED 0.85]
- **Paged Public Catalog Read Pattern (repository tuple -> PagedResult envelope)** — servicescontroller_ServicesController, staffcontroller_StaffController, pagedresult_PagedResult, isalonservicerepository_ISalonServiceRepository, istaffrepository_IStaffRepository [INFERRED 0.85]
- **Booking lifecycle: guarded status transitions each publishing a domain event** — booking_Booking, bookingstatus_BookingStatus, booking_StatusTransitionInvariants, bookingcreated_BookingCreated, bookingconfirmed_BookingConfirmed, bookingcancelled_BookingCancelled, bookingcompleted_BookingCompleted, booking_CancellationWindowInvariant [EXTRACTED 1.00]
- **Loyalty points flow: completion award -> event -> customer balance + ledger line** — booking_Booking, booking_LoyaltyPointsAward, bookingcompleted_BookingCompleted, customer_Customer, customer_LoyaltyPointsLedger, loyaltytransaction_LoyaltyTransaction [INFERRED 0.85]
- **Double-booking prevention: service duration -> slot -> overlap test -> staff/day query** — salonservice_SalonService, bookingslot_BookingSlot, bookingslot_OverlapDetection, ibookingrepository_IBookingRepository, booking_Booking [INFERRED 0.75]
- **Staff availability determination (time off + weekly schedule + requested window)** — staffmember_StaffMember, staffmember_IsAvailableOn, workingschedule_WorkingSchedule, timeoff_TimeOff, design_SlotGenerationRule, design_CalendarBookingFlow [INFERRED 0.85]
- **Waitlist offer lifecycle: cancellation fan-out, 24h offer, 48h expiry** — waitlistentry_WaitlistEntry, waitliststatus_WaitlistStatus, iwaitlistrepository_IWaitlistRepository, design_WaitlistStatusFsm, design_WaitlistOfferWindow, design_WaitlistCancellationTrigger, design_HangfireBackgroundJobs [INFERRED 0.85]
- **SharedKernel DDD contract realising the Clean Architecture layering decision** — entity_Entity, aggregateroot_AggregateRoot, idomainevent_IDomainEvent, irepository_IRepository, iunitofwork_IUnitOfWork, valueobject_ValueObject, design_CleanArchitectureDdd [EXTRACTED 1.00]
- **Hangfire Reminder and Expiry Pipeline** — dependencyinjection_addinfrastructure, dependencyinjection_hangfire_inmemory_storage, hangfire_recurring_schedule, ibookingreminderjob_ibookingreminderjob, bookingreminderjob_bookingreminderjob, notification_notification, waitlistentry_waitlistentry [INFERRED 0.85]
- **Assembly-Scanned EF Model Mapping** — applicationdbcontext_applicationdbcontext, applicationdbcontext_assembly_scanned_configuration, bookingconfiguration_bookingconfiguration, customerconfiguration_customerconfiguration, customerconfiguration_loyaltytransactionconfiguration, notificationconfiguration_notificationconfiguration, salonserviceconfiguration_salonserviceconfiguration, staffmemberconfiguration_staffmemberconfiguration, waitlistentryconfiguration_waitlistentryconfiguration [EXTRACTED 1.00]
- **Demo Dataset Bootstrap via Domain Factories** — dbseeder_seedasync, dbseeder_idempotent_seeding, salonservice_salonservice, staffmember_staffmember, workingschedule_workingschedule, customer_customer, booking_booking, bookingslot_bookingslot [EXTRACTED 1.00]
- **Generic Repository Base and Its Aggregate Specializations** — repository_Repository, irepository_IRepository, bookingrepository_BookingRepository, customerrepository_CustomerRepository, salonservicerepository_SalonServiceRepository, staffrepository_StaffRepository, waitlistrepository_WaitlistRepository [EXTRACTED 1.00]
- **Slot Availability Computation Data Path** — bookingrepository_GetByStaffAndDateAsync, staffrepository_GetActiveAsync, staffrepository_GetByServiceAsync, initialcreate_WorkingScheduleTable, initialcreate_TimeOffTable, initialcreate_BookingsTable, concept_EagerLoadingAvailabilityGraph [INFERRED 0.85]
- **Background Job Claim, Mutate and Commit Cycle** — notificationrepository_GetPendingAsync, waitlistrepository_GetExpiredOffersAsync, waitlistrepository_GetWaitingByServiceAsync, unitofwork_SaveChangesAsync, concept_BackgroundJobPollingPredicate, concept_UnitOfWorkSaveSemantics [INFERRED 0.75]

## Communities (84 total, 36 thin omitted)

### Community 0 - "Frontend API Surface"
Cohesion: 0.05
Nodes (70): Admin Analytics Page (Recharts), Status->Endpoint Map (confirm/complete/cancel/no-show), Admin Bookings Management Page, Admin Dashboard Overview Page, Admin Services Catalog CRUD Page, Admin Staff & Working Hours Page, Active/Off-Duty Toggle (local state only), Admin Waitlist Queue Page (+62 more)

### Community 1 - "React Component Modules"
Cohesion: 0.06
Nodes (32): revenueData, serviceCategoryData, AdminBookingItem, AdminLayout(), ServiceRecord, StaffMemberAdmin, AdminWaitlistEntry, AuthModal() (+24 more)

### Community 2 - "Infrastructure Composition & Persistence"
Cohesion: 0.08
Nodes (53): ApplicationDbContext, Assembly-Scanned EF Configuration Discovery, Booking (aggregate root), BookingConfiguration, Owned Value-Object Mapping of BookingSlot, BookingReminderJob (Hangfire worker), ExpireWaitlistEntriesAsync, Send1hRemindersAsync (+45 more)

### Community 3 - "Frontend Dependency Manifest"
Cohesion: 0.05
Nodes (41): dependencies, axios, dayjs, @emotion/react, @emotion/styled, framer-motion, @fullcalendar/daygrid, @fullcalendar/interaction (+33 more)

### Community 4 - "Repository Queries & DB Schema"
Cohesion: 0.10
Nodes (42): ApplicationDbContext, ApplicationDbContext Model Snapshot, BookingRepository, BookingRepository.GetAllAsync(from,to), BookingRepository.GetByCustomerIdAsync, BookingRepository.GetByStaffAndDateAsync, IsActive Flag as Soft Catalog Visibility, Status+Timestamp Polling Predicate for Background Jobs (+34 more)

### Community 5 - "API Endpoints & CQRS Wiring"
Cohesion: 0.11
Nodes (34): API Development Settings, API Application Settings, AuthController, GoogleAuthRequest DTO, BookingReminderJob, BookingsController, CancelBookingCommand, CancelBookingCommandHandler (+26 more)

### Community 6 - "Domain Aggregates & Business Rules"
Cohesion: 0.15
Nodes (29): Cross-aggregate references held as bare Guids, DDD aggregate root vs child entity split, Booking Aggregate, Private parameterless constructor for EF Core materialization, Flat 10-point award on booking completion, Booking status transition invariants (guarded state machine), BookingCancelled domain event, BookingCompleted domain event (carries LoyaltyPointsEarned) (+21 more)

### Community 7 - "Launch Profiles & Hosting Config"
Cohesion: 0.07
Nodes (28): ASPNETCORE_ENVIRONMENT, applicationUrl, commandName, dotnetRunMessages, environmentVariables, launchBrowser, launchUrl, applicationUrl (+20 more)

### Community 8 - "MVC Controller Classes"
Cohesion: 0.08
Nodes (13): ControllerBase, AuthController, GoogleAuthRequest, BookingsController, CustomersController, ServicesController, StaffController, IConfiguration (+5 more)

### Community 9 - "Frontend Shell & Auth Guard"
Cohesion: 0.14
Nodes (25): AdminLayout Management Shell, Admin Sidebar Navigation Manifest, App Route Tree (React Router), AuthModal Login/Register Dialog, Simulated Authentication Fallback, Google/Facebook Social Login Handler, Button UI Primitive, Card UI Primitive (+17 more)

### Community 10 - "EF Entity Type Configurations"
Cohesion: 0.10
Nodes (8): BookingConfiguration, CustomerConfiguration, LoyaltyTransactionConfiguration, NotificationConfiguration, SalonServiceConfiguration, StaffMemberConfiguration, WaitlistEntryConfiguration, IEntityTypeConfiguration

### Community 11 - "Architecture Spec & SharedKernel"
Cohesion: 0.21
Nodes (21): AggregateRoot (marker base class), Bilingual AR/EN with RTL layout, Bookings bounded context, Calendar-first booking flow, Clean Architecture with DDD layering, Customers bounded context, Feature-Sliced frontend architecture, Dark glassmorphism visual theme (+13 more)

### Community 12 - "Browser TypeScript Config"
Cohesion: 0.10
Nodes (19): compilerOptions, allowArbitraryExtensions, allowImportingTsExtensions, erasableSyntaxOnly, jsx, lib, module, moduleDetection (+11 more)

### Community 13 - "MediatR Handler Classes"
Cohesion: 0.10
Nodes (12): CancelBookingCommandHandler, ConfirmBookingCommandHandler, CreateBookingCommandHandler, IRequestHandler, GetBookingQueryHandler, IBookingRepository, IUnitOfWork, IBookingRepository (+4 more)

### Community 14 - "Node Tooling TypeScript Config"
Cohesion: 0.12
Nodes (16): compilerOptions, allowImportingTsExtensions, erasableSyntaxOnly, lib, module, moduleDetection, noEmit, noFallthroughCasesInSwitch (+8 more)

### Community 15 - "Vite Template Branding Assets"
Cohesion: 0.19
Nodes (17): Booking / Scheduling UI Iconography (absent), Browser Tab / Brand Identity, Community / Social Link Affordances, Developer Documentation Link, Vite + React Template Scaffolding (vestigial), frontend/index.html (favicon link), App Favicon (Vite bolt mark), bluesky-clip clipPath (+9 more)

### Community 16 - "Entity Base & Child Entities"
Cohesion: 0.14
Nodes (5): LoyaltyTransaction, Entity, List, TimeOff, WorkingSchedule

### Community 17 - "Hero Brand Visual Identity"
Cohesion: 0.24
Nodes (13): --accent-primary Electric Purple Theme Token (#9D00FF), Hero Brand Illustration (hero.png), Visual Identity of the Spa Booking Product, Electric Purple / Violet Brand Accent Palette, Home Page Hero Section, Isometric Exploded-Layer Composition, Light Asset vs Dark Charcoal App Theme, Minimal White / Transparent Negative-Space Backdrop (+5 more)

### Community 18 - "App Settings & JWT Config"
Cohesion: 0.15
Nodes (12): AllowedHosts, ConnectionStrings, DefaultConnection, JwtSettings, Audience, ExpiryMinutes, Issuer, Secret (+4 more)

### Community 19 - "Booking Reminder Job"
Cohesion: 0.17
Nodes (4): BookingReminderJob, IBookingReminderJob, ApplicationDbContext, ILogger

### Community 22 - "Service Catalog & Pagination"
Cohesion: 0.38
Nodes (10): ISalonServiceRepository, PagedResult<T>, Offset Pagination Envelope, Controller-to-Repository Direct Access, Bilingual Arabic/English catalog fields, SalonService Entity, Soft deactivation instead of deletion (IsActive), ServiceCategory Enum (+2 more)

### Community 25 - "Waitlist & Notification Contexts"
Cohesion: 0.42
Nodes (9): Hangfire background job schedule, Notifications bounded context, Waitlist bounded context, Cancellation-triggered waitlist matching, Waitlist offer and expiry window, Waitlist status state machine, IWaitlistRepository, WaitlistEntry aggregate root (+1 more)

### Community 29 - "Notification Repository"
Cohesion: 0.29
Nodes (3): INotificationRepository, NotificationRepository, ApplicationDbContext

### Community 31 - "Notification Jobs (Unwired)"
Cohesion: 0.40
Nodes (3): INotificationJobs, NotificationJobs, INotificationRepository

### Community 32 - "Oxlint Rule Config"
Cohesion: 0.33
Nodes (5): plugins, rules, react/only-export-components, react/rules-of-hooks, $schema

### Community 33 - "Exception Handling Middleware"
Cohesion: 0.40
Nodes (3): ExceptionHandlingMiddleware, RequestDelegate, ILogger

### Community 34 - "InitialCreate Migration"
Cohesion: 0.33
Nodes (3): Migration, InitialCreate, SpaReservation.Infrastructure.Data.Migrations

### Community 40 - "EF Model Snapshot"
Cohesion: 0.40
Nodes (3): ApplicationDbContextModelSnapshot, SpaReservation.Infrastructure.Data.Migrations, ModelSnapshot

### Community 42 - "Unit of Work"
Cohesion: 0.40
Nodes (3): IUnitOfWork, UnitOfWork, ApplicationDbContext

### Community 48 - "Development Logging Config"
Cohesion: 0.40
Nodes (4): Logging, LogLevel, Default, Microsoft.AspNetCore

### Community 50 - "Slot Value Object & Cancellation Rule"
Cohesion: 0.67
Nodes (4): Two-hour cancellation cutoff invariant, BookingSlot Value Object, Slot overlap test (half-open interval double-booking check), ValueObject base record

## Ambiguous Edges - Review These
- `Bilingual EN/AR Translation Catalog` → `AdminLayout Management Shell`  [AMBIGUOUS]
  frontend/src/components/admin/AdminLayout.tsx · relation: conceptually_related_to
- `Hand-Rolled CSS Design System` → `Frontend Package Manifest & Scripts`  [AMBIGUOUS]
  frontend/package.json · relation: conceptually_related_to
- `Axios API Client (api.ts)` → `Frontend README (Vite template)`  [AMBIGUOUS]
  frontend/README.md · relation: conceptually_related_to
- `Staff DTO (id,name,specialization,bio)` → `Admin Staff & Working Hours Page`  [AMBIGUOUS]
  frontend/src/pages/admin/AdminStaff.tsx · relation: shares_data_with
- `Mock Availability Slots (setTimeout stub)` → `Availability Slots Endpoint (unwired / missing)`  [AMBIGUOUS]
  frontend/src/pages/Booking.tsx · relation: conceptually_related_to
- `Admin Analytics Page (Recharts)` → `GET /api/admin/bookings`  [AMBIGUOUS]
  frontend/src/pages/admin/AdminAnalytics.tsx · relation: shares_data_with
- `Admin Dashboard Overview Page` → `GET /api/admin/bookings`  [AMBIGUOUS]
  frontend/src/pages/admin/AdminDashboard.tsx · relation: shares_data_with
- `API Composition Root (Program.cs)` → `CreateBookingCommandValidator`  [AMBIGUOUS]
  src/SpaReservation.API/Program.cs · relation: references
- `AuthController` → `IUnitOfWork`  [AMBIGUOUS]
  src/SpaReservation.API/Controllers/AuthController.cs · relation: references
- `CreateBookingCommandHandler` → `ISalonServiceRepository`  [AMBIGUOUS]
  src/SpaReservation.Application/Bookings/Commands/CreateBookingCommandHandler.cs · relation: references
- `CreateBookingCommandHandler` → `IStaffRepository`  [AMBIGUOUS]
  src/SpaReservation.Application/Bookings/Commands/CreateBookingCommandHandler.cs · relation: references
- `IUnitOfWork` → `Entity (domain base class)`  [AMBIGUOUS]
  src/SpaReservation.Domain/SharedKernel/IUnitOfWork.cs · relation: conceptually_related_to
- `Booking Aggregate` → `Notification (delivery record)`  [AMBIGUOUS]
  src/SpaReservation.Domain/Bookings/Booking.cs · relation: conceptually_related_to
- `Booking Aggregate` → `Class1 (empty template placeholder)`  [AMBIGUOUS]
  src/SpaReservation.Domain/Class1.cs · relation: conceptually_related_to
- `Customer Aggregate` → `Notification (delivery record)`  [AMBIGUOUS]
  src/SpaReservation.Domain/Customers/Customer.cs · relation: conceptually_related_to
- `BookingCreated domain event` → `NotificationType enum (booking + waitlist triggers)`  [AMBIGUOUS]
  src/SpaReservation.Domain/Notifications/NotificationType.cs · relation: conceptually_related_to
- `Entity (domain base class)` → `WaitlistEntry aggregate root`  [AMBIGUOUS]
  src/SpaReservation.Domain/Waitlist/WaitlistEntry.cs · relation: conceptually_related_to
- `AggregateRoot (marker base class)` → `UnitOfWork.SaveChangesAsync`  [AMBIGUOUS]
  src/SpaReservation.Infrastructure/Repositories/UnitOfWork.cs · relation: conceptually_related_to
- `BookingTests (Booking aggregate unit tests)` → `UnitTest1 (empty domain test scaffold)`  [AMBIGUOUS]
  tests/SpaReservation.Domain.Tests/UnitTest1.cs · relation: conceptually_related_to
- `CreateBookingCommandHandlerTests` → `No overlapping bookings per staff member`  [AMBIGUOUS]
  tests/SpaReservation.Application.Tests/Bookings/CreateBookingCommandHandlerTests.cs · relation: conceptually_related_to
- `CreateBookingCommandHandlerTests` → `UnitTest1 (empty application test scaffold)`  [AMBIGUOUS]
  tests/SpaReservation.Application.Tests/UnitTest1.cs · relation: conceptually_related_to
- `Infrastructure Class1 Placeholder` → `AddInfrastructure Composition Root`  [AMBIGUOUS]
  src/SpaReservation.Infrastructure/Class1.cs · relation: conceptually_related_to
- `AddInfrastructure Composition Root` → `INotificationJobs`  [AMBIGUOUS]
  src/SpaReservation.Infrastructure/BackgroundJobs/NotificationJobs.cs · relation: references
- `ExpireWaitlistEntriesAsync` → `IWaitlistRepository`  [AMBIGUOUS]
  src/SpaReservation.Infrastructure/BackgroundJobs/BookingReminderJob.cs · relation: conceptually_related_to
- `BookingRepository.GetByStaffAndDateAsync` → `Index-per-Foreign-Key Strategy`  [AMBIGUOUS]
  src/SpaReservation.Infrastructure/Repositories/BookingRepository.cs · relation: conceptually_related_to
- `StaffRepository.GetByServiceAsync` → `Services Table`  [AMBIGUOUS]
  src/SpaReservation.Infrastructure/Repositories/StaffRepository.cs · relation: conceptually_related_to
- `CustomerRepository.GetByExternalAuthIdAsync` → `Index-per-Foreign-Key Strategy`  [AMBIGUOUS]
  src/SpaReservation.Infrastructure/Repositories/CustomerRepository.cs · relation: conceptually_related_to
- `Hero Brand Illustration (hero.png)` → `Home Page Hero Section`  [AMBIGUOUS]
  frontend/src/assets/hero.png · relation: references
- `Depicted Subject: Two Stacked Floating Squircle Slabs` → `Home Page Hero Section`  [AMBIGUOUS]
  frontend/src/assets/hero.png · relation: semantically_similar_to
- `Minimal White / Transparent Negative-Space Backdrop` → `Light Asset vs Dark Charcoal App Theme`  [AMBIGUOUS]
  frontend/src/index.css · relation: conceptually_related_to
- `Icon Sprite Sheet (icons.svg)` → `Booking / Scheduling UI Iconography (absent)`  [AMBIGUOUS]
  frontend/public/icons.svg · relation: conceptually_related_to
- `social-icon symbol` → `Booking / Scheduling UI Iconography (absent)`  [AMBIGUOUS]
  frontend/public/icons.svg · relation: semantically_similar_to

## Knowledge Gaps
- **186 isolated node(s):** `$schema`, `plugins`, `react/rules-of-hooks`, `react/only-export-components`, `name` (+181 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **36 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **What is the exact relationship between `Bilingual EN/AR Translation Catalog` and `AdminLayout Management Shell`?**
  _Edge tagged AMBIGUOUS (relation: conceptually_related_to) - confidence is low._
- **What is the exact relationship between `Hand-Rolled CSS Design System` and `Frontend Package Manifest & Scripts`?**
  _Edge tagged AMBIGUOUS (relation: conceptually_related_to) - confidence is low._
- **What is the exact relationship between `Axios API Client (api.ts)` and `Frontend README (Vite template)`?**
  _Edge tagged AMBIGUOUS (relation: conceptually_related_to) - confidence is low._
- **What is the exact relationship between `Staff DTO (id,name,specialization,bio)` and `Admin Staff & Working Hours Page`?**
  _Edge tagged AMBIGUOUS (relation: shares_data_with) - confidence is low._
- **What is the exact relationship between `Mock Availability Slots (setTimeout stub)` and `Availability Slots Endpoint (unwired / missing)`?**
  _Edge tagged AMBIGUOUS (relation: conceptually_related_to) - confidence is low._
- **What is the exact relationship between `Admin Analytics Page (Recharts)` and `GET /api/admin/bookings`?**
  _Edge tagged AMBIGUOUS (relation: shares_data_with) - confidence is low._
- **What is the exact relationship between `Admin Dashboard Overview Page` and `GET /api/admin/bookings`?**
  _Edge tagged AMBIGUOUS (relation: shares_data_with) - confidence is low._