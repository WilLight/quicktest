import axios from 'axios';
import { ClassroomInterface } from '../interfaces';

export const classroomApi = {
   async getDetails(roomId: number): Promise<ClassroomInterface> {
      const { data } = await axios.get(`http://localhost:5000/classroom/getdetails/?classroomId=${roomId}`);
      return data;
   },

   async getById(userId: number): Promise<ClassroomInterface[]> {
      const { data } = await axios.get(`http://localhost:5000/classroom/getclassrooms/?userId=${userId}`);
      return data;
   },

   async add(payload: { userId: number; roomName: string }): Promise<ClassroomInterface> {
      const { data } = await axios.post('http://localhost:5000/classroom/add', {
         userId: payload.userId,
         roomName: payload.roomName,
      });

      return data;
   },

   async addUser(payload: { classroomId: number; userId: number }): Promise<ClassroomInterface> {
      const { data } = await axios.post('http://localhost:5000/classroom/adduser', {
         classroomId: payload.classroomId,
         userId: payload.userId,
      });

      return data;
   },

   async addQuiz(payload: { classroomId: number; quizId: number }): Promise<ClassroomInterface> {
      const { data } = await axios.post('http://localhost:5000/classroom/addquiz', {
         classroomId: payload.classroomId,
         quizId: payload.quizId,
      });

      return data;
   },
};
