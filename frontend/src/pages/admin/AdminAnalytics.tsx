import { AreaChart, Area, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, BarChart, Bar } from 'recharts';
import { Card } from '../../components/ui/Card';
import './AdminAnalytics.css';

const revenueData = [
  { month: 'Apr', revenue: 4200, bookings: 65 },
  { month: 'May', revenue: 5800, bookings: 88 },
  { month: 'Jun', revenue: 6400, bookings: 95 },
  { month: 'Jul', revenue: 7900, bookings: 120 },
  { month: 'Aug', revenue: 9200, bookings: 142 },
  { month: 'Sep', revenue: 11500, bookings: 175 },
];

const serviceCategoryData = [
  { name: 'Massage Therapy', count: 68 },
  { name: 'Facial & Skincare', count: 52 },
  { name: 'Hair & Styling', count: 44 },
  { name: 'Nail Care', count: 35 },
  { name: 'Body Scrub', count: 28 },
];

export default function AdminAnalytics() {
  return (
    <div className="admin-analytics-container">
      <div className="page-header">
        <h1>Analytics & Revenue Reports</h1>
        <p className="subtitle">Visual insights into salon monthly growth, top treatment popularity & staff utilization.</p>
      </div>

      <div className="analytics-grid">
        {/* Revenue Growth Chart */}
        <Card className="chart-card">
          <h3>📈 Monthly Revenue & Booking Trend</h3>
          <p className="chart-sub">Total revenue ($ USD) and completed appointment count over 6 months</p>
          <div className="chart-wrapper">
            <ResponsiveContainer width="100%" height={300}>
              <AreaChart data={revenueData}>
                <defs>
                  <linearGradient id="colorRev" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor="#C44DFF" stopOpacity={0.8}/>
                    <stop offset="95%" stopColor="#C44DFF" stopOpacity={0}/>
                  </linearGradient>
                </defs>
                <CartesianGrid strokeDasharray="3 3" stroke="rgba(255,255,255,0.1)" />
                <XAxis dataKey="month" stroke="rgba(255,255,255,0.5)" />
                <YAxis stroke="rgba(255,255,255,0.5)" />
                <Tooltip contentStyle={{ background: '#1a1a2e', border: '1px solid #C44DFF' }} />
                <Area type="monotone" dataKey="revenue" stroke="#C44DFF" fillOpacity={1} fill="url(#colorRev)" />
              </AreaChart>
            </ResponsiveContainer>
          </div>
        </Card>

        {/* Service Popularity Breakdown */}
        <Card className="chart-card">
          <h3>📊 Popular Treatment Categories</h3>
          <p className="chart-sub">Number of reservations booked per service category</p>
          <div className="chart-wrapper">
            <ResponsiveContainer width="100%" height={300}>
              <BarChart data={serviceCategoryData}>
                <CartesianGrid strokeDasharray="3 3" stroke="rgba(255,255,255,0.1)" />
                <XAxis dataKey="name" stroke="rgba(255,255,255,0.5)" tick={{ fontSize: 12 }} />
                <YAxis stroke="rgba(255,255,255,0.5)" />
                <Tooltip contentStyle={{ background: '#1a1a2e', border: '1px solid #7B2FBE' }} />
                <Bar dataKey="count" fill="#7B2FBE" radius={[6, 6, 0, 0]} />
              </BarChart>
            </ResponsiveContainer>
          </div>
        </Card>
      </div>
    </div>
  );
}
