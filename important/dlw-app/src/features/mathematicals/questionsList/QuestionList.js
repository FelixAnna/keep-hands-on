import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import Button from '@mui/material/Button';
import Question from '../question/Question';
import './QuestionList.css';

import {
  currentQuestions, updateShowResult, updateShowAnswer, currentShowResult,
  currentShowAnswer, submitResult, currentScore,
} from '../reducers/searchBar';

function QuestionList() {
  const score = useSelector(currentScore);
  const dispatch = useDispatch();
  const questions = useSelector(currentQuestions);
  const result = useSelector(currentShowResult);
  const answer = useSelector(currentShowAnswer);

  let qestionList = [];
  if (questions !== undefined) {
    qestionList = questions.map((q, i) => {
      const data = { ...q };
      data.showResult = result;
      data.showAnswer = answer;
      data.index = i;

      return <Question key={data.index} data={data} />;
    });
  }

  return (
    <div className="question-list-style-right">
      <div className="question-root">
        {
            questions !== undefined ? qestionList : (<div>No data</div>)
        }
      </div>
      <Button variant="outlined" className="buttons_hidden" onClick={() => dispatch(updateShowAnswer())}>答案</Button>
      <Button variant="outlined" className="buttons" onClick={() => dispatch(updateShowResult())}>检查</Button>
      <Button variant="outlined" className="buttons" onClick={() => dispatch(submitResult())}>交卷</Button>
      <div />
      <div>
        Your score:
        {score}
      </div>
    </div>
  );
}

export default QuestionList;
