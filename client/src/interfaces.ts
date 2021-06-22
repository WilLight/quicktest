export interface TestInterface {
   ownerId: number;
   quizId: number;
   quizName: string;
   studentIds: number[];
   questions: QuestionInterface[];
}

export interface QuestionInterface {
   questionId: number;
   questionName: string;
   questionAnswer: string;
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
   roomId: number;
   roomName: string;
   teacherId: number;
   studentIds?: number[];
   inviteToken: string;
}
