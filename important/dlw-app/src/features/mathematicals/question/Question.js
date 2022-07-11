import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import {faCheckCircle, faXmarkCircle} from "@fortawesome/free-solid-svg-icons";
import { useDispatch } from 'react-redux';

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

    let display = 'flex'
    if(props.Display === false){
        display = 'none'
    }

    return (<div className="question-item" key={props.index} style={{display: display}}>
            <div className='question-item-no'>{props.index+1}.</div>
            <div className='question-item-body'>{props.Question}</div>
            <input key="value" className="number-range-field" type="number" value={props.UserAnswer} onChange={e => handleChange(e)}/>
            {props.checkResult?
            (<div className='question-item-check'>{props.Answer === props.UserAnswer?(correct):(wrong) }</div>)
            :""
            }

            {props.showAnswer?
            (<div className='question-item-answer'>Answer: {props.Answer}</div>)
            :""
            }
        </div>)
}

export default Question;