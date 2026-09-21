import { BrowserRouter, Routes, Route } from 'react-router-dom';
import MainLayout from './components/layout/MainLayout';
import Home from './pages/Home';
import Services from './pages/Services';
import StaffPage from './pages/Staff';
import Booking from './pages/Booking';
import MyBookings from './pages/MyBookings';
import Loyalty from './pages/Loyalty';
import Waitlist from './pages/Waitlist';

// Admin Components
import AdminLayout from './components/admin/AdminLayout';
import AdminDashboard from './pages/admin/AdminDashboard';
import AdminBookings from './pages/admin/AdminBookings';
import AdminServices from './pages/admin/AdminServices';
import AdminStaff from './pages/admin/AdminStaff';
import AdminWaitlist from './pages/admin/AdminWaitlist';
import AdminAnalytics from './pages/admin/AdminAnalytics';

import { ProtectedRoute } from './components/auth/ProtectedRoute';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        {/* Customer Public & Portal Layout */}
        <Route path="/" element={<MainLayout />}>
          <Route index element={<Home />} />
          <Route path="services" element={<Services />} />
          <Route path="staff" element={<StaffPage />} />
          <Route path="booking" element={<Booking />} />
          
          {/* Customer Portal */}
          <Route path="my-bookings" element={
            <ProtectedRoute allowedRoles={['Customer', 'Admin', 'SuperAdmin']}>
              <MyBookings />
            </ProtectedRoute>
          } />
          <Route path="loyalty" element={
            <ProtectedRoute allowedRoles={['Customer', 'Admin', 'SuperAdmin']}>
              <Loyalty />
            </ProtectedRoute>
          } />
          <Route path="waitlist" element={
            <ProtectedRoute allowedRoles={['Customer', 'Admin', 'SuperAdmin']}>
              <Waitlist />
            </ProtectedRoute>
          } />
        </Route>

        {/* Admin Management Panel */}
        <Route path="/admin" element={
          <ProtectedRoute allowedRoles={['Admin', 'SuperAdmin']}>
            <AdminLayout />
          </ProtectedRoute>
        }>
          <Route index element={<AdminDashboard />} />
          <Route path="bookings" element={<AdminBookings />} />
          <Route path="services" element={<AdminServices />} />
          <Route path="staff" element={<AdminStaff />} />
          <Route path="waitlist" element={<AdminWaitlist />} />
          <Route path="analytics" element={<AdminAnalytics />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
