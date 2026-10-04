import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { Button } from '../components/ui/Button';
import heroBeauty from '../assets/hero-beauty.jpg';
import './Home.css';

export default function Home() {
  const navigate = useNavigate();
  const { t } = useTranslation();

  return (
    <div className="home-container">
      {/* Hero Section */}
      <section className="hero">
        <div className="hero-copy">
          <span className="hero-badge">✦ LUXURY SPA & SALON IRAQ ✦</span>
          <h1>{t('Hero Title')}</h1>
          <p>{t('Hero Subtitle')}</p>
          <div className="hero-cta-group">
            <Button size="large" onClick={() => navigate('/booking')}>
              {t('Book Now')}
            </Button>
            <Button size="large" variant="outline" onClick={() => navigate('/services')}>
              {t('Category All')}
            </Button>
          </div>
        </div>
        <div className="hero-media">
          <img
            className="hero-image"
            src={heroBeauty}
            alt="Close-up of a model wearing full makeup, with luminous highlighted skin"
          />
        </div>
      </section>

      {/* Offerings Showcase */}
      <section className="features">
        <h2 className="section-title">{t('Our Offerings')}</h2>
        <div className="feature-grid">
          <div className="feature-card">
            <div className="card-icon">🧘‍♀️</div>
            <h3>{t('Expert Therapists')}</h3>
            <p>{t('Expert Therapists Desc')}</p>
          </div>
          <div className="feature-card">
            <div className="card-icon">🌿</div>
            <h3>{t('Premium Products')}</h3>
            <p>{t('Premium Products Desc')}</p>
          </div>
          <div className="feature-card">
            <div className="card-icon">✨</div>
            <h3>{t('Serene Environment')}</h3>
            <p>{t('Serene Environment Desc')}</p>
          </div>
        </div>
      </section>

      {/* CTA Section */}
      <section className="cta-banner">
        <div className="cta-box">
          <h2>Ready for an Unforgettable Spa Experience?</h2>
          <p>Select your preferred treatment date, specialist, and time slot seamlessly online.</p>
          <Button size="large" variant="primary" onClick={() => navigate('/booking')}>
            {t('Book Appointment')}
          </Button>
        </div>
      </section>
    </div>
  );
}
