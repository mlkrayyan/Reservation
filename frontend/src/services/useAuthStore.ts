import { create } from 'zustand';

export interface User {
  id: string;
  fullName: string;
  email: string;
  role: 'Customer' | 'Staff' | 'Admin' | 'SuperAdmin';
  loyaltyPoints?: number;
}

interface AuthState {
  user: User | null;
  token: string | null;
  isAuthenticated: boolean;
  login: (token: string, user: User) => void;
  logout: () => void;
  updateLoyaltyPoints: (points: number) => void;
}

export const useAuthStore = create<AuthState>((set) => ({
  user: JSON.parse(localStorage.getItem('spa_user') || 'null'),
  token: localStorage.getItem('spa_token'),
  isAuthenticated: !!localStorage.getItem('spa_token'),
  login: (token, user) => {
    localStorage.setItem('spa_token', token);
    localStorage.setItem('spa_user', JSON.stringify(user));
    set({ token, user, isAuthenticated: true });
  },
  logout: () => {
    localStorage.removeItem('spa_token');
    localStorage.removeItem('spa_user');
    set({ token: null, user: null, isAuthenticated: false });
  },
  updateLoyaltyPoints: (points) => {
    set((state) => {
      if (!state.user) return state;
      const updatedUser = { ...state.user, loyaltyPoints: points };
      localStorage.setItem('spa_user', JSON.stringify(updatedUser));
      return { user: updatedUser };
    });
  },
}));
