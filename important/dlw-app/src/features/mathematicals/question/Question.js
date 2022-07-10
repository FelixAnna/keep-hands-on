import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import {faCheckCircle, faXmarkCircle} from "@fortawesome/free-solid-svg-icons";
import { useSelector, useDispatch } from 'react-redux';

import {
    updateAnswer
} from '../reducers/searchBar';

const correct = <FontAwesomeIcon icon={faCheckCircle} style={{color:'green', fontSize: '20px'}}/>
const wrong =<FontAwesomeIcon icon={faXmarkCircle} style={{color:'red', fontSize: '20px'}}/>


const Question = (props) => {
    const dispatch =useDispatch()

    const handleChange = (e) => {
        const value =Number(e.target.value)
        dispatch(updateAnswer({index: props.index, answer: value}))
    }

    return (
        <div className="question-item" key={props.index}>
            <span>{props.index+1}.</span>
            <span>{props.Question}</span>
            <input key="value" className="number-range-field" type="number" onChange={e => handleChange(e)}/>
            {props.checkResult?
            (<text>{props.Answer == props.UserAnswer?(correct):(wrong) }</text>)
            :""
            }

            {props.showAnswer?
            (<text>Answer: {props.Answer}</text>)
            :""
            }
        </div>)
}

export default Question;