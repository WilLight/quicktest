import axios from 'axios';
import { ClassroomInterface } from '../interfaces';

export const classroomApi = {
   async getById(userId: number): Promise<ClassroomInterface[]> {
      const { data } = await axios.post(`http://localhost:5000/classroom/getclassrooms/?userId=${userId}`);
      return data;
   },

   async add(payload: { userId: number; name: string }): Promise<ClassroomInterface> {
      const { data } = await axios.post('http://localhost:5000/classroom/add', {
         userId: payload.userId,
         name: payload.name,
      });

      return data;
   },
};
