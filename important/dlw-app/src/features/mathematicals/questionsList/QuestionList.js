import React from 'react';
import Question from "../question/Question";
import { useSelector, useDispatch } from 'react-redux';
import Button from '@mui/material/Button';
import './QuestionList.css'

import {
    currentQuestions,checkResult,showAnswer,currentCheckResult,currentShowAnswer,submitResult,currentScore
} from '../reducers/searchBar';

function QuestionList(){
    const score = useSelector(currentScore)
    const dispatch = useDispatch();
    const questions = useSelector(currentQuestions)
    const result = useSelector(currentCheckResult)
    const answer = useSelector(currentShowAnswer)

    let qestionList = []
    if (questions!== undefined){
        qestionList = questions.map((q,i) => {
            return <Question key={i} checkResult={result} showAnswer={answer} index={i} {...q}  />
        })
    }

    return (
        <div className="form-style-right">
        <div className="question-root">
            {
                questions !== undefined ? qestionList:(<div>No data</div>)
            }
        </div>
        <Button variant="outlined" className='buttons' onClick={() => dispatch(showAnswer())} >答案</Button>
        <Button variant="outlined"  className='buttons' onClick={() => dispatch(checkResult())} >检查</Button>
        <Button variant="outlined"  className='buttons' onClick={() => dispatch(submitResult())} >交卷</Button>
        <div></div>
        <div>Your score:{score}</div>
        </div>)
}

export default QuestionList;