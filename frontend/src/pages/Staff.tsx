import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { getStaff, type Staff } from '../services/api';
import { Card } from '../components/ui/Card';
import { Button } from '../components/ui/Button';
import './Staff.css';

export default function StaffPage() {
  const { t } = useTranslation();
  const navigate = useNavigate();
  const [staff, setStaff] = useState<Staff[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    getStaff()
      .then((data) => setStaff(data))
      .catch((err) => {
        console.warn('API connection offline, using staff showcase fallback data', err);
        setStaff([
          { id: 'stf-1', name: 'Dr. Fatima Al-Zahra', specialization: 'Facial & Skincare Specialist', bio: '10+ years experience in medical skincare & anti-aging treatments.' },
          { id: 'stf-2', name: 'Amina Khalil', specialization: 'Lead Massage Therapist', bio: 'Certified in Swedish, Deep Tissue, and Aromatherapy techniques.' },
          { id: 'stf-3', name: 'Layla Hassan', specialization: 'Senior Hair Stylist', bio: 'Specializes in modern hair coloring, styling, and keratherapies.' },
          { id: 'stf-4', name: 'Zainab Omar', specialization: 'Nail Artist & Esthetician', bio: 'Master in gel extensions, nail art, and hand care treatments.' },
        ]);
      })
      .finally(() => setLoading(false));
  }, []);

  return (
    <div className="staff-page-container">
      <div className="header-banner">
        <h1>{t('Staff')}</h1>
        <p className="subtitle">Meet our team of world-class certified spa professionals dedicated to your care.</p>
      </div>

      {loading ? (
        <div className="loader">Loading staff profiles...</div>
      ) : (
        <div className="staff-grid">
          {staff.map((member: any) => (
            <Card key={member.id} className="staff-card">
              <div className="avatar-wrapper">
                <div className="avatar">
                  {member.name.charAt(0)}
                </div>
                <span className="status-dot" title="Available" />
              </div>
              <h3>{member.name}</h3>
              <p className="spec">{member.specialization}</p>
              {member.bio && <p className="bio">{member.bio}</p>}
              <Button 
                size="small" 
                variant="outline" 
                style={{ marginTop: '1rem', width: '100%' }}
                onClick={() => navigate(`/booking?staffId=${member.id}`)}
              >
                {t('Book Appointment')}
              </Button>
            </Card>
          ))}
        </div>
      )}
    </div>
  );
}
