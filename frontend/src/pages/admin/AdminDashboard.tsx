import { useNavigate } from 'react-router-dom';
import { Card } from '../../components/ui/Card';
import { Button } from '../../components/ui/Button';
import './AdminDashboard.css';

export default function AdminDashboard() {
  const navigate = useNavigate();

  const kpis = [
    { label: "Today's Bookings", val: "14", icon: "📅", change: "+12% from yesterday", color: "#C44DFF" },
    { label: "Estimated Daily Revenue", val: "$1,280", icon: "💵", change: "+18% from target", color: "#10b981" },
    { label: "Staff Utilization Rate", val: "88%", icon: "⚡", change: "4 Specialist Staff Active", color: "#3b82f6" },
    { label: "Pending Confirmations", val: "3", icon: "⏳", change: "Requires admin action", color: "#f59e0b" },
  ];

  const todaySchedule = [
    { time: '09:00 AM', customer: 'Sara Ahmed', service: 'Royal Swedish Massage', staff: 'Amina Khalil', status: 'Confirmed' },
    { time: '10:30 AM', customer: 'Noor Ali', service: 'Hydra-Facial Glow', staff: 'Dr. Fatima Al-Zahra', status: 'Completed' },
    { time: '11:30 AM', customer: 'Zahra Hassan', service: 'Silk Hair Spa & Blowdry', staff: 'Layla Hassan', status: 'Confirmed' },
    { time: '02:00 PM', customer: 'Mona Kareem', service: 'Luxury Spa Manicure', staff: 'Zainab Omar', status: 'Pending' },
    { time: '04:00 PM', customer: 'Rania Tariq', service: 'Full Body Scrub', staff: 'Amina Khalil', status: 'Confirmed' },
  ];

  return (
    <div className="admin-dashboard-container">
      <div className="dashboard-header">
        <h1>Dashboard Overview</h1>
        <p className="subtitle">Real-time salon operations, daily revenue, and schedule analytics.</p>
      </div>

      {/* KPI Cards Grid */}
      <div className="kpi-grid">
        {kpis.map((kpi, idx) => (
          <Card key={idx} className="kpi-card">
            <div className="kpi-top">
              <span className="kpi-label">{kpi.label}</span>
              <span className="kpi-icon">{kpi.icon}</span>
            </div>
            <div className="kpi-val" style={{ color: kpi.color }}>{kpi.val}</div>
            <div className="kpi-change">{kpi.change}</div>
          </Card>
        ))}
      </div>

      {/* Quick Action Bar & Schedule Grid */}
      <div className="dashboard-layout">
        <div className="schedule-panel">
          <div className="panel-header">
            <h2>Today's Appointment Timeline</h2>
            <Button size="small" variant="outline" onClick={() => navigate('/admin/bookings')}>
              View All Bookings →
            </Button>
          </div>

          <div className="schedule-table-wrapper">
            <table className="admin-table">
              <thead>
                <tr>
                  <th>Time Slot</th>
                  <th>Customer</th>
                  <th>Service</th>
                  <th>Specialist</th>
                  <th>Status</th>
                </tr>
              </thead>
              <tbody>
                {todaySchedule.map((item, index) => (
                  <tr key={index}>
                    <td className="time-col">⏰ {item.time}</td>
                    <td className="font-medium">{item.customer}</td>
                    <td>{item.service}</td>
                    <td>{item.staff}</td>
                    <td>
                      <span className={`status-pill status-${item.status.toLowerCase()}`}>
                        {item.status}
                      </span>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>

        <div className="quick-actions-panel">
          <h2>Quick Actions</h2>
          <div className="actions-list">
            <Button size="medium" variant="primary" style={{ width: '100%' }} onClick={() => navigate('/admin/bookings')}>
              + New Manual Booking
            </Button>
            <Button size="medium" variant="secondary" style={{ width: '100%' }} onClick={() => navigate('/admin/services')}>
              ⚙️ Manage Services & Pricing
            </Button>
            <Button size="medium" variant="secondary" style={{ width: '100%' }} onClick={() => navigate('/admin/staff')}>
              👥 Staff Working Hours
            </Button>
            <Button size="medium" variant="secondary" style={{ width: '100%' }} onClick={() => navigate('/admin/waitlist')}>
              ⏳ Offer Slot to Waitlist
            </Button>
          </div>
        </div>
      </div>
    </div>
  );
}
