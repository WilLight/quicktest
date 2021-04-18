import React from 'react';
import { Link } from 'react-router-dom';
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
            <Link to="/" className="logo">
               <span>QuickTest</span>
            </Link>
            <div className="header__nav">
               <button className="header__navitem" onClick={toggleVisibleAuth}>
                  Account
               </button>
               <Link to="/modules" className="header__navitem">
                  Your modules
               </Link>
               <button className="header__navitem">Discover</button>
               <button className="header__navitem">Add</button>
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