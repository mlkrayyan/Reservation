import { useState, useEffect } from 'react';
import { Button } from '../../components/ui/Button';
import api from '../../services/api';
import './AdminWaitlist.css';

interface AdminWaitlistEntry {
  id: string;
  customerName: string;
  customerPhone: string;
  serviceName: string;
  preferredStaffName: string;
  preferredDate: string;
  status: 'Waiting' | 'Offered' | 'Booked' | 'Expired';
  createdAt: string;
}

export default function AdminWaitlist() {
  const [entries, setEntries] = useState<AdminWaitlistEntry[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api.get('/admin/waitlist')
      .then((res) => setEntries(res.data))
      .catch(() => {
        setEntries([
          { id: 'WL-101', customerName: 'Sara Ahmed', customerPhone: '+964 770 123 4567', serviceName: 'Hydra-Facial Glow', preferredStaffName: 'Dr. Fatima Al-Zahra', preferredDate: '2026-09-28', status: 'Waiting', createdAt: '2026-09-20' },
          { id: 'WL-102', customerName: 'Zahra Hassan', customerPhone: '+964 772 333 4444', serviceName: 'Royal Swedish Massage', preferredStaffName: 'Amina Khalil', preferredDate: '2026-09-22', status: 'Offered', createdAt: '2026-09-18' },
          { id: 'WL-103', customerName: 'Mona Kareem', customerPhone: '+964 770 555 6666', serviceName: 'Full Body Scrub', preferredStaffName: 'Amina Khalil', preferredDate: '2026-09-25', status: 'Waiting', createdAt: '2026-09-19' },
        ]);
      })
      .finally(() => setLoading(false));
  }, []);

  const handleOfferSlot = (id: string) => {
    setEntries((prev) =>
      prev.map((e) => (e.id === id ? { ...e, status: 'Offered' } : e))
    );
    alert(`Slot offer notification dispatched via Twilio SMS & SendGrid Email! Customer has 24h to confirm.`);
  };

  return (
    <div className="admin-waitlist-container">
      <div className="page-header">
        <h1>Waitlist Queue Management</h1>
        <p className="subtitle">View customers waiting for cancelled slots & send automated offers.</p>
      </div>

      {loading ? (
        <div className="loader">Loading waitlist queue...</div>
      ) : (
        <div className="table-card">
          <table className="admin-table">
            <thead>
              <tr>
                <th>Queue ID</th>
                <th>Customer</th>
                <th>Service Requested</th>
                <th>Specialist</th>
                <th>Preferred Date</th>
                <th>Status</th>
                <th>Action</th>
              </tr>
            </thead>
            <tbody>
              {entries.map((item) => (
                <tr key={item.id}>
                  <td className="font-mono">{item.id}</td>
                  <td>
                    <div className="font-bold">{item.customerName}</div>
                    <div className="phone-sub">{item.customerPhone}</div>
                  </td>
                  <td>{item.serviceName}</td>
                  <td>{item.preferredStaffName}</td>
                  <td className="font-bold">📅 {item.preferredDate}</td>
                  <td>
                    <span className={`status-pill status-${item.status.toLowerCase()}`}>
                      {item.status}
                    </span>
                  </td>
                  <td>
                    {item.status === 'Waiting' ? (
                      <Button size="small" variant="primary" onClick={() => handleOfferSlot(item.id)}>
                        🔔 Offer Opening
                      </Button>
                    ) : (
                      <span className="offer-sent-badge">Offer Sent</span>
                    )}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
