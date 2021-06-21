export interface TestInterface {
   id: string;
   question: string;
   answer: string;
}

export interface UserInterface {
   id: number;
   credentials: {
      login: string;
      password: string;
   };
   userRole: number;
}

export interface ClassroomInterface {
   readonly roomId: number;
   readonly teacherId: number;
   readonly studentIds?: number[];
   readonly inviteToken: string;
}
