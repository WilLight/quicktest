import React from 'react';
import { useForm } from 'react-hook-form';
import { toastify } from '../utils/toastify';

interface Props {
   onCloseMenu: () => void;
   setVisibleSignup: React.Dispatch<React.SetStateAction<boolean>>;
}

export interface LoginFormProps {
   email: string;
   password: string;
}

export const Login: React.FC<Props> = ({ onCloseMenu, setVisibleSignup }) => {
   const { handleSubmit } = useForm<LoginFormProps>({});
   //    const { mutateAsync, isLoading, status } = useMutation(authApi.login);

   const onSubmit = async (data: LoginFormProps) => {
      //   try {
      //      await mutateAsync(data);
      //   } catch (error) {
      //      setAuth(AuthStatus.ERROR_LOGIN);
      //   }
   };

   //    React.useEffect(() => {
   //       if (status === 'success') {
   //          //  router.push('/account');
   //       }

   //       if (status === 'error') toastify('Not valid email or password');
   //    }, [status]);

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
               <span>Email</span>
               <input type="email" name="email" required />
            </div>
            <div className="input">
               <span>Password</span>
               <input type="password" name="password" required />
            </div>
            <button type="submit" className="button button--transparent">
               <span>GO</span>
            </button>
         </form>
         <div className="auth__bottom">
            <button onClick={toggleAuthMenu}>Sign up</button>
         </div>
      </div>
   );
};
