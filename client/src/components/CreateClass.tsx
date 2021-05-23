import React from 'react';

export const CreateClass = () => {
   return (
      <>
         <div className="create__block">
            <div className="input input--white">
               <span>Class Name</span>
               <input type="text" />
            </div>
            <div className="input input--white">
               <span>Description</span>
               <input type="text" />
            </div>
         </div>
         <div className="create__block">
            <button type="submit" className="button button--transparent">
               <span>Create Class</span>
            </button>
         </div>
      </>
   );
};
