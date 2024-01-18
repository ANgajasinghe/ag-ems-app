export interface Employee {
  fullName: string;
  email: string;
  joinDate: string;
  telephone: string;
  address: string;
  salary: number;

  lock: boolean;

}

export interface Dropdown {
  value: string;
  label: string;

}



export interface TokenResponse {
  accessToken: string;
  refreshToken: string;
}
