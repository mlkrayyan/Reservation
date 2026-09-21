import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { getServices, type Service } from '../services/api';
import { Card } from '../components/ui/Card';
import { Button } from '../components/ui/Button';
import './Services.css';

const CATEGORIES = [
  'Category All',
  'HaircutStyling',
  'FacialSkincare',
  'MassageTherapy',
  'NailCare',
  'BodyTreatments',
  'Waxing',
  'MakeupBeauty',
  'EyelashEyebrow'
];

export default function Services() {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const [services, setServices] = useState<Service[]>([]);
  const [loading, setLoading] = useState(true);
  const [selectedCategory, setSelectedCategory] = useState('Category All');

  useEffect(() => {
    getServices()
      .then((data) => {
        setServices(data);
      })
      .catch((err) => {
        console.warn('API connection offline, utilizing fallback services catalog', err);
        // Robust fallback data matching design spec bounded context
        setServices([
          { id: 'srv-1', name: 'Royal Swedish Massage', description: 'Deep muscle relaxation treatment using essential lavender oils.', durationMinutes: 60, price: 80, category: 'MassageTherapy' },
          { id: 'srv-2', name: 'Hydra-Facial Glow', description: 'Deep cleansing, exfoliation, and hydration for radiant skin.', durationMinutes: 45, price: 65, category: 'FacialSkincare' },
          { id: 'srv-3', name: 'Luxury Spa Manicure & Pedicure', description: 'Nail shaping, cuticle care, scrub, and luxury polish.', durationMinutes: 50, price: 50, category: 'NailCare' },
          { id: 'srv-4', name: 'Silk Hair Spa & Blowdry', description: 'Nourishing hair mask treatment followed by professional styling.', durationMinutes: 40, price: 45, category: 'HaircutStyling' },
          { id: 'srv-5', name: 'Full Body Scrub & Wrap', description: 'Dead Sea salt exfoliation with detoxifying clay wrap.', durationMinutes: 75, price: 110, category: 'BodyTreatments' },
          { id: 'srv-6', name: 'Eyelash Extensions Classic', description: 'Full set of premium lightweight eyelash extensions.', durationMinutes: 90, price: 95, category: 'EyelashEyebrow' },
        ]);
      })
      .finally(() => setLoading(false));
  }, []);

  const filteredServices = selectedCategory === 'Category All'
    ? services
    : services.filter((s: any) => s.category === selectedCategory || selectedCategory.includes(s.category));

  return (
    <div className="services-page-container">
      <div className="header-banner">
        <h1>{t('Services')}</h1>
        <p className="subtitle">Discover our luxury spa treatments tailored for your ultimate relaxation.</p>
      </div>

      {/* Category Tabs */}
      <div className="category-tabs">
        {CATEGORIES.map((cat) => (
          <button
            key={cat}
            className={`tab-btn ${selectedCategory === cat ? 'active' : ''}`}
            onClick={() => setSelectedCategory(cat)}
          >
            {t(cat)}
          </button>
        ))}
      </div>

      {loading ? (
        <div className="loader">Loading services catalog...</div>
      ) : (
        <div className="service-grid">
          {filteredServices.map((service) => (
            <Card key={service.id} className="service-card">
              <div className="service-header">
                <h3>{service.name}</h3>
                <span className="price-tag">${service.price}</span>
              </div>
              <p className="desc">{service.description}</p>
              <div className="service-footer">
                <span className="duration">⏱️ {service.durationMinutes} mins</span>
                <Button 
                  size="small" 
                  variant="primary" 
                  onClick={() => navigate(`/booking?serviceId=${service.id}`)}
                >
                  {t('Book Now')}
                </Button>
              </div>
            </Card>
          ))}
        </div>
      )}
    </div>
  );
}
