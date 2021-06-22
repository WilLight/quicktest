import React from 'react';
import { useForm } from 'react-hook-form';
import * as yup from 'yup';
import { yupResolver } from '@hookform/resolvers/yup';
import { testsApi } from '../api/testsApi';
import { useMutation } from 'react-query';
import { toastify } from '../utils/toastify';
import { sessionMemory } from '../utils/sessionMemory';
import { UserInterface } from '../interfaces';

const TestSchema = yup.object().shape({
   ownerId: yup.number().required(),
   quizName: yup.string().required(),
   questions: yup.array().of(
      yup.object().shape({
         questionId: yup.number().required(),
         questionName: yup.string().required(),
         questionAnswer: yup.string().required(),
      }),
   ),
});

export const CreateTest = () => {
   const {
      handleSubmit,
      register,
      formState: { errors },
   } = useForm({ resolver: yupResolver(TestSchema) });
   const [termCount, setTermCount] = React.useState<number>(1);
   const incTermCount = () => setTermCount(termCount + 1);
   const user: UserInterface = sessionMemory.get('userdata');

   React.useEffect(() => console.log(errors), [errors]);

   const { mutateAsync } = useMutation(testsApi.add);
   const onSubmit = async (data: any) => {
      console.log(data);
      try {
         mutateAsync(data);
         toastify('successfully created test');
      } catch (error) {
         toastify('error with create test');
      }
   };

   return (
      <form onSubmit={handleSubmit(onSubmit)}>
         <input type="hidden" required {...register('ownerId')} defaultValue={user ? user.id : 0} />
         <div className="create__block">
            <div className="input input--white">
               <span>Test Name</span>
               <input type="text" required {...register('quizName')} />
            </div>
         </div>

         {Array(termCount)
            .fill(0)
            .map((_, index) => (
               <div key={index} className="create__block">
                  <input type="hidden" required {...register(`questions[${index}].questionId`)} defaultValue={index} />
                  <div className="create__qa">
                     <div className="input input--white">
                        <span>Term</span>
                        <input type="text" required {...register(`questions[${index}].questionName`)} />
                     </div>
                     <div className="input input--white">
                        <span>Defitinion</span>
                        <input type="text" required {...register(`questions[${index}].questionAnswer`)} />
                     </div>
                  </div>
               </div>
            ))}

         <div className="create__block">
            <button onClick={incTermCount} type="button" className="button button--transparent">
               <span>Add term</span>
            </button>
         </div>
         <div className="create__block">
            <button type="submit" className="button button--transparent">
               <span>Create Test</span>
            </button>
         </div>
      </form>
   );
};
