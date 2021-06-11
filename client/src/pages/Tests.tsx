import React from 'react';
import { Link } from 'react-router-dom';

export const Tests = () => {
   return (
      <div className="modules">
         <div className="">
            <h1>Your tests</h1>
         </div>
         <Link to="/t/XYZABC" className="modules__card">
            <h3>Module XYZABC</h3>
            <span>46 Terms</span>
         </Link>
         <Link to="/t/SDDFZGAS" className="modules__card">
            <h3>Module SDDFZGAS</h3>
            <span>10 Terms</span>
         </Link>
         <Link to="/t/WEWEREHS" className="modules__card">
            <h3>Module WEWEREHS</h3>
            <span>416 Terms</span>
         </Link>
      </div>
   );
};
