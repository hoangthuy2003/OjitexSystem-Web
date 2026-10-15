export interface Category {
  categoryId: string;
  categoryName: string;
}

export interface AuthenticatedUser {
  userId: string;
  userFamilyName: string | null;
  userFirstName: string | null;
  roles: string[];
  categories: Category[];
}

export interface LoginResponse {
  accessToken: string;
  expiresAt: string;
  user: AuthenticatedUser;
}
