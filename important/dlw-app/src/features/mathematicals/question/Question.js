import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import {faCheckCircle, faXmarkCircle} from "@fortawesome/free-solid-svg-icons";

const correct = <FontAwesomeIcon icon={faCheckCircle} style={{color:'green', fontSize: '20px'}}/>
const wrong =<FontAwesomeIcon icon={faXmarkCircle} style={{color:'red', fontSize: '20px'}}/>

class Question extends React.Component{
    constructor(props){
        super(props);
        this.state = {
            answer: "",
            correct: false,
        }

        this.handleChange = this.handleChange.bind(this)
    }
    
    handleChange (event){
        const value =Number(event.target.value)
        this.setState({
            answer: value,
            correct: value === this.props.Answer
        })
    }

    render(){
        

        return (
            <div key={this.props.index}>
                <span>{this.props.index+1}.</span>
                <span>{this.props.Question}</span>
                <input key="value" className="number-range-field" type="number" value={this.state.value} onChange={this.handleChange}/>
                {this.props.showResult?
                (<span>{this.state.correct?(correct):(wrong)} Answer: {this.props.Answer}</span>)
                :""
                }
            </div>)
    }
}

export default Question;