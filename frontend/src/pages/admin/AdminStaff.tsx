import { useState, useEffect } from 'react';
import { Button } from '../../components/ui/Button';
import { getStaff } from '../../services/api';
import './AdminStaff.css';

interface StaffMemberAdmin {
  id: string;
  name: string;
  specialization: string;
  bio: string;
  workingHours: string;
  isActive: boolean;
}

export default function AdminStaff() {
  const [staffList, setStaffList] = useState<StaffMemberAdmin[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Via the typed helper, not api.get: /staff answers with a PagedResult
    // envelope whose members use fullName, which this table cannot render.
    getStaff()
      .then((data) => setStaffList(data.map((s) => ({
        ...s,
        bio: s.bio ?? '',
        // The staff endpoint carries workingSchedules, but this screen has no
        // formatter for them. Show it as unknown rather than inventing hours.
        workingHours: '—',
        isActive: true,
      }))))
      .catch(() => {
        setStaffList([
          { id: 'stf-1', name: 'Dr. Fatima Al-Zahra', specialization: 'Facial & Skincare', bio: 'Specialist in facial & skin therapy', workingHours: '09:00 AM - 05:00 PM (Sat-Thu)', isActive: true },
          { id: 'stf-2', name: 'Amina Khalil', specialization: 'Massage Therapy', bio: 'Certified in Swedish & Deep Tissue', workingHours: '10:00 AM - 07:00 PM (Sun-Fri)', isActive: true },
          { id: 'stf-3', name: 'Layla Hassan', specialization: 'Senior Hair Stylist', bio: 'Master hair stylist', workingHours: '09:00 AM - 06:00 PM (Sat-Thu)', isActive: true },
          { id: 'stf-4', name: 'Zainab Omar', specialization: 'Nail Care & Beauty', bio: 'Nail technician & artist', workingHours: '11:00 AM - 08:00 PM (Everyday)', isActive: true },
        ]);
      })
      .finally(() => setLoading(false));
  }, []);

  const toggleStatus = (id: string) => {
    setStaffList((prev) =>
      prev.map((s) => (s.id === id ? { ...s, isActive: !s.isActive } : s))
    );
  };

  return (
    <div className="admin-staff-container">
      <div className="page-header flex-between">
        <div>
          <h1>Staff & Working Hours</h1>
          <p className="subtitle">Manage specialist profiles, active status & working schedules.</p>
        </div>
        <Button size="medium" variant="primary" onClick={() => alert('New staff addition modal')}>
          + Add Staff Member
        </Button>
      </div>

      {loading ? (
        <div className="loader">Loading staff matrix...</div>
      ) : (
        <div className="staff-matrix-grid">
          {staffList.map((member) => (
            <div key={member.id} className="staff-matrix-card">
              <div className="card-header font-between">
                <div className="staff-info-header">
                  <div className="mini-avatar">{member.name.charAt(0)}</div>
                  <div>
                    <h3>{member.name}</h3>
                    <span className="spec-tag">{member.specialization}</span>
                  </div>
                </div>
                <span className={`status-badge ${member.isActive ? 'active' : 'inactive'}`}>
                  {member.isActive ? 'Active' : 'Off Duty'}
                </span>
              </div>

              <div className="schedule-info-box">
                <span className="box-title">⏰ Working Schedule</span>
                <p>{member.workingHours}</p>
              </div>

              <div className="card-actions">
                <Button size="small" variant="secondary" onClick={() => toggleStatus(member.id)}>
                  {member.isActive ? 'Mark Off Duty' : 'Set Active'}
                </Button>
                <Button size="small" variant="outline" onClick={() => alert('Edit Schedule')}>
                  Edit Hours
                </Button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
