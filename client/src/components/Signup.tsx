import React from 'react';
import { useForm } from 'react-hook-form';
import { useMutation } from 'react-query';
import { toastify } from '../utils/toastify';

interface Props {
   onCloseMenu: () => void;
   setVisibleSignup: React.Dispatch<React.SetStateAction<boolean>>;
}

export interface RegisterFormProps {
   email: string;
   password: string;
   password2: string;
}

export const Signup: React.FC<Props> = ({ onCloseMenu, setVisibleSignup }) => {
   const { handleSubmit } = useForm<RegisterFormProps>({});
   // const { mutateAsync, isLoading, status } = useMutation(authApi.register);

   const onSubmit = async (data: RegisterFormProps) => {
      //   try {
      //      await mutateAsync(data);
      //   } catch (error) {
      //      setAuth(AuthStatus.ERROR_REGISTRATION);
      //   }
   };

   //    React.useEffect(() => {
   //       if (status === 'success') {
   //          toggleAuthMenu();
   //          toastify('You have successfully registered, you can now log in.');
   //       }

   //       if (status === 'error') toastify('Such e-mail is already registered or passwords do not match.');
   //    }, [status]);

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
               <input type="text" name="fullname" required />
            </div>
            <div className="input">
               <span>Email</span>
               <input type="email" name="email" required />
            </div>
            <div className="input">
               <span>Password</span>
               <input type="password" name="password" required />
            </div>
            <div className="rowwrapper">
               <div className="input">
                  <span>I am a teacher</span>
                  <input type="radio" name="role" value="teacher" required />
               </div>
               <div className="input">
                  <span>I am a student</span>
                  <input type="radio" name="role" value="student" required />
               </div>
            </div>
            <button type="submit" className="button button--transparent">
               <span>GO</span>
            </button>
         </form>
         <div className="auth__bottom">
            <button onClick={toggleAuthMenu}>I am already registered</button>
         </div>
      </div>
   );
};
