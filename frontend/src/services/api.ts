import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5139/api',
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

export const getServices = async (): Promise<Service[]> => {
  const res = await api.get('/services');
  return res.data;
};

export const getStaff = async (): Promise<Staff[]> => {
  const res = await api.get('/staff');
  return res.data;
};

export default api;
