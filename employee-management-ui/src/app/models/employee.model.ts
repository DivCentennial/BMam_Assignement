export interface EmployeePersonal {
  employeeId: number;
  fullName: string;
  gender: string;
  dob: Date;
  age?: number;
  address: string;
  contactNo: string;
  email: string;
  profileImageUrl?: string;
}

export interface EmployeeProfessional {
  employeeId: number;
  designation: string;
  department: string;
  qualification?: string;
  experience?: number;
  skill?: string;
}

export interface EmployeeUpsertRequest {
  personal: EmployeePersonal;
  professional: EmployeeProfessional;
}

export interface Employee {
  personal: EmployeePersonal;
  professional: EmployeeProfessional;
}

