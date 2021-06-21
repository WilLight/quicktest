import axios from 'axios';

export const classroomApi = {

   async get(classroomId: number): Promise<any> {
      const { data } = await axios.post('http://localhost:5000/classroom/getdetails', {
         classroomId: classroomId
      });

      return data;
   },

   async getByMyId(userId: number): Promise<any> {
      const { data } = await axios.get('http://localhost:5000/classroom/getbyuserid', {
         params: {
            userId: userId
         }
      });

      return data;
   }
};
