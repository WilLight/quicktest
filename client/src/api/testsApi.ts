import axios from 'axios';
import { TestInterface } from '../interfaces';

export const testsApi = {
   async getByUser(userId: number): Promise<TestInterface[]> {
      const { data } = await axios.get(`http://localhost:5000/quiz/getquizes/?userId=${userId}`);
      return data;
   },

   async getByQuiz(quizId: number): Promise<TestInterface> {
      const { data } = await axios.get(`http://localhost:5000/quiz/getquiz/?quizId=${quizId}`);
      return data;
   },

   async add(payload: TestInterface): Promise<TestInterface> {
      const { data } = await axios.post('http://localhost:5000/quiz/addquiz', { payload });
      return data;
   },
};
