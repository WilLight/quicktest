import axios from 'axios';
import { LoginFormProps } from '../components/Login';
import { RegisterFormProps } from '../components/Signup';
import { UserInterface } from '../interfaces';

export const authApi = {
   async register(payload: RegisterFormProps): Promise<UserInterface> {
      const { data } = await axios.post('http://localhost:5000/userauth/register', {
         registerData: {
            login: payload.login,
            password: payload.password,
         },
         userRole: payload.role,
      });

      return data;
   },

   async login(payload: LoginFormProps): Promise<UserInterface> {
      const { data } = await axios.post('http://localhost:5000/userauth/login', {
         login: payload.login,
         password: payload.password,
      });

      return data;
   },
};
