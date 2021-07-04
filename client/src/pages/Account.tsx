// список классов
// данные пользователя

import React from 'react';
import { useQuery } from 'react-query';
import { Link } from 'react-router-dom';
import { classroomApi } from '../api/classroomApi';
import { UserInterface } from '../interfaces';
import { sessionMemory } from '../utils/sessionMemory';

export const Account = () => {
   const userData: UserInterface = sessionMemory.get('userdata');
   React.useEffect(() => {
      if (!userData) window.location.replace('/');
   }, [userData]);

   /* active label */
   const [activeLabel, setActiveLabel] = React.useState<'classes' | 'students'>('classes');
   const setActiveClasses = () => setActiveLabel('classes');
   const setActiveStudents = () => setActiveLabel('students');

   /* classes */
   const { data: classes } = useQuery(['classes', userData ? userData.id : 0], () =>
      classroomApi.getById(userData ? userData.id : 0),
   );

   /* logout */
   const logOut = () => {
      sessionMemory.remove('userdata');
      window.location.replace('/');
   };

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
               <div>
                  <button
                     onClick={setActiveClasses}
                     className={activeLabel === 'classes' ? 'dashboard__button dashboard__button--active' : 'dashboard__button'}>
                     Classes
                  </button>
                  {userData && userData.userRole === 0 ? (
                     <button
                        onClick={setActiveStudents}
                        className={
                           activeLabel === 'students' ? 'dashboard__button dashboard__button--active' : 'dashboard__button'
                        }>
                        Students
                     </button>
                  ) : undefined}
               </div>
               <button onClick={logOut} className="dashboard__button">
                  Logout
               </button>
            </div>
         </div>
         <ul className="dashboard__content">
            {classes && activeLabel === 'classes'
               ? classes.map((content) => (
                    <li key={content.roomId} className="modules__card">
                       <Link to={`/classroom/${content.roomId}`}>
                          <h3>{content.roomName}</h3>
                          <span>{content.studentsIds?.length ? content.studentsIds?.length : 0} students</span>
                       </Link>
                    </li>
                 ))
               : undefined}
         </ul>
      </div>
   );
};
