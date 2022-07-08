import React from 'react';
import Question from "../question/Question";
import { useSelector, useDispatch } from 'react-redux';
import Button from '@mui/material/Button';
import './QuestionList.css'

import {
    currentQuestions,checkResult,currentShowResult,submitResult,currentScore
} from '../reducers/searchBar';

function QuestionList(){
    const score = useSelector(currentScore)
    const dispatch = useDispatch();
    const questions = useSelector(currentQuestions)
    const showResult = useSelector(currentShowResult)

    return (
        <div className="form-style-right">
        <div>
            {
                questions !== undefined ? 
                questions.map((q,i) => {
                    return <Question key={i} showResult={showResult} index={i} {...q}  />
                }):(<div>No data</div>)
            }
        </div>
        <Button variant="outlined" onClick={() => dispatch(checkResult())} >检查</Button>
        <Button variant="outlined" onClick={() => dispatch(submitResult())} >交卷</Button>
        <div></div>
        <div>Your score:{score}</div>
        </div>)
}

export default QuestionList;