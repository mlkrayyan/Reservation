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
          "Book Now": "Book Now",
          "Login": "Login"
        }
      },
      ar: {
        translation: {
          "Home": "الرئيسية",
          "Services": "الخدمات",
          "Staff": "الكادر",
          "Book Now": "احجز الان",
          "Login": "تسجيل الدخول"
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
