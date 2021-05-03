import axios from 'axios';
import { RegisterFormProps } from '../components/Signup';

export const authApi = {
   async register(payload: RegisterFormProps): Promise<any> {
      const { data } = await axios.post('http://localhost:5000/userauth/register', {
         registerData: {
            fullname: payload.fullname,
            // email: payload.email,
            password: payload.password,
         },
         userRole: payload.role,
      });

      return data;
   },

   async login(payload: any): Promise<any> {
      const { data } = await axios.post('http://localhost:5000/userauth/login', {
         email: payload.email,
         password: payload.password,
      });

      return data;
   },
};
