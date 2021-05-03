import { toast } from 'react-toastify';

export const toastify = (value: string) =>
   toast.dark(value, {
      position: 'bottom-right',
      autoClose: 5000,
      hideProgressBar: false,
      closeOnClick: true,
      pauseOnHover: true,
      draggable: true,
      progress: undefined,
   });
