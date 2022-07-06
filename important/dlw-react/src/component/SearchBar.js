import React from 'react';
import './SearchBar.css';

class SearchBar extends React.Component{
    constructor(props) {
        super(props);
        this.state = {
            Min: 0, Max: 100,
            Quantity: 10,

            ResultMin: 0,
            ResultMax:100,

            Category: 0, 
            Kind: 1, 
            Type: 0
        };

        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleQueue = this.handleQueue.bind(this);
        this.handleResultMaxChange = this.handleChange.bind(this);
    }

    handleChange(event, type) {
        //console.log(event.target.value, type)
        switch (type) {
            case 0:
                this.setState({Category: event.target.value});
                break;

            case 1:
                this.setState({Min: event.target.value});
                break;
            case 2:
                this.setState({Max: event.target.value});
                break;

            case 3:
                this.setState({ResultMin: event.target.value});
                break;
            case 4:
                this.setState({ResultMax: event.target.value});
                break;
            
            case 5:
                this.setState({Kind: event.target.value});
                break;
            case 6:
                this.setState({Type: event.target.value});
                break;
            case 7:
                this.setState({Quantity: event.target.value});
                break;

            default:
                break;
        }
        
    }

    handleSubmit(event) {
        alert('A request was submitted: ' + JSON.stringify(this.state));
        event.preventDefault();
    }

    handleQueue(event) {
        alert('A request was queued: ' + JSON.stringify(this.state));
        event.preventDefault();
    }
      
    render(){
        return (
            <div className="form-style">
            <div className="form-style-heading">配置题目生成模板</div>
                <form onSubmit={this.handleSubmit}>
                    <div>
                        <span>算术类型:</span>
                        <select className="select-field" value={this.state.Category} onChange={(e)=> this.handleChange(e, 0)} >
                            <option value="0">加法</option>
                            <option value="1">减法</option>
                        </select>
                    </div>
                    <div>
                        <span>数字范围:</span>
                        <input className="number-range-field" type="number" value={this.state.Min} onChange={(e)=> this.handleChange(e, 1)}/>
                        -
                        <input className="number-range-field" type="number" value={this.state.Max} onChange={(e)=> this.handleChange(e, 2)}/>
                    </div>
                    <div>
                        <span>结果范围:</span>
                        <input className="number-range-field" type="number" value={this.state.ResultMin} onChange={(e)=> this.handleChange(e, 3)}/>
                        -
                        <input className="number-range-field" type="number" value={this.state.ResultMax} onChange={(e)=> this.handleChange(e, 4)}/>
                    </div>
                    <div>
                        <span>求值类型:</span>
                        <select className="select-field" value={this.state.Kind} onChange={(e)=> this.handleChange(e, 5)} >
                            <option value="1">第一个数：? * b= c </option>
                            <option value="2">第二个数: a * ?= c </option>
                            <option value="3">结果: a * b= ? </option>
                        </select>
                    </div>
                    <div>
                        <span>输出格式:</span>
                        <select className="select-field" value={this.state.Type} onChange={(e)=> this.handleChange(e, 6)} >
                            <option value="0">算术表达式: a+b=c</option>
                            <option value="1">应用题: 比a多b的数是c</option>
                        </select>
                    </div>
                    <div>
                        <span>题目数量:</span>
                        <input className="input-field" type="number" min="1" max="1000" value={this.state.Quantity} onChange={(e)=> this.handleChange(e, 7)} />
                    </div>
                    <div>
                        <input type="submit" value="立即生成" />
                        &nbsp;&nbsp;&nbsp;
                        <input type="button" value="加入队列" onClick={this.handleQueue} />
                    </div>
                </form>
            </div>
          )
    }
}

export default SearchBar;