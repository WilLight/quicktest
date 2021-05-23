import React from 'react';
const arr = [0, 1, 2, 3, 4];
export const CreateTest = () => {
   return (
      <>
         <div className="create__block">
            <div className="input input--white">
               <span>Test Name</span>
               <input type="text" />
            </div>
            <div className="input input--white">
               <span>Description</span>
               <input type="text" />
            </div>
         </div>
         {arr.map((_, i) => (
            <div key={i} className="create__block">
               <div className="create__qa">
                  <div className="input input--white">
                     <span>Term</span>
                     <input type="text" />
                  </div>
                  <div className="input input--white">
                     <span>Defitinion</span>
                     <input type="text" />
                  </div>
               </div>
            </div>
         ))}
         <div className="create__block">
            <button type="submit" className="button button--transparent">
               <span>Create Test</span>
            </button>
         </div>
      </>
   );
};
