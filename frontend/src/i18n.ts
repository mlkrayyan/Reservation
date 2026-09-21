import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import LanguageDetector from 'i18next-browser-languagedetector';

i18n
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    fallbackLng: 'en',
    resources: {
      en: {
        translation: {
          "Home": "Home",
          "Services": "Services",
          "Staff": "Staff",
          "Booking": "Book Appointment",
          "My Bookings": "My Bookings",
          "Loyalty": "Loyalty Program",
          "Waitlist": "Waitlist",
          "Admin": "Admin Panel",
          "Book Now": "Book Now",
          "Login": "Login",
          "Logout": "Logout",
          "Register": "Register",
          "Hero Title": "Experience Ultimate Luxury & Spa Relaxation",
          "Hero Subtitle": "Rejuvenate your body and mind with our world-class treatments and expert specialists in Iraq.",
          "Our Offerings": "Our Offerings",
          "Expert Therapists": "Expert Therapists",
          "Expert Therapists Desc": "Certified professionals dedicated to providing personalized luxury treatments.",
          "Premium Products": "Organic & Premium Products",
          "Premium Products Desc": "Top-tier organic skincare and therapeutic essential oils.",
          "Serene Environment": "Serene Atmosphere",
          "Serene Environment Desc": "A peaceful sanctuary crafted to offer deep tranquility and comfort.",
          "Category All": "All Services",
          "Category HaircutStyling": "Haircut & Styling",
          "Category FacialSkincare": "Facial & Skincare",
          "Category MassageTherapy": "Massage Therapy",
          "Category NailCare": "Nail Care",
          "Category BodyTreatments": "Body Treatments",
          "Category Waxing": "Waxing",
          "Category MakeupBeauty": "Makeup & Beauty",
          "Category EyelashEyebrow": "Eyelashes & Eyebrows",
          "Select Date": "Select Date",
          "Select Time Slot": "Select Time Slot",
          "Select Service": "Select Service",
          "Select Staff": "Select Specialist",
          "Any Staff": "Any Available Specialist",
          "Confirm Booking": "Confirm Reservation",
          "Booking Success": "Reservation Confirmed Successfully!",
          "Loyalty Points Balance": "Loyalty Points Balance",
          "Points": "Points",
          "Cancel Booking": "Cancel Booking",
          "Cancel Notice": "Bookings must be cancelled at least 2 hours before the scheduled time.",
          "Join Waitlist": "Join Waitlist",
          "Dashboard": "Dashboard",
          "Manage Bookings": "Manage Bookings",
          "Manage Services": "Manage Services",
          "Manage Staff": "Manage Staff",
          "Manage Waitlist": "Manage Waitlist",
          "Analytics": "Analytics & Reports"
        }
      },
      ar: {
        translation: {
          "Home": "الرئيسية",
          "Services": "الخدمات",
          "Staff": "الكادر",
          "Booking": "حجز موعد",
          "My Bookings": "حجوزاتي",
          "Loyalty": "برنامج الولاء",
          "Waitlist": "قائمة الانتظار",
          "Admin": "لوحة التحكم",
          "Book Now": "احجز الان",
          "Login": "تسجيل الدخول",
          "Logout": "تسجيل الخروج",
          "Register": "إنشاء حساب",
          "Hero Title": "تجربة الفخامة والاسترخاء المطلق في سبا عالي المستوى",
          "Hero Subtitle": "جدد نشاطك وجسمك مع أفضل العلاجات وأخصائيينا المتميزين في العراق.",
          "Our Offerings": "خدماتنا المتميزة",
          "Expert Therapists": "أخصائيون محترفون",
          "Expert Therapists Desc": "خبراء معتمدون مكرسون لتقديم أفضل تجربة علاجية وفاخرة.",
          "Premium Products": "منتجات عضوية فاخرة",
          "Premium Products Desc": "نستخدم أرقى الزيوت ومستحضرات العناية بالبشرة العضوية العالمية.",
          "Serene Environment": "أجواء هادئة ومريحة",
          "Serene Environment Desc": "ملاذ مخصص لتوفير الراحة والهدوء التام لجميع زوارنا.",
          "Category All": "جميع الخدمات",
          "Category HaircutStyling": "قص وتصفيف الشعر",
          "Category FacialSkincare": "العناية بالبشرة والوجه",
          "Category MassageTherapy": "العلاج بالتدليك",
          "Category NailCare": "العناية بالأظافر",
          "Category BodyTreatments": "علاجات الجسم",
          "Category Waxing": "إزالة الشعر",
          "Category MakeupBeauty": "المكياج والجمال",
          "Category EyelashEyebrow": "الرموش والحواجب",
          "Select Date": "اختر التاريخ",
          "Select Time Slot": "اختر الوقت المناسب",
          "Select Service": "اختر الخدمة",
          "Select Staff": "اختر الأخصائي",
          "Any Staff": "أي أخصائي متوفر",
          "Confirm Booking": "تأكيد الحجز",
          "Booking Success": "تم تأكيد حجزك بنجاح!",
          "Loyalty Points Balance": "رصيد نقاط الولاء",
          "Points": "نقطة",
          "Cancel Booking": "إلغاء الحجز",
          "Cancel Notice": "يمكن إلغاء الحجز قبل ساعتين على الأقل من الموعد المحدد.",
          "Join Waitlist": "الانضمام لقائمة الانتظار",
          "Dashboard": "لوحة القيادة",
          "Manage Bookings": "إدارة الحجوزات",
          "Manage Services": "إدارة الخدمات",
          "Manage Staff": "إدارة الكادر",
          "Manage Waitlist": "إدارة قائمة الانتظار",
          "Analytics": "التقارير والتحليلات"
        }
      }
    },
    interpolation: {
      escapeValue: false,
    }
  });

i18n.on('languageChanged', (lng) => {
  document.documentElement.dir = lng === 'ar' ? 'rtl' : 'ltr';
  document.documentElement.lang = lng;
});

export default i18n;

