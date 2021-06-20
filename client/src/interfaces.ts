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
