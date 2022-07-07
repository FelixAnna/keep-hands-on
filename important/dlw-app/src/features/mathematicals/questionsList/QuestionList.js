import React from 'react';
import Question from "../question/Question";
import { useSelector, useDispatch } from 'react-redux';

import {
    loadAsync,currentQuestions,currentCriterias
} from '../reducers/searchBar';

function QuestionList(){
    const dispatch = useDispatch();
    const questions = useSelector(currentQuestions)

    return (
        <div>
            <div>
            {
                questions !== undefined ? 
                questions.map((q,i) => {
                    return <Question index={i} Question = {q.Question} Answer = {q.Answer} Kind = {q.Kind} FullText={q.FullText}  />
                }):""
            }
            </div>
            <div>
            <input type="button" value="刷新" onClick={()=>dispatch(loadAsync(null))} />
        </div>
    </div>)
}

export default QuestionList;