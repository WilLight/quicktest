// список классов
// данные пользователя

import React from 'react';
import { useHistory } from 'react-router';
import { UserInterface } from '../interfaces';
import { sessionMemory } from '../utils/sessionMemory';

export const Account = () => {
   const userData: UserInterface = sessionMemory.get('userdata');
   const history = useHistory();

   React.useEffect(() => {
      if (!userData) history.replace('/');
   }, [userData]);

   return (
      <div className="dashboard">
         <div className="dashboard__name">
            <h1>Account</h1>
         </div>
         <div className="dashboard__header">
            <div className="dashboard__profile flex-column">
               <span>{userData ? userData.id : ''}</span>
               <span>{userData ? userData.credentials.login : ''}</span>
            </div>
            <div className="dashboard__items">
               <button className="dashboard__button dashboard__button--active">Classes</button>
               <button className="dashboard__button">Students</button>
            </div>
         </div>
         <ul className="dashboard__content">
            <li className="modules__card">
               <h3>Quick Test Classroom</h3>
               <span>2137 students</span>
            </li>
            <li className="modules__card">
               <h3>Dolor sunt ad reprehenderit labore velit mollit culpa sunt voluptate reprehenderit velit quis duis.</h3>
               <span>307 students</span>
            </li>
         </ul>
      </div>
   );
};
