import { useState } from 'react';
import { Outlet, Link, useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useAuthStore } from '../../services/useAuthStore';
import { AuthModal } from '../auth/AuthModal';
import './MainLayout.css';

export default function MainLayout() {
  const { t, i18n } = useTranslation();
  const navigate = useNavigate();
  const { isAuthenticated, user, logout } = useAuthStore();
  const [isAuthOpen, setIsAuthOpen] = useState(false);

  const toggleLang = () => {
    i18n.changeLanguage(i18n.language === 'en' ? 'ar' : 'en');
  };

  return (
    <div className="layout">
      <header className="navbar">
        <div className="logo" onClick={() => navigate('/')}>
          <span className="logo-sparkle">✦</span> SPA SALON
        </div>
        <nav className="nav-links">
          <Link to="/">{t('Home')}</Link>
          <Link to="/services">{t('Services')}</Link>
          <Link to="/staff">{t('Staff')}</Link>
          <Link to="/booking">{t('Booking')}</Link>
          {isAuthenticated && (
            <>
              <Link to="/my-bookings">{t('My Bookings')}</Link>
              <Link to="/loyalty">{t('Loyalty')}</Link>
              <Link to="/waitlist">{t('Waitlist')}</Link>
            </>
          )}
          {isAuthenticated && (user?.role === 'Admin' || user?.role === 'SuperAdmin') && (
            <Link to="/admin" className="admin-link">{t('Admin')}</Link>
          )}
        </nav>
        <div className="nav-actions">
          <button className="lang-btn" onClick={toggleLang}>
            {i18n.language === 'en' ? 'عربي' : 'EN'}
          </button>
          
          {isAuthenticated ? (
            <div className="user-profile-menu">
              <span className="user-name">👤 {user?.fullName}</span>
              {user?.loyaltyPoints !== undefined && (
                <span className="points-badge">⭐ {user.loyaltyPoints} pts</span>
              )}
              <button className="logout-btn" onClick={logout}>{t('Logout')}</button>
            </div>
          ) : (
            <button className="login-btn" onClick={() => setIsAuthOpen(true)}>
              {t('Login')}
            </button>
          )}
        </div>
      </header>

      <main className="content">
        <Outlet />
      </main>

      <footer className="footer">
        <div className="footer-container">
          <p>&copy; {new Date().getFullYear()} SPA Salon Iraq. All rights reserved.</p>
          <p className="footer-sub">Luxury Calendar-Based Reservation & Relaxation Platform</p>
        </div>
      </footer>

      <AuthModal isOpen={isAuthOpen} onClose={() => setIsAuthOpen(false)} />
    </div>
  );
}
