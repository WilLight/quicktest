import axios from 'axios';

export const authApi = {
   async register(payload: any): Promise<any> {
      const { data } = await axios.post<any>('http://localhost:8888/api/auth/register', {
         name: payload.email,
         email: payload.email,
         password: payload.password,
      });

      return data;
   },

   async login(payload: any): Promise<any> {
      const { data } = await axios.post<any>(
         'http://localhost:8888/api/auth/login',
         { email: payload.email, password: payload.password },
         { withCredentials: true },
      );

      return data;
   },
};
