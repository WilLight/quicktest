import React from 'react';
import { useForm } from 'react-hook-form';
import { useMutation } from 'react-query';
import { useHistory } from 'react-router-dom';
import * as yup from 'yup';
import { yupResolver } from '@hookform/resolvers/yup';
import { authApi } from '../api/authApi';
import { toastify } from '../utils/toastify';
import { sessionMemory } from '../utils/sessionMemory';

interface Props {
   onCloseMenu: () => void;
   setVisibleSignup: React.Dispatch<React.SetStateAction<boolean>>;
}

export interface LoginFormProps {
   login: string;
   password: string;
}

const LoginSchema = yup.object().shape({
   login: yup.string().required('no nickname'),
   password: yup.string().min(8, 'min password length 8').required(),
});

export const Login: React.FC<Props> = ({ onCloseMenu, setVisibleSignup }) => {
   const history = useHistory();
   const { handleSubmit, register } = useForm<LoginFormProps>({ resolver: yupResolver(LoginSchema) });
   const { data, mutateAsync, isLoading, status } = useMutation(authApi.login);

   const onSubmit = async (data: LoginFormProps) => {
      try {
         await mutateAsync(data);
      } catch (error) {
         console.log(error);
      }
   };

   React.useEffect(() => {
      if (data) {
         sessionMemory.set('userdata', data);
         onCloseMenu();
         history.push('/account');
      }
   }, [data]);

   React.useEffect(() => {
      if (status === 'error') toastify('Not valid payload');
      // eslint-disable-next-line react-hooks/exhaustive-deps
   }, [status]);

   const toggleAuthMenu = () => {
      setVisibleSignup(true);
   };

   return (
      <div className="auth__container">
         <div className="auth__header">
            <span>Login</span>
            <button onClick={onCloseMenu} className="minibutton" type="button" title="close" aria-label="close">
               <span className="minibutton__content minibutton__content--primary">
                  <svg xmlns="http://www.w3.org/2000/svg" width="1.5em" height="1.5em" fill="currentColor" viewBox="0 0 16 16">
                     <path
                        fillRule="evenodd"
                        d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z"
                     />
                  </svg>
               </span>
            </button>
         </div>
         <form onSubmit={handleSubmit(onSubmit)}>
            <div className="input">
               <span>Nickname via Login</span>
               <input type="text" required {...register('login')} />
            </div>
            <div className="input">
               <span>Password</span>
               <input type="password" required {...register('password')} />
            </div>
            <button type="submit" className="button button--transparent">
               <span>{isLoading ? 'Loading..' : 'GO'}</span>
            </button>
         </form>
         <div className="auth__bottom">
            <button onClick={toggleAuthMenu}>Sign up</button>
         </div>
      </div>
   );
};
