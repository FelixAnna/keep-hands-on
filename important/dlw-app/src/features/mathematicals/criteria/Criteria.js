import React from 'react';
import {MathCategory, MathKind, MathType} from '../const'

class Criteria extends React.Component{
    constructor(props){
        super(props);
        this.convertToText = this.convertToText.bind(this)
    }

    convertToText(value, type){
       let result =""
        switch (type) {
            case "Category":
                result = MathCategory.find(op=> op.value === value).text
                break;
            case "Kind":
                result = MathKind.find(op=> op.value === value).text
                break;
            case "Type":
                MathType.forEach(group=> {
                    const option = group.options.find(op=>op.value === value)
                    if(option !== undefined){
                        result = option.text
                    }
                })
                break;
        
            default:
                break;
        }

        return result
    }

    render(){
        return (
            <div key={this.props.index}>
                <span>{this.props.index+1}.</span>
                <span>{this.props.Min}~{this.props.Max}</span>
                <span>{this.props.Range.Min}~{this.props.Range.Max}</span>
                <span>{this.props.Quantity}</span>
                <span>{this.convertToText(this.props.Category,"Category")}</span>
                <span>{this.convertToText(this.props.Kind,"Kind")}</span>
                <span>{this.convertToText(this.props.Type,"Type")}</span>
            </div>)
    }
}

export default Criteria;