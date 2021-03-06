import React from 'react';
import { useQuery } from 'react-query';
import { useParams } from 'react-router-dom';
import { testsApi } from '../api/testsApi';
import { QuestionInterface } from '../interfaces';

export const Test: React.FC = () => {
   const [term, setTerm] = React.useState<QuestionInterface[]>([]); // all terms keep here
   const [incorrect, setIncorrect] = React.useState<QuestionInterface[]>([]); // wrong answers keep here
   const [text, setText] = React.useState<string>('');
   const [indexArr, setIndexArr] = React.useState<number>(0);
   const [score, setScore] = React.useState<number>(0);
   const [value, setValue] = React.useState<'correct' | 'wrong' | 'end'>();
   const [nextStage, setNextStage] = React.useState<boolean>(false);

   const params: { id: string } = useParams();
   const { data: tests } = useQuery(['tests', params.id], () => testsApi.getByQuiz(parseInt(params.id)));
   React.useEffect(() => {
      if (tests) setTerm(tests.questions.sort(() => Math.random() - 0.5));
   }, [tests]);

   const endScore: number = 100 - Math.round((incorrect.length / term.length) * 100);
   const scorePercent: number = Math.round((score / term.length) * 100);
   const progressbarStyle = { width: scorePercent + '%' };
   const delTerm = (): void => setIncorrect(incorrect.filter((obj: QuestionInterface, i: number) => obj !== incorrect[i]));

   const handleChangeText = (event: React.FormEvent<HTMLInputElement>) => {
      if (event.currentTarget) setText(event.currentTarget.value.toLowerCase().replace(/\s+/g, ' '));
   };

   const checkAnswer = (event: React.FormEvent) => {
      event.preventDefault();
      setText(text.trim());

      if (!text.trim()) return;

      //checking correct answer
      if (text !== term[indexArr].questionAnswer) {
         setIncorrect([...incorrect, term[indexArr]]);
         return setValue('wrong');
      }

      setIncorrect(incorrect.filter((obj: QuestionInterface, index: number) => incorrect.indexOf(obj) === index));

      // checking length end
      if (indexArr < term.length - 1) {
         if (nextStage) delTerm();

         setValue('correct');
         setScore(score + 1);
         setTimeout(() => {
            setIndexArr(indexArr + 1);
            setValue(undefined);
            setText('');
         }, 1350);
      } else {
         if (nextStage) delTerm();

         setValue('correct');
         setScore(score + 1);
         setTimeout(() => {
            setIndexArr(0);
            setText('');
            setValue('end');
         }, 1350);
      }
   };

   const goNextStage = () => {
      setTerm(incorrect);
      setValue(undefined);
      setNextStage(true);
      setScore(0);
   };

   const onRestart = () => window.location.reload();

   return (
      <div className="test">
         <div className="container wrapper">
            {value === 'end' ? (
               <div className="test__end">
                  {incorrect.length > 0 ? (
                     <>
                        <h1>General knowledge of the material {endScore}%</h1>
                        <button onClick={goNextStage}>Continue</button>
                     </>
                  ) : (
                     <>
                        <h1>Congratulations! You have passed all the material!</h1>
                        <button onClick={onRestart}>Restart</button>
                     </>
                  )}
               </div>
            ) : term.length > 0 ? (
               <div
                  className={
                     value === 'correct' ? 'test__block correct' : value === 'wrong' ? 'test__block wrong' : 'test__block'
                  }>
                  <div className="test__progressbar">
                     <div className="test__progressbar__line" style={progressbarStyle}></div>
                  </div>
                  <div className="test__columns">
                     <div className="test__content">
                        <span>{incorrect[indexArr] ? incorrect[indexArr].questionName : term[indexArr].questionName}</span>
                        {value === 'wrong' ? (
                           <>
                              <span className="lineWidth"></span>
                              <span className="correctAnswerIs">correct answer</span>
                              <span>{term[indexArr].questionAnswer}</span>
                           </>
                        ) : undefined}
                     </div>
                     <form onSubmit={checkAnswer} className="sendForm">
                        <input onChange={handleChangeText} value={text} />
                        <button onClick={checkAnswer}>{value === 'correct' ? 'Correct' : 'Answer'}</button>
                     </form>
                  </div>
               </div>
            ) : undefined}
         </div>
      </div>
   );
};
