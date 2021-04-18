import React from 'react';
import { useForm } from 'react-hook-form';
import * as yup from 'yup';
import { yupResolver } from '@hookform/resolvers/yup';
import { useMutation } from 'react-query';
import { authApi } from '../api/authApi';
import { toastify } from '../utils/toastify';

interface Props {
   onCloseMenu: () => void;
   setVisibleSignup: React.Dispatch<React.SetStateAction<boolean>>;
}

export interface RegisterFormProps {
   fullname: string;
   // email: string;
   password: string;
   role: string;
}

const RegisterSchema = yup.object().shape({
   fullname: yup.string().required('no fullname'),
   // email: yup.string().email('not valid email').required('no email'),
   password: yup.string().min(8, 'min password length 8').required(),
   role: yup.string().required(),
});

export const Signup: React.FC<Props> = ({ onCloseMenu, setVisibleSignup }) => {
   const { register, handleSubmit } = useForm({ resolver: yupResolver(RegisterSchema) });
   const { mutateAsync, isLoading, status } = useMutation(authApi.register);

   const onSubmit = async (data: RegisterFormProps) => {
      try {
         await mutateAsync(data);
      } catch (error) {
         console.log('error registatrion');
         //   setAuth(AuthStatus.ERROR_REGISTRATION);
      }
   };

   React.useEffect(() => {
      if (status === 'success') {
         toggleAuthMenu();
         toastify('You have successfully registered, you can now log in.');
      }

      if (status === 'error') toastify('Such e-mail is already registered or passwords do not match.');
   }, [status]);

   const toggleAuthMenu = () => {
      setVisibleSignup(false);
   };

   return (
      <div className="auth__container">
         <div className="auth__header">
            <span>Sign up</span>
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
               <span>Full Name</span>
               <input type="text" required {...register('fullname')} />
            </div>
            <div className="input">
               <span>Email</span>
               <input type="email" required {...register('email')} />
            </div>
            <div className="input">
               <span>Password</span>
               <input type="password" required {...register('password')} />
            </div>
            <div className="rowwrapper">
               <div className="input">
                  <span>I am a teacher</span>
                  <input type="radio" value="teacher" required {...register('role')} />
               </div>
               <div className="input">
                  <span>I am a student</span>
                  <input type="radio" value="student" required {...register('role')} />
               </div>
            </div>
            <button disabled={isLoading} type="submit" className="button button--transparent">
               <span>{isLoading ? 'Loading..' : 'GO'}</span>
            </button>
         </form>
         <div className="auth__bottom">
            <button onClick={toggleAuthMenu}>I am already registered</button>
         </div>
      </div>
   );
};
