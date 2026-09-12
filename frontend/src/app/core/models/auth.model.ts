import { UserRole } from './enums';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  expiresAt: string;
  user: User;
}

export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
}

export interface User {
  id: number;
  name: string;
  email: string;
  role: UserRole;
  roleName: string;
  isActive: boolean;
  doctorId?: number | null;
  createdAt: string;
}

export interface CreateUser {
  name: string;
  email: string;
  password: string;
  role: UserRole;
  doctorId?: number | null;
}

export interface UpdateUser {
  name: string;
  email: string;
  role: UserRole;
  isActive: boolean;
  password?: string | null;
  doctorId?: number | null;
}
