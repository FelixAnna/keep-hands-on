import React from 'react';
import Question from "../question/Question";
import { useSelector, useDispatch } from 'react-redux';
import Button from '@mui/material/Button';
import './QuestionList.css'

import {
    currentQuestions,checkResult,currentShowResult
} from '../reducers/searchBar';

function QuestionList(){
    
    const dispatch = useDispatch();
    const questions = useSelector(currentQuestions)
    const showResult = useSelector(currentShowResult)

    return (
        <div className="form-style-right">
        <div>
            {
                questions !== undefined ? 
                questions.map((q,i) => {
                    return <Question key={i} showResult={showResult} index={i} Question = {q.Question} Answer = {q.Answer} Kind = {q.Kind} FullText={q.FullText}  />
                }):(<div>No data</div>)
            }
        </div>
        <Button variant="outlined" onClick={() => dispatch(checkResult())} >交卷</Button>
        </div>)
}

export default QuestionList;