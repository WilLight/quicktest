import { useQuery } from 'react-query';
import { Link } from 'react-router-dom';
import { testsApi } from '../api/testsApi';
import { UserInterface } from '../interfaces';
import { sessionMemory } from '../utils/sessionMemory';

export const Tests = () => {
   const user: UserInterface = sessionMemory.get('userdata');
   const { data: tests } = useQuery(['tests', user.id], () => testsApi.getByUser(user.id));

   return (
      <div className="modules">
         <div className="">
            <h1>Your tests</h1>
         </div>
         {tests
            ? tests.map((content) => (
                 <Link to={`/t/${content.quizId}`} className="modules__card">
                    <h3>Module {content.quizName}</h3>
                    <span>{content.questions.length} Terms</span>
                 </Link>
              ))
            : undefined}
      </div>
   );
};
