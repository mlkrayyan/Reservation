import { BrowserRouter, Routes, Route } from 'react-router-dom';
import MainLayout from './components/layout/MainLayout';

// Placeholder Pages
const Home = () => <div style={{padding: '2rem'}}><h1>Home Page</h1></div>;
const Services = () => <div style={{padding: '2rem'}}><h1>Services Page</h1></div>;
const Staff = () => <div style={{padding: '2rem'}}><h1>Staff Page</h1></div>;
const Booking = () => <div style={{padding: '2rem'}}><h1>Booking Page</h1></div>;

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<MainLayout />}>
          <Route index element={<Home />} />
          <Route path="services" element={<Services />} />
          <Route path="staff" element={<Staff />} />
          <Route path="booking" element={<Booking />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
