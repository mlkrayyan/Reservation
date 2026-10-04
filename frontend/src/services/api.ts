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

// The API exposes the bilingual domain entities directly: SalonService carries
// nameEn/nameAr and StaffMember carries fullName. The UI is built around a flat
// single-language shape, and reads member.name.charAt(0) unguarded, so map here
// rather than scattering fallbacks through the pages. The `??` chains keep these
// working if an endpoint is ever changed to return the flat shape already.
const toService = (raw: any): Service => ({
  id: raw.id,
  name: raw.nameEn ?? raw.name ?? '',
  description: raw.descriptionEn ?? raw.description ?? '',
  durationMinutes: raw.durationMinutes,
  price: raw.price,
  category: raw.category,
});

const toStaff = (raw: any): Staff => ({
  id: raw.id,
  name: raw.fullName ?? raw.name ?? '',
  // StaffMember has no separate specialization field; its bio is written as one
  // ("Facial & Skincare Specialist with 10+ years experience").
  specialization: raw.specialization ?? raw.bio ?? '',
  bio: raw.specialization ? raw.bio : undefined,
});

export const getServices = async (): Promise<Service[]> => {
  const res = await api.get('/services');
  return unwrapPaged<any>(res.data).map(toService);
};

export const getStaff = async (): Promise<Staff[]> => {
  const res = await api.get('/staff');
  return unwrapPaged<any>(res.data).map(toStaff);
};

export default api;
