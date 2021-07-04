import React from 'react';
import { useMutation, useQuery } from 'react-query';
import { Link, useParams } from 'react-router-dom';
import { classroomApi } from '../api/classroomApi';
import { useForm } from 'react-hook-form';
import * as yup from 'yup';
import { yupResolver } from '@hookform/resolvers/yup';
import { toastify } from '../utils/toastify';
import { testsApi } from '../api/testsApi';

const StudentSchema = yup.object().shape({
   classroomId: yup.number(),
   studentId: yup.number(),
});

const QuizSchema = yup.object().shape({
   classroomId: yup.number(),
   quizId: yup.number(),
});

export const Classroom = () => {
   const params: { id: string } = useParams();

   const [modalActive, setModalActive] = React.useState<'student' | 'test' | 'info'>();
   const closeModal = () => setModalActive(undefined);
   const setActiveModal = (payload: 'student' | 'test' | 'info') => setModalActive(payload);

   const { data: classrooms } = useQuery(['classroom', params.id], () => classroomApi.getDetails(parseInt(params.id)));
   const { data: tests } = useQuery(['tests', params.id], () => testsApi.getByClassroom(parseInt(params.id)));

   return (
      <>
         {classrooms ? (
            <div className="dashboard">
               <div className="dashboard__name">
                  <h1>Classroom</h1>
               </div>
               <div className="dashboard__header">
                  <div className="dashboard__profile flex-column">
                     <span>{classrooms.roomName}</span>
                     <span>Students: {classrooms.studentsIds?.length ? classrooms.studentsIds.length : 0}</span>
                  </div>
                  <div className="dashboard__items">
                     <div>
                        <button className="dashboard__button dashboard__button--active">Tests</button>
                        <button className="dashboard__button">Students</button>
                        <button onClick={() => setActiveModal('info')} className="dashboard__button">
                           Info
                        </button>
                     </div>
                     <div>
                        <button onClick={() => setActiveModal('student')} className="dashboard__button">
                           Add student
                        </button>
                        <button onClick={() => setActiveModal('test')} className="dashboard__button">
                           Add test
                        </button>
                     </div>
                  </div>
               </div>
               <ul className="dashboard__content">
                  {tests ? (
                     tests.map((content) => (
                        <li className="modules__card">
                           <Link to={`/t/${content.quizId}`} className="modules__card">
                              <h3>Module {content.quizName}</h3>
                              <p>ID: {content.quizId}</p>
                              <span>{content.questions.length ? content.questions.length : 0} Term(s)</span>
                           </Link>
                        </li>
                     ))
                  ) : (
                     <p style={{ marginTop: '25px' }}>There are no tests..</p>
                  )}
               </ul>
            </div>
         ) : undefined}
         <div className={modalActive ? 'overlay overlay--visible' : 'overlay'}></div>
         <div className={modalActive === 'info' ? 'modal modal--visible' : 'modal'}>
            <div className="modal__header">
               <h2>Info about classroom</h2>
               <button onClick={closeModal} className="minibutton" type="button" title="close" aria-label="close">
                  <span className="minibutton__content minibutton__content--white">
                     <svg xmlns="http://www.w3.org/2000/svg" width="1.5em" height="1.5em" fill="currentColor" viewBox="0 0 16 16">
                        <path
                           fillRule="evenodd"
                           d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z"
                        />
                     </svg>
                  </span>
               </button>
            </div>
            <div className="modal__block">
               <div className="modal__message">there are must be info</div>
            </div>
         </div>
         <div className={modalActive === 'student' ? 'modal modal--visible' : 'modal'}>
            <div className="modal__header">
               <h2>Invite students</h2>
               <button onClick={closeModal} className="minibutton" type="button" title="close" aria-label="close">
                  <span className="minibutton__content minibutton__content--white">
                     <svg xmlns="http://www.w3.org/2000/svg" width="1.5em" height="1.5em" fill="currentColor" viewBox="0 0 16 16">
                        <path
                           fillRule="evenodd"
                           d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z"
                        />
                     </svg>
                  </span>
               </button>
            </div>
            <div className="modal__block">
               <div className="modal__message">To invite students to this class, add their QuickTest userId below.</div>
               <StudentSubmitForm classroomId={parseInt(params.id)} />
            </div>
         </div>
         <div className={modalActive === 'test' ? 'modal modal--visible' : 'modal'}>
            <div className="modal__header">
               <h2>Add test</h2>
               <button onClick={closeModal} className="minibutton" type="button" title="close" aria-label="close">
                  <span className="minibutton__content minibutton__content--white">
                     <svg xmlns="http://www.w3.org/2000/svg" width="1.5em" height="1.5em" fill="currentColor" viewBox="0 0 16 16">
                        <path
                           fillRule="evenodd"
                           d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z"
                        />
                     </svg>
                  </span>
               </button>
            </div>
            <div className="modal__block">
               <div className="modal__message">To add tests to this class, add their QuickTest id below.</div>
               <QuizSubmitForm classroomId={parseInt(params.id)} />
            </div>
         </div>
      </>
   );
};

const StudentSubmitForm: React.FC<{ classroomId: number }> = ({ classroomId }) => {
   const { handleSubmit, register } = useForm({ resolver: yupResolver(StudentSchema) });
   const { mutateAsync: callAddUser } = useMutation(classroomApi.addUser);
   const onSubmitStudent = async (data: { classroomId: number; userId: number }) => {
      try {
         await callAddUser(data);
         toastify(`successfully added user by id ${data.userId}`);
      } catch (error) {
         toastify('error with invite student');
      }
   };

   return (
      <form onSubmit={handleSubmit(onSubmitStudent)} className="flex-row justify-between">
         <div className="input">
            <input type="hidden" defaultValue={classroomId} {...register('classroomId')} />
            <input type="text" required {...register('userId')} />
         </div>
         <button type="submit" className="button button--transparent">
            <span>Invite</span>
         </button>
      </form>
   );
};

const QuizSubmitForm: React.FC<{ classroomId: number }> = ({ classroomId }) => {
   const { handleSubmit, register } = useForm({ resolver: yupResolver(QuizSchema) });
   const { mutateAsync: callAddQuiz } = useMutation(classroomApi.addQuiz);
   const onSubmitQuiz = async (data: { classroomId: number; quizId: number }) => {
      try {
         await callAddQuiz(data);
         toastify(`successfully added quiz by id ${data.quizId}`);
      } catch (error) {
         toastify('error with add quiz');
      }
   };

   return (
      <form onSubmit={handleSubmit(onSubmitQuiz)} className="flex-row justify-between">
         <div className="input">
            <input type="hidden" defaultValue={classroomId} {...register('classroomId')} />
            <input type="text" required {...register('quizId')} />
         </div>
         <button type="submit" className="button button--transparent">
            <span>Add</span>
         </button>
      </form>
   );
};
