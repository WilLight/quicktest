import React from 'react';
import { Login } from './Login';
import { Signup } from './Signup';

export const Header: React.FC = () => {
   const [visibleAuth, setVisibleAuth] = React.useState<boolean>(false);
   const toggleVisibleAuth = () => setVisibleAuth(!visibleAuth);
   const [visibleSignup, setVisibleSignup] = React.useState<boolean>(false);

   const onCloseMenu = (): void => {
      setVisibleAuth(false);
      document.body.classList.toggle('bodynoscroll');
   };

   return (
      <>
         <header className="header">
            <div className="logo">
               <span>QuickTest</span>
            </div>
            <div className="header__nav">
               <button onClick={toggleVisibleAuth}>Account</button>
               <button>Add</button>
            </div>
         </header>
         <div className={visibleAuth ? 'auth auth--visible' : 'auth'}>
            {visibleAuth && visibleSignup ? (
               <Signup onCloseMenu={onCloseMenu} setVisibleSignup={setVisibleSignup} />
            ) : (
               <Login onCloseMenu={onCloseMenu} setVisibleSignup={setVisibleSignup} />
            )}
         </div>
         <div className={visibleAuth ? 'overlay overlay--visible' : 'overlay'}></div>
      </>
   );
};
