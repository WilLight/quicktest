import React from 'react';

export const CreateTest = () => {
   const [termCount, setTermCount] = React.useState<number>(1);
   const incTermCount = () => setTermCount(termCount + 1);

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

         {Array(termCount)
            .fill(0)
            .map((_, index) => (
               <div key={index} className="create__block">
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
            <button onClick={incTermCount} type="submit" className="button button--transparent">
               <span>Add term</span>
            </button>
         </div>
         <div className="create__block">
            <button type="submit" className="button button--transparent">
               <span>Create Test</span>
            </button>
         </div>
      </>
   );
};
