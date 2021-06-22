// список классов
// данные пользователя

import React from 'react';
import { useMutation } from 'react-query';
import { classroomApi } from '../api/classroomApi';
import { UserInterface } from '../interfaces';
import { sessionMemory } from '../utils/sessionMemory';

export const Account = () => {
   const userData: UserInterface = sessionMemory.get('userdata');
   React.useEffect(() => {
      if (!userData) window.location.replace('/');
   }, [userData]);

   /* classes */
   const { data: classes } = useMutation('classes', () => classroomApi.getById(userData.id));

   return (
      <div className="dashboard">
         <div className="dashboard__name">
            <h1>Account</h1>
         </div>
         <div className="dashboard__header">
            <div className="dashboard__profile flex-column">
               <span>{userData && userData.credentials ? userData.credentials.login : ''}</span>
               <span>{userData && userData.userRole === 0 ? 'Teacher' : 'Student'}</span>
            </div>
            <div className="dashboard__items">
               <button className="dashboard__button dashboard__button--active">Classes</button>
               <button className="dashboard__button">Students</button>
            </div>
         </div>
         <ul className="dashboard__content">
            {classes
               ? classes.map((content) => (
                    <li className="modules__card">
                       <h3>{content.roomId}</h3>
                       <span>{content.studentIds?.length} students</span>
                    </li>
                 ))
               : undefined}
         </ul>
      </div>
   );
};
