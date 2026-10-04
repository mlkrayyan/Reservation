import axios from 'axios';

// Relative by default so the production build talks to whatever origin served
// it — that is what lets the demo exe host the SPA and the API on one port.
// VITE_API_BASE_URL stays available for pointing a build at a remote API.
const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL ?? '/api',
});


api.interceptors.request.use((config) => {
  const token = localStorage.getItem('spa_token');
  if (token && config.headers) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export interface Service {
  id: string;
  name: string;
  description: string;
  durationMinutes: number;
  price: number;
  category?: string;
}

export interface Staff {
  id: string;
  name: string;
  specialization: string;
  bio?: string;
}

// /services and /staff answer with a PagedResult envelope ({ items, pageNumber,
// ... }), not a bare array. Callers assign straight into a Service[]/Staff[]
// state and then .filter/.map it, so handing back the envelope crashes the
// render — and it crashes past the caller's .catch, because the request itself
// succeeded. Unwrap here, tolerating a bare array in case an endpoint changes.
const unwrapPaged = <T,>(data: T[] | { items: T[] }): T[] =>
  Array.isArray(data) ? data : data?.items ?? [];

export const getServices = async (): Promise<Service[]> => {
  const res = await api.get('/services');
  return unwrapPaged<Service>(res.data);
};

export const getStaff = async (): Promise<Staff[]> => {
  const res = await api.get('/staff');
  return unwrapPaged<Staff>(res.data);
};

export default api;
