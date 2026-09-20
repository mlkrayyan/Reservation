import { Outlet, Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import './MainLayout.css'; // Optional local CSS if needed

export default function MainLayout() {
  const { t, i18n } = useTranslation();

  const toggleLang = () => {
    i18n.changeLanguage(i18n.language === 'en' ? 'ar' : 'en');
  };

  return (
    <div className="layout">
      <header className="navbar">
        <div className="logo">SPA SALON</div>
        <nav className="nav-links">
          <Link to="/">{t('Home')}</Link>
          <Link to="/services">{t('Services')}</Link>
          <Link to="/staff">{t('Staff')}</Link>
          <Link to="/booking">{t('Book Now')}</Link>
        </nav>
        <div className="nav-actions">
          <button className="lang-btn" onClick={toggleLang}>
            {i18n.language === 'en' ? 'عربي' : 'EN'}
          </button>
          <Link to="/login" className="login-btn">{t('Login')}</Link>
        </div>
      </header>

      <main className="content">
        <Outlet />
      </main>

      <footer className="footer">
        <p>&copy; {new Date().getFullYear()} Spa Salon. All rights reserved.</p>
      </footer>
    </div>
  );
}
