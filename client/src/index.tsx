import React from 'react';
import ReactDOM from 'react-dom';
import './styles/globals.scss';
import App from './App';
import { ToastContainer } from 'react-toastify';
import { BrowserRouter } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from 'react-query';
import 'react-toastify/dist/ReactToastify.min.css';

const queryClient = new QueryClient({
   defaultOptions: {
      queries: {
         refetchOnWindowFocus: false,
         retry: false,
      },
   },
});

ReactDOM.render(
   <React.StrictMode>
      <BrowserRouter>
         <QueryClientProvider client={queryClient}>
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
         </QueryClientProvider>
      </BrowserRouter>
   </React.StrictMode>,
   document.getElementById('root'),
);
