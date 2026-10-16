import type { Category } from '../../../core/auth/auth.models';

export interface AdminUser {
  userId: string;
  userFamilyName: string | null;
  userFirstName: string | null;
  isLocked: boolean;
  lastLoginDate: string | null;
  roles: string[];
  categories: Category[];
  directCategoryIds: string[];
}

export interface UserRole {
  role: string;
  description: string | null;
}

export interface UserForm {
  userId: string;
  userFamilyName: string;
  userFirstName: string;
  roles: string[];
  categoryIds: string[];
}
