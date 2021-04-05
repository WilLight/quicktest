import React from 'react';
import ReactDOM from 'react-dom';
import './styles/globals.scss';
import App from './App';
import { ToastContainer } from 'react-toastify';

ReactDOM.render(
   <React.StrictMode>
      <App />
      <ToastContainer
         progressStyle={{ background: '#6ca7ff' }}
         position="bottom-right"
         autoClose={5000}
         hideProgressBar={false}
         newestOnTop={false}
         closeOnClick
         rtl={false}
         pauseOnFocusLoss
         draggable
         pauseOnHover
      />
   </React.StrictMode>,
   document.getElementById('root'),
);
