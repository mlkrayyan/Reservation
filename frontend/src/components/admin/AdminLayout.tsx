import { Outlet, Link, useNavigate, useLocation } from 'react-router-dom';
import { useAuthStore } from '../../services/useAuthStore';
import './AdminLayout.css';

export default function AdminLayout() {
  const navigate = useNavigate();
  const location = useLocation();
  const { user, logout } = useAuthStore();

  const navItems = [
    { path: '/admin', label: '📊 Dashboard', exact: true },
    { path: '/admin/bookings', label: '📅 Bookings' },
    { path: '/admin/services', label: '💆 Services Catalog' },
    { path: '/admin/staff', label: '👥 Staff & Schedule' },
    { path: '/admin/waitlist', label: '⏳ Waitlist' },
    { path: '/admin/analytics', label: '📈 Analytics' },
  ];

  return (
    <div className="admin-layout">
      {/* Sidebar */}
      <aside className="admin-sidebar">
        <div className="sidebar-brand" onClick={() => navigate('/admin')}>
          <span className="brand-logo">✦ SPA ADMIN</span>
        </div>
        
        <nav className="sidebar-menu">
          {navItems.map((item) => {
            const isActive = item.exact 
              ? location.pathname === item.path
              : location.pathname.startsWith(item.path);

            return (
              <Link
                key={item.path}
                to={item.path}
                className={`sidebar-item ${isActive ? 'active' : ''}`}
              >
                {item.label}
              </Link>
            );
          })}
        </nav>

        <div className="sidebar-footer">
          <Link to="/" className="back-to-site">← Back to Customer Website</Link>
        </div>
      </aside>

      {/* Main Content Area */}
      <div className="admin-main">
        <header className="admin-topbar">
          <div className="topbar-title">SPA Salon Management Portal</div>
          <div className="topbar-user">
            <span className="admin-badge">ADMIN</span>
            <span>{user?.fullName || 'Administrator'}</span>
            <button className="topbar-logout" onClick={() => { logout(); navigate('/'); }}>
              Logout
            </button>
          </div>
        </header>

        <main className="admin-content">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
