import { useState, useEffect } from 'react';
import { Button } from '../../components/ui/Button';
import api from '../../services/api';
import './AdminBookings.css';

interface AdminBookingItem {
  id: string;
  customerName: string;
  customerPhone: string;
  serviceName: string;
  staffName: string;
  date: string;
  timeSlot: string;
  status: 'Pending' | 'Confirmed' | 'Completed' | 'Cancelled' | 'NoShow';
  price: number;
}

export default function AdminBookings() {
  const [bookings, setBookings] = useState<AdminBookingItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [filterStatus, setFilterStatus] = useState<string>('All');
  const [searchQuery, setSearchQuery] = useState('');

  useEffect(() => {
    api.get('/admin/bookings')
      .then((res) => setBookings(res.data))
      .catch(() => {
        // Fallback admin bookings dataset
        setBookings([
          { id: 'BK-90412', customerName: 'Sara Ahmed', customerPhone: '+964 770 123 4567', serviceName: 'Royal Swedish Massage', staffName: 'Amina Khalil', date: '2026-09-21', timeSlot: '09:00 AM', status: 'Confirmed', price: 80 },
          { id: 'BK-90413', customerName: 'Noor Ali', customerPhone: '+964 771 987 6543', serviceName: 'Hydra-Facial Glow', staffName: 'Dr. Fatima Al-Zahra', date: '2026-09-21', timeSlot: '10:30 AM', status: 'Completed', price: 65 },
          { id: 'BK-90414', customerName: 'Zahra Hassan', customerPhone: '+964 772 333 4444', serviceName: 'Silk Hair Spa & Blowdry', staffName: 'Layla Hassan', date: '2026-09-21', timeSlot: '11:30 AM', status: 'Confirmed', price: 45 },
          { id: 'BK-90415', customerName: 'Mona Kareem', customerPhone: '+964 770 555 6666', serviceName: 'Luxury Spa Manicure', staffName: 'Zainab Omar', date: '2026-09-21', timeSlot: '02:00 PM', status: 'Pending', price: 50 },
          { id: 'BK-90416', customerName: 'Huda Jasim', customerPhone: '+964 773 111 2222', serviceName: 'Eyelash Extensions Classic', staffName: 'Zainab Omar', date: '2026-09-22', timeSlot: '01:00 PM', status: 'Confirmed', price: 95 },
        ]);
      })
      .finally(() => setLoading(false));
  }, []);

  const updateBookingStatus = (id: string, newStatus: AdminBookingItem['status']) => {
    const endpointMap: Record<string, string> = {
      Confirmed: `/admin/bookings/${id}/confirm`,
      Completed: `/admin/bookings/${id}/complete`,
      Cancelled: `/bookings/${id}/cancel`,
      NoShow: `/admin/bookings/${id}/no-show`,
    };

    const url = endpointMap[newStatus];
    if (url) {
      api.put(url).catch(() => {});
    }

    setBookings((prev) =>
      prev.map((item) => (item.id === id ? { ...item, status: newStatus } : item))
    );
  };

  const filtered = bookings.filter((b) => {
    const matchesStatus = filterStatus === 'All' || b.status === filterStatus;
    const matchesSearch =
      b.customerName.toLowerCase().includes(searchQuery.toLowerCase()) ||
      b.id.toLowerCase().includes(searchQuery.toLowerCase()) ||
      b.serviceName.toLowerCase().includes(searchQuery.toLowerCase());
    return matchesStatus && matchesSearch;
  });

  return (
    <div className="admin-bookings-container">
      <div className="page-header">
        <div>
          <h1>Bookings Management</h1>
          <p className="subtitle">View all customer appointments, confirm bookings & mark completion.</p>
        </div>
      </div>

      {/* Filter Bar */}
      <div className="filter-bar">
        <input
          type="text"
          placeholder="Search by customer name, ref code, service..."
          value={searchQuery}
          onChange={(e) => setSearchQuery(e.target.value)}
          className="search-input"
        />

        <div className="status-filter-group">
          {['All', 'Pending', 'Confirmed', 'Completed', 'Cancelled', 'NoShow'].map((status) => (
            <button
              key={status}
              className={`filter-btn ${filterStatus === status ? 'active' : ''}`}
              onClick={() => setFilterStatus(status)}
            >
              {status}
            </button>
          ))}
        </div>
      </div>

      {loading ? (
        <div className="loader">Loading bookings...</div>
      ) : (
        <div className="table-card">
          <table className="admin-table">
            <thead>
              <tr>
                <th>Ref Code</th>
                <th>Customer</th>
                <th>Service</th>
                <th>Specialist</th>
                <th>Date & Time</th>
                <th>Amount</th>
                <th>Status</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {filtered.map((b) => (
                <tr key={b.id}>
                  <td className="font-mono">{b.id}</td>
                  <td>
                    <div className="font-bold">{b.customerName}</div>
                    <div className="phone-sub">{b.customerPhone}</div>
                  </td>
                  <td>{b.serviceName}</td>
                  <td>{b.staffName}</td>
                  <td>{b.date} @ {b.timeSlot}</td>
                  <td className="font-bold text-accent">${b.price}</td>
                  <td>
                    <span className={`status-pill status-${b.status.toLowerCase()}`}>
                      {b.status}
                    </span>
                  </td>
                  <td>
                    <div className="action-buttons">
                      {b.status === 'Pending' && (
                        <Button size="small" variant="primary" onClick={() => updateBookingStatus(b.id, 'Confirmed')}>
                          Confirm
                        </Button>
                      )}
                      {b.status === 'Confirmed' && (
                        <Button size="small" variant="primary" onClick={() => updateBookingStatus(b.id, 'Completed')}>
                          Complete (+10 Pts)
                        </Button>
                      )}
                      {b.status !== 'Cancelled' && b.status !== 'Completed' && (
                        <Button size="small" variant="danger" onClick={() => updateBookingStatus(b.id, 'Cancelled')}>
                          Cancel
                        </Button>
                      )}
                    </div>
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
