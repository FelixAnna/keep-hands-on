import React from 'react';
import Question from "../question/Question";
import { useSelector } from 'react-redux';

import {
    currentQuestions
} from '../reducers/searchBar';

function QuestionList(){
    const questions = useSelector(currentQuestions)

    return (
        <div className="form-style">
        <div>
            {
                questions !== undefined ? 
                questions.map((q,i) => {
                    return <Question key={i} index={i} Question = {q.Question} Answer = {q.Answer} Kind = {q.Kind} FullText={q.FullText}  />
                }):(<div>No data</div>)
            }
        </div>
        </div>)
}

export default QuestionList;