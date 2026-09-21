import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Button } from '../components/ui/Button';
import api from '../services/api';
import './Waitlist.css';

interface WaitlistEntryItem {
  id: string;
  serviceName: string;
  preferredStaffName?: string;
  preferredDate: string;
  status: 'Waiting' | 'Offered' | 'Booked' | 'Expired';
  createdAt: string;
}

export default function Waitlist() {
  const { t } = useTranslation();
  const [entries, setEntries] = useState<WaitlistEntryItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [showJoinModal, setShowJoinModal] = useState(false);

  // Form state
  const [serviceName, setServiceName] = useState('Royal Swedish Massage');
  const [preferredDate, setPreferredDate] = useState(new Date().toISOString().split('T')[0]);

  useEffect(() => {
    api.get('/waitlist/my')
      .then((res) => setEntries(res.data))
      .catch(() => {
        // Fallback demo data
        setEntries([
          {
            id: 'wl-901',
            serviceName: 'Hydra-Facial Glow',
            preferredStaffName: 'Dr. Fatima Al-Zahra',
            preferredDate: '2026-09-28',
            status: 'Waiting',
            createdAt: '2026-09-20',
          },
          {
            id: 'wl-402',
            serviceName: 'Full Body Scrub',
            preferredStaffName: 'Amina Khalil',
            preferredDate: '2026-09-22',
            status: 'Offered',
            createdAt: '2026-09-18',
          },
        ]);
      })
      .finally(() => setLoading(false));
  }, []);

  const handleLeaveWaitlist = (id: string) => {
    if (!window.confirm('Leave this waitlist entry?')) return;

    api.delete(`/waitlist/${id}`)
      .then(() => setEntries((prev) => prev.filter((e) => e.id !== id)))
      .catch(() => setEntries((prev) => prev.filter((e) => e.id !== id)));
  };

  const handleJoinSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const newEntry: WaitlistEntryItem = {
      id: `wl-${Math.floor(100 + Math.random() * 900)}`,
      serviceName,
      preferredStaffName: 'Any Available Specialist',
      preferredDate,
      status: 'Waiting',
      createdAt: new Date().toISOString().split('T')[0],
    };

    api.post('/waitlist', newEntry)
      .then(() => {
        setEntries((prev) => [newEntry, ...prev]);
        setShowJoinModal(false);
      })
      .catch(() => {
        setEntries((prev) => [newEntry, ...prev]);
        setShowJoinModal(false);
      });
  };

  return (
    <div className="waitlist-page-container">
      <div className="header-banner flex-header">
        <div>
          <h1>{t('Waitlist')}</h1>
          <p className="subtitle">Get notified instantly when fully booked time slots open up.</p>
        </div>
        <Button size="medium" onClick={() => setShowJoinModal(true)}>
          + {t('Join Waitlist')}
        </Button>
      </div>

      {loading ? (
        <div className="loader">Loading your waitlist entries...</div>
      ) : entries.length === 0 ? (
        <div className="empty-state">
          <p>You have no active waitlist requests.</p>
        </div>
      ) : (
        <div className="waitlist-grid">
          {entries.map((item) => (
            <div key={item.id} className="waitlist-card">
              <div className="card-top">
                <span className={`status-pill status-${item.status.toLowerCase()}`}>
                  {item.status}
                </span>
                <span className="date-badge">Joined {item.createdAt}</span>
              </div>
              <h3>{item.serviceName}</h3>
              <p className="pref-info">👤 Preferred Specialist: {item.preferredStaffName || 'Any'}</p>
              <p className="pref-date">📅 Desired Date: <strong>{item.preferredDate}</strong></p>

              {item.status === 'Offered' && (
                <div className="offer-alert">
                  🔔 A slot became available! You have 24 hours to claim this appointment.
                </div>
              )}

              <Button
                size="small"
                variant="danger"
                style={{ marginTop: '1rem', width: '100%' }}
                onClick={() => handleLeaveWaitlist(item.id)}
              >
                Leave Waitlist
              </Button>
            </div>
          ))}
        </div>
      )}

      {/* Join Waitlist Modal */}
      {showJoinModal && (
        <div className="modal-overlay" onClick={() => setShowJoinModal(false)}>
          <div className="waitlist-modal-card" onClick={(e) => e.stopPropagation()}>
            <button className="close-btn" onClick={() => setShowJoinModal(false)}>&times;</button>
            <h2>Join Salon Waitlist</h2>
            <p className="subtitle">We will notify you via SMS/Email as soon as a slot opens.</p>

            <form onSubmit={handleJoinSubmit} className="waitlist-form">
              <div className="input-group">
                <label>Select Service</label>
                <select value={serviceName} onChange={(e) => setServiceName(e.target.value)}>
                  <option value="Royal Swedish Massage">Royal Swedish Massage</option>
                  <option value="Hydra-Facial Glow">Hydra-Facial Glow</option>
                  <option value="Luxury Spa Manicure & Pedicure">Luxury Spa Manicure & Pedicure</option>
                  <option value="Silk Hair Spa & Blowdry">Silk Hair Spa & Blowdry</option>
                </select>
              </div>

              <div className="input-group">
                <label>Preferred Date</label>
                <input
                  type="date"
                  required
                  min={new Date().toISOString().split('T')[0]}
                  value={preferredDate}
                  onChange={(e) => setPreferredDate(e.target.value)}
                />
              </div>

              <Button type="submit" size="large" variant="primary" style={{ width: '100%' }}>
                Submit Waitlist Request
              </Button>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
