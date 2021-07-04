import axios from 'axios';
import { TestInterface } from '../interfaces';

export const testsApi = {
   async getByClassroom(roomId: number): Promise<TestInterface[]> {
      const { data } = await axios.get(`http://localhost:5000/quiz/GetClassroomQuizes/?classroomId=${roomId}`);
      return data;
   },

   async getByUser(userId: number): Promise<TestInterface[]> {
      const { data } = await axios.get(`http://localhost:5000/quiz/GetUserQuizes/?userId=${userId}`);
      return data;
   },

   async getByQuiz(quizId: number): Promise<TestInterface> {
      const { data } = await axios.get(`http://localhost:5000/quiz/getquiz/?quizId=${quizId}`);
      return data;
   },

   async add(payload: TestInterface): Promise<TestInterface> {
      const { data } = await axios.post('http://localhost:5000/quiz/addquiz', payload);
      return data;
   },
};
