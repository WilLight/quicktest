import React from 'react';
import { CreateClass } from '../components/CreateClass';
import { CreateTest } from '../components/CreateTest';

const modulesCreate = ['Classroom', 'Test'];

export const Create = () => {
   /* popup */
   const [visiblePopupCreate, setVisiblePopupCreate] = React.useState<boolean>(false);
   const toggleVisiblePopupCreate = () => setVisiblePopupCreate(!visiblePopupCreate);
   const [activeCreate, setActiveCreate] = React.useState<number>();
   const popUpCreateRef = React.useRef<HTMLDivElement>(null);
   const setActiveLabelCreate = (index: number) => {
      setActiveCreate(index);
      toggleVisiblePopupCreate();
   };
   const nonPopupClick = (event: any) => {
      const path = event.path || (event.composedPath && event.composedPath());
      if (!path.includes(popUpCreateRef.current)) setVisiblePopupCreate(false);
   };
   React.useEffect(() => {
      document.body.addEventListener('click', nonPopupClick);
      return () => document.body.removeEventListener('click', nonPopupClick);
   }, []);

   console.log(activeCreate);

   return (
      <div className="create">
         <div className="create__header">
            <h1>Create</h1>
         </div>
         <div className="popup" ref={popUpCreateRef}>
            <button type="button" onClick={toggleVisiblePopupCreate}>
               <span>
                  {activeCreate !== undefined ? `Creating: ${modulesCreate[activeCreate]}` : 'Choose what you want to create'}
               </span>
               <svg
                  className={visiblePopupCreate ? 'rotate' : undefined}
                  xmlns="http://www.w3.org/2000/svg"
                  height="24"
                  viewBox="0 0 24 24"
                  width="24"
                  fill="currentColor">
                  <path d="M0 0h24v24H0z" fill="none"></path>
                  <path d="M16.59 8.59L12 13.17 7.41 8.59 6 10l6 6 6-6z"></path>
               </svg>
            </button>
            {visiblePopupCreate && (
               <div className="popup__list">
                  <ul>
                     {modulesCreate.map((name, index) => (
                        <li onClick={() => setActiveLabelCreate(index)} key={index}>
                           {name}
                        </li>
                     ))}
                  </ul>
               </div>
            )}
         </div>
         {activeCreate === 0 ? <CreateClass /> : activeCreate === 1 ? <CreateTest /> : undefined}
      </div>
   );
};
