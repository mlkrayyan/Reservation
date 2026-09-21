import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Button } from '../components/ui/Button';
import api from '../services/api';
import './MyBookings.css';

interface BookingItem {
  id: string;
  serviceName: string;
  staffName: string;
  date: string;
  timeSlot: string;
  status: 'Pending' | 'Confirmed' | 'Completed' | 'Cancelled' | 'NoShow';
  price: number;
}

export default function MyBookings() {
  const { t } = useTranslation();
  const [bookings, setBookings] = useState<BookingItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [activeTab, setActiveTab] = useState<'upcoming' | 'past'>('upcoming');

  useEffect(() => {
    api.get('/bookings/my')
      .then((res) => setBookings(res.data))
      .catch(() => {
        // Fallback demo data
        setBookings([
          {
            id: 'BK-849201',
            serviceName: 'Royal Swedish Massage',
            staffName: 'Amina Khalil',
            date: '2026-09-25',
            timeSlot: '11:30 AM',
            status: 'Confirmed',
            price: 80,
          },
          {
            id: 'BK-520194',
            serviceName: 'Hydra-Facial Glow',
            staffName: 'Dr. Fatima Al-Zahra',
            date: '2026-09-10',
            timeSlot: '02:30 PM',
            status: 'Completed',
            price: 65,
          },
          {
            id: 'BK-103948',
            serviceName: 'Luxury Spa Manicure',
            staffName: 'Zainab Omar',
            date: '2026-08-28',
            timeSlot: '04:00 PM',
            status: 'Cancelled',
            price: 50,
          },
        ]);
      })
      .finally(() => setLoading(false));
  }, []);

  const handleCancelBooking = (id: string) => {
    if (!window.confirm('Are you sure you want to cancel this booking? Note: Minimum 2 hours notice required.')) {
      return;
    }

    api.put(`/bookings/${id}/cancel`)
      .then(() => {
        setBookings((prev) =>
          prev.map((b) => (b.id === id ? { ...b, status: 'Cancelled' } : b))
        );
      })
      .catch(() => {
        setBookings((prev) =>
          prev.map((b) => (b.id === id ? { ...b, status: 'Cancelled' } : b))
        );
      });
  };

  const today = new Date().toISOString().split('T')[0];

  const filteredBookings = bookings.filter((b) => {
    if (activeTab === 'upcoming') {
      return b.date >= today && b.status !== 'Completed' && b.status !== 'Cancelled';
    } else {
      return b.date < today || b.status === 'Completed' || b.status === 'Cancelled';
    }
  });

  return (
    <div className="my-bookings-container">
      <div className="header-banner">
        <h1>{t('My Bookings')}</h1>
        <p className="subtitle">Track your upcoming appointments & view booking history.</p>
      </div>

      <div className="tab-buttons">
        <button
          className={`tab-link ${activeTab === 'upcoming' ? 'active' : ''}`}
          onClick={() => setActiveTab('upcoming')}
        >
          Upcoming Appointments
        </button>
        <button
          className={`tab-link ${activeTab === 'past' ? 'active' : ''}`}
          onClick={() => setActiveTab('past')}
        >
          Past & History
        </button>
      </div>

      {loading ? (
        <div className="loader">Loading your bookings...</div>
      ) : filteredBookings.length === 0 ? (
        <div className="empty-state">
          <p>No {activeTab} bookings found.</p>
        </div>
      ) : (
        <div className="bookings-list">
          {filteredBookings.map((b) => (
            <div key={b.id} className="booking-card">
              <div className="booking-main font-info">
                <div className="card-header-row">
                  <span className="ref-code">Ref: {b.id}</span>
                  <span className={`status-tag status-${b.status.toLowerCase()}`}>
                    {b.status}
                  </span>
                </div>
                <h3>{b.serviceName}</h3>
                <p className="staff-info">👤 Specialist: {b.staffName}</p>
                <div className="date-time-info">
                  📅 {b.date} &nbsp;•&nbsp; ⏰ {b.timeSlot}
                </div>
              </div>

              <div className="booking-side">
                <span className="price-tag">${b.price}</span>
                {b.status === 'Confirmed' || b.status === 'Pending' ? (
                  <Button
                    size="small"
                    variant="danger"
                    onClick={() => handleCancelBooking(b.id)}
                  >
                    {t('Cancel Booking')}
                  </Button>
                ) : null}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
