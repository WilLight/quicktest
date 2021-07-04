import React from 'react';
import { useQuery } from 'react-query';
import { useParams } from 'react-router-dom';
import { classroomApi } from '../api/classroomApi';

export const Classroom = () => {
   const params: { id: string } = useParams();

   const [modalActive, setModalActive] = React.useState<'student' | 'test' | 'info'>();
   const closeModal = () => setModalActive(undefined);
   const setActiveModal = (payload: 'student' | 'test' | 'info') => setModalActive(payload);

   const { data } = useQuery(['classroom', params.id], () => classroomApi.getDetails(parseInt(params.id)));

   return (
      <>
         {data ? (
            <div className="dashboard">
               <div className="dashboard__name">
                  <h1>Classroom</h1>
               </div>
               <div className="dashboard__header">
                  <div className="dashboard__profile flex-column">
                     <span>{data.roomName}</span>
                     <span>Students: {data.studentIds ? data.studentIds : 0}</span>
                  </div>
                  <div className="dashboard__items">
                     <div>
                        <button className="dashboard__button dashboard__button--active">Tests</button>
                        <button className="dashboard__button">Students</button>
                        <button onClick={() => setActiveModal('info')} className="dashboard__button">
                           Info
                        </button>
                     </div>
                     <div>
                        <button onClick={() => setActiveModal('student')} className="dashboard__button">
                           Add student
                        </button>
                        <button onClick={() => setActiveModal('test')} className="dashboard__button">
                           Add test
                        </button>
                     </div>
                  </div>
               </div>
               <ul className="dashboard__content">
                  <li className="modules__card">
                     <h3>Quick Test</h3>
                     <span>2137 questions</span>
                  </li>
                  <li className="modules__card">
                     <h3>Dolor sunt ad reprehenderit labore velit mollit culpa sunt voluptate reprehenderit velit quis duis.</h3>
                     <span>307 questions</span>
                  </li>
               </ul>
            </div>
         ) : undefined}
         <div className={modalActive ? 'overlay overlay--visible' : 'overlay'}></div>
         <div className={modalActive === 'info' ? 'modal modal--visible' : 'modal'}>
            <div className="modal__header">
               <h2>Info about classroom</h2>
               <button onClick={closeModal} className="minibutton" type="button" title="close" aria-label="close">
                  <span className="minibutton__content minibutton__content--white">
                     <svg xmlns="http://www.w3.org/2000/svg" width="1.5em" height="1.5em" fill="currentColor" viewBox="0 0 16 16">
                        <path
                           fillRule="evenodd"
                           d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z"
                        />
                     </svg>
                  </span>
               </button>
            </div>
            <div className="modal__block">
               <div className="modal__message">there are must be info</div>
            </div>
         </div>
         <div className={modalActive === 'student' ? 'modal modal--visible' : 'modal'}>
            <div className="modal__header">
               <h2>Invite students</h2>
               <button onClick={closeModal} className="minibutton" type="button" title="close" aria-label="close">
                  <span className="minibutton__content minibutton__content--white">
                     <svg xmlns="http://www.w3.org/2000/svg" width="1.5em" height="1.5em" fill="currentColor" viewBox="0 0 16 16">
                        <path
                           fillRule="evenodd"
                           d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z"
                        />
                     </svg>
                  </span>
               </button>
            </div>
            <div className="modal__block">
               <div className="modal__message">
                  To invite students to this class, add their QuickTest usernames below (separate by commas).
               </div>
               <form className="flex-row justify-between">
                  <div className="input">
                     <input type="text" />
                  </div>
                  <button type="submit" className="button button--transparent">
                     <span>Invite</span>
                  </button>
               </form>
            </div>
         </div>
         <div className={modalActive === 'test' ? 'modal modal--visible' : 'modal'}>
            <div className="modal__header">
               <h2>Add test</h2>
               <button onClick={closeModal} className="minibutton" type="button" title="close" aria-label="close">
                  <span className="minibutton__content minibutton__content--white">
                     <svg xmlns="http://www.w3.org/2000/svg" width="1.5em" height="1.5em" fill="currentColor" viewBox="0 0 16 16">
                        <path
                           fillRule="evenodd"
                           d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z"
                        />
                     </svg>
                  </span>
               </button>
            </div>
            <div className="modal__block">
               <div className="modal__message">
                  To add tests to this class, add their QuickTest ids below (separate by commas).
               </div>
               <form className="flex-row justify-between">
                  <div className="input">
                     <input type="text" />
                  </div>
                  <button type="submit" className="button button--transparent">
                     <span>Add</span>
                  </button>
               </form>
            </div>
         </div>
      </>
   );
};
