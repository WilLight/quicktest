import { useMutation } from 'react-query';
import { classroomApi } from '../api/classroomApi';
import { useForm } from 'react-hook-form';
import * as yup from 'yup';
import { yupResolver } from '@hookform/resolvers/yup';
import { toastify } from '../utils/toastify';
import { UserInterface } from '../interfaces';
import { sessionMemory } from '../utils/sessionMemory';

const RoomSchema = yup.object().shape({
   roomName: yup.string().required(),
});

export const CreateClass = () => {
   const { handleSubmit, register } = useForm({ resolver: yupResolver(RoomSchema) });
   const user: UserInterface = sessionMemory.get('userdata');

   const { mutateAsync } = useMutation(classroomApi.add);
   const onSubmit = async (data: { roomName: string }) => {
      try {
         mutateAsync({ roomName: data.roomName, userId: user.id });
         toastify('successfully created classroom');
      } catch (error) {
         toastify('error with create classroom');
      }
   };

   return (
      <form onSubmit={handleSubmit(onSubmit)}>
         <input type="hidden" required {...register('userId')} />
         <div className="create__block">
            <div className="input input--white">
               <span>Class Name</span>
               <input type="text" required {...register('roomName')} />
            </div>
         </div>
         <div className="create__block">
            <button type="submit" className="button button--transparent">
               <span>Create Class</span>
            </button>
         </div>
      </form>
   );
};
