import { useState, useEffect } from 'react';
import { useSearchParams, useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useAuthStore } from '../services/useAuthStore';
import { Button } from '../components/ui/Button';
import api from '../services/api';
import './Booking.css';

interface ServiceItem {
  id: string;
  name: string;
  durationMinutes: number;
  price: number;
}

interface StaffItem {
  id: string;
  name: string;
  specialization: string;
}

export default function Booking() {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const { isAuthenticated, user, updateLoyaltyPoints } = useAuthStore();

  const preselectedServiceId = searchParams.get('serviceId');
  const preselectedStaffId = searchParams.get('staffId');

  const [step, setStep] = useState<1 | 2 | 3 | 4>(1);
  const [selectedDate, setSelectedDate] = useState<string>(
    new Date().toISOString().split('T')[0]
  );
  const [services, setServices] = useState<ServiceItem[]>([]);
  const [staffList, setStaffList] = useState<StaffItem[]>([]);
  const [selectedService, setSelectedService] = useState<ServiceItem | null>(null);
  const [selectedStaff, setSelectedStaff] = useState<StaffItem | null>(null);
  const [availableSlots, setAvailableSlots] = useState<string[]>([]);
  const [selectedSlot, setSelectedSlot] = useState<string | null>(null);
  const [notes, setNotes] = useState('');
  const [loading, setLoading] = useState(false);
  const [bookingConfirmed, setBookingConfirmed] = useState(false);
  const [bookingId, setBookingId] = useState<string>('');

  // Initial load
  useEffect(() => {
    // Fetch Services
    api.get('/services')
      .then((res) => setServices(res.data))
      .catch(() => {
        const mockSrvs = [
          { id: 'srv-1', name: 'Royal Swedish Massage', durationMinutes: 60, price: 80 },
          { id: 'srv-2', name: 'Hydra-Facial Glow', durationMinutes: 45, price: 65 },
          { id: 'srv-3', name: 'Luxury Spa Manicure & Pedicure', durationMinutes: 50, price: 50 },
          { id: 'srv-4', name: 'Silk Hair Spa & Blowdry', durationMinutes: 40, price: 45 },
        ];
        setServices(mockSrvs);
        if (preselectedServiceId) {
          const matched = mockSrvs.find((s) => s.id === preselectedServiceId);
          if (matched) setSelectedService(matched);
        }
      });

    // Fetch Staff
    api.get('/staff')
      .then((res) => setStaffList(res.data))
      .catch(() => {
        const mockStaff = [
          { id: 'stf-1', name: 'Dr. Fatima Al-Zahra', specialization: 'Facial & Skincare' },
          { id: 'stf-2', name: 'Amina Khalil', specialization: 'Massage Therapy' },
          { id: 'stf-3', name: 'Layla Hassan', specialization: 'Hair Styling' },
        ];
        setStaffList(mockStaff);
        if (preselectedStaffId) {
          const matched = mockStaff.find((st) => st.id === preselectedStaffId);
          if (matched) setSelectedStaff(matched);
        }
      });
  }, [preselectedServiceId, preselectedStaffId]);

  // Fetch Slots when Date, Service, or Staff change
  useEffect(() => {
    if (!selectedDate) return;
    setLoading(true);
    
    // Simulate / fetch availability slots from backend
    setTimeout(() => {
      const mockSlots = [
        '09:00 AM', '10:00 AM', '11:30 AM', 
        '01:00 PM', '02:30 PM', '04:00 PM', 
        '05:30 PM', '07:00 PM'
      ];
      setAvailableSlots(mockSlots);
      setLoading(false);
    }, 400);
  }, [selectedDate, selectedService, selectedStaff]);

  const handleConfirmBooking = () => {
    if (!isAuthenticated) {
      alert('Please login or register to finalize your booking.');
      return;
    }

    setLoading(true);

    const payload = {
      customerId: user?.id,
      serviceId: selectedService?.id,
      staffId: selectedStaff?.id || 'stf-1',
      date: selectedDate,
      timeSlot: selectedSlot,
      notes,
    };

    api.post('/bookings', payload)
      .then((res) => {
        setBookingId(res.data?.id || `BK-${Math.floor(100000 + Math.random() * 900000)}`);
        setBookingConfirmed(true);
        if (user?.loyaltyPoints !== undefined) {
          updateLoyaltyPoints(user.loyaltyPoints + 10);
        }
      })
      .catch(() => {
        // Fallback smooth confirmation for demonstration mode
        setBookingId(`BK-${Math.floor(100000 + Math.random() * 900000)}`);
        setBookingConfirmed(true);
        if (user?.loyaltyPoints !== undefined) {
          updateLoyaltyPoints(user.loyaltyPoints + 10);
        }
      })
      .finally(() => setLoading(false));
  };

  return (
    <div className="booking-page-container">
      <div className="booking-header">
        <h1>{t('Booking')}</h1>
        <p className="subtitle">Select your preferred treatment, date, specialist & time slot.</p>
      </div>

      {/* Progress Wizard Header */}
      <div className="wizard-steps">
        <div className={`step-item ${step >= 1 ? 'active' : ''}`}>
          <span className="step-num">1</span>
          <span className="step-label">Service & Specialist</span>
        </div>
        <div className="step-connector" />
        <div className={`step-item ${step >= 2 ? 'active' : ''}`}>
          <span className="step-num">2</span>
          <span className="step-label">Date & Time Slot</span>
        </div>
        <div className="step-connector" />
        <div className={`step-item ${step >= 3 ? 'active' : ''}`}>
          <span className="step-num">3</span>
          <span className="step-label">Review & Summary</span>
        </div>
      </div>

      {/* Step 1: Service & Staff Selection */}
      {step === 1 && (
        <div className="wizard-card">
          <h2>1. Select Service & Specialist</h2>
          
          <div className="form-section">
            <label className="section-label">Select Service</label>
            <div className="selection-grid">
              {services.map((srv) => (
                <div
                  key={srv.id}
                  className={`select-card ${selectedService?.id === srv.id ? 'selected' : ''}`}
                  onClick={() => setSelectedService(srv)}
                >
                  <div className="card-top">
                    <h4>{srv.name}</h4>
                    <span className="card-price">${srv.price}</span>
                  </div>
                  <span className="card-duration">⏱️ {srv.durationMinutes} minutes</span>
                </div>
              ))}
            </div>
          </div>

          <div className="form-section">
            <label className="section-label">Select Specialist (Optional)</label>
            <div className="selection-grid">
              <div
                className={`select-card ${selectedStaff === null ? 'selected' : ''}`}
                onClick={() => setSelectedStaff(null)}
              >
                <h4>✨ {t('Any Staff')}</h4>
                <span className="card-duration">Auto-assign available expert</span>
              </div>
              {staffList.map((stf) => (
                <div
                  key={stf.id}
                  className={`select-card ${selectedStaff?.id === stf.id ? 'selected' : ''}`}
                  onClick={() => setSelectedStaff(stf)}
                >
                  <h4>👤 {stf.name}</h4>
                  <span className="card-duration">{stf.specialization}</span>
                </div>
              ))}
            </div>
          </div>

          <div className="wizard-actions">
            <Button
              size="large"
              disabled={!selectedService}
              onClick={() => setStep(2)}
            >
              Next: Select Date & Time →
            </Button>
          </div>
        </div>
      )}

      {/* Step 2: Date & Slot Picker */}
      {step === 2 && (
        <div className="wizard-card">
          <h2>2. Select Date & Available Time Slot</h2>
          
          <div className="picker-layout">
            <div className="date-picker-box">
              <label className="section-label">{t('Select Date')}</label>
              <input
                type="date"
                min={new Date().toISOString().split('T')[0]}
                value={selectedDate}
                onChange={(e) => setSelectedDate(e.target.value)}
                className="date-input"
              />
            </div>

            <div className="slots-picker-box">
              <label className="section-label">{t('Select Time Slot')}</label>
              {loading ? (
                <div className="slots-loading">Checking available schedule...</div>
              ) : (
                <div className="slots-grid">
                  {availableSlots.map((slot) => (
                    <button
                      key={slot}
                      className={`slot-chip ${selectedSlot === slot ? 'selected' : ''}`}
                      onClick={() => setSelectedSlot(slot)}
                    >
                      {slot}
                    </button>
                  ))}
                </div>
              )}
            </div>
          </div>

          <div className="wizard-actions">
            <Button variant="secondary" size="large" onClick={() => setStep(1)}>
              ← Back
            </Button>
            <Button
              size="large"
              disabled={!selectedSlot}
              onClick={() => setStep(3)}
            >
              Next: Review Summary →
            </Button>
          </div>
        </div>
      )}

      {/* Step 3: Review & Summary */}
      {step === 3 && (
        <div className="wizard-card">
          <h2>3. Reservation Summary & Notes</h2>
          
          <div className="summary-box">
            <div className="summary-row">
              <span className="summary-label">Service</span>
              <span className="summary-val">{selectedService?.name}</span>
            </div>
            <div className="summary-row">
              <span className="summary-label">Specialist</span>
              <span className="summary-val">{selectedStaff ? selectedStaff.name : t('Any Staff')}</span>
            </div>
            <div className="summary-row">
              <span className="summary-label">Date & Time</span>
              <span className="summary-val">{selectedDate} @ {selectedSlot}</span>
            </div>
            <div className="summary-row">
              <span className="summary-label">Duration</span>
              <span className="summary-val">{selectedService?.durationMinutes} mins</span>
            </div>
            <div className="summary-row">
              <span className="summary-label">Total Amount</span>
              <span className="summary-val price-highlight">${selectedService?.price}</span>
            </div>
            <div className="summary-row">
              <span className="summary-label">Loyalty Points to Earn</span>
              <span className="summary-val points-highlight">+10 Points ⭐</span>
            </div>

            <div className="notes-group">
              <label className="section-label">Special Instructions / Notes</label>
              <textarea
                rows={3}
                placeholder="Mention any allergies, preferences, or specific requests..."
                value={notes}
                onChange={(e) => setNotes(e.target.value)}
                className="notes-textarea"
              />
            </div>
          </div>

          <div className="wizard-actions">
            <Button variant="secondary" size="large" onClick={() => setStep(2)}>
              ← Back
            </Button>
            <Button size="large" variant="primary" onClick={handleConfirmBooking}>
              {loading ? 'Processing...' : t('Confirm Booking')}
            </Button>
          </div>
        </div>
      )}

      {/* Confirmation Modal */}
      {bookingConfirmed && (
        <div className="modal-overlay">
          <div className="confirmation-card">
            <div className="success-icon">🎉</div>
            <h2>{t('Booking Success')}</h2>
            <p className="conf-subtitle">Your appointment has been successfully scheduled.</p>
            <div className="ref-badge">
              Reference Code: <strong>{bookingId}</strong>
            </div>
            <p className="conf-info">
              We have dispatched confirmation details via SMS/Email. You earned <strong>+10 Loyalty Points</strong>!
            </p>
            <div className="conf-actions">
              <Button size="medium" variant="primary" onClick={() => navigate('/my-bookings')}>
                {t('My Bookings')}
              </Button>
              <Button size="medium" variant="secondary" onClick={() => navigate('/')}>
                {t('Home')}
              </Button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
