import React from 'react';

class Question extends React.Component{
     
    render(){
        return (<div key={this.props.index}>
                <p>{this.props.Question}</p>
                <p>{this.props.Answer}</p>
                <p>{this.props.Kind}</p>
                <p>{this.props.FullText}</p>
            </div>)
    }
}

export default Question;