import React from 'react';
import Question from "../question/Question";
import { useSelector, useDispatch } from 'react-redux';

import {
    loadAsync,currentQuestions
} from '../reducers/searchBar';

function QuestionList(){
    const dispatch = useDispatch();
    const questions = useSelector(currentQuestions)

    return (
        <div className="form-style">
        <div className="form-style-heading">题目列表</div>
        <div>
            {
                questions !== undefined ? 
                questions.map((q,i) => {
                    return <Question key={i} index={i} Question = {q.Question} Answer = {q.Answer} Kind = {q.Kind} FullText={q.FullText}  />
                }):(<div>No data</div>)
            }
            <div>
                <input type="button" value="刷新" onClick={()=>dispatch(loadAsync(null))} />
            </div>
        </div>
        </div>)
}

export default QuestionList;