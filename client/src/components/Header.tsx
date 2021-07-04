import React from 'react';
import { Link } from 'react-router-dom';
import { UserInterface } from '../interfaces';
import { sessionMemory } from '../utils/sessionMemory';
import { Login } from './Login';
import { Signup } from './Signup';

export const Header: React.FC = () => {
   const [visibleAuth, setVisibleAuth] = React.useState<boolean>(false);
   const toggleVisibleAuth = () => setVisibleAuth(!visibleAuth);
   const [visibleSignup, setVisibleSignup] = React.useState<boolean>(false);

   const userData: UserInterface = sessionMemory.get('userdata');

   const onCloseMenu = (): void => {
      setVisibleAuth(false);
      document.body.classList.toggle('bodynoscroll');
   };

   return (
      <>
         <header className="header">
            <div className="container ">
               <div className="header__inner flex-row justify-between">
                  <Link to="/" className="logo">
                     <span>QuickTest</span>
                  </Link>
                  <div className="header__nav">
                     {!userData ? (
                        <button className="header__navitem" onClick={toggleVisibleAuth}>
                           Account
                        </button>
                     ) : (
                        <Link to="/account" className="header__navitem">
                           Account
                        </Link>
                     )}
                     {userData && userData.userRole === 0 ? (
                        <>
                           <Link to="/tests" className="header__navitem">
                              Your tests
                           </Link>
                           <Link to="/create" className="header__navitem">
                              Create
                           </Link>
                        </>
                     ) : undefined}
                  </div>
               </div>
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
