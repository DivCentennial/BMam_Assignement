export interface UserAuth {
  userId: number;
  userName: string;
  passwordHash: string;
  role: string;
  employeeId?: number;
}

export interface LoginRequest {
  username: string;
  password: string;
}
