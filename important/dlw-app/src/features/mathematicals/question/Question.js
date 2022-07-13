import React from 'react';
import PropTypes from 'prop-types';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCheckCircle, faXmarkCircle } from '@fortawesome/free-solid-svg-icons';
import { useDispatch } from 'react-redux';

import {
  updateAnswer,
} from '../reducers/searchBar';

const correct = <FontAwesomeIcon icon={faCheckCircle} style={{ color: 'green', fontSize: '20px' }} />;
const wrong = <FontAwesomeIcon icon={faXmarkCircle} style={{ color: 'red', fontSize: '20px' }} />;

function Question(props) {
  const { data } = props;
  const dispatch = useDispatch();
  const handleChange = (e) => {
    dispatch(updateAnswer({ index: data.index, answer: Number(e.target.value) }));
  };

  let display = 'flex';
  if (data.Display === false) {
    display = 'none';
  }

  return (
    <div className="question-item" key={data.index} style={{ display }}>
      <div className="question-item-no">
        {data.index + 1}
        .
      </div>
      <div className="question-item-body">{data.Question}</div>
      <input key="value" className="number-range-field" type="number" value={data.UserAnswer} onChange={(e) => handleChange(e)} />
      {data.checkResult
        ? (<div className="question-item-check">{data.Answer === data.UserAnswer ? (correct) : (wrong) }</div>)
        : ''}

      {data.showAnswer
        ? (
          <div className="question-item-answer">
            Answer:
            {data.Answer}
          </div>
        )
        : ''}
    </div>
  );
}

Question.propTypes = {
  data: PropTypes.shape({
    index: PropTypes.number,
    checkResult: PropTypes.bool,
    showAnswer: PropTypes.bool,

    Display: PropTypes.bool,
    Question: PropTypes.string,
    Answer: PropTypes.number,
    UserAnswer: PropTypes.number,
  }).isRequired,
};

export default Question;
