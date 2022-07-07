import './SearchBar.css';
import React, {useState} from 'react';
import { useDispatch } from 'react-redux';
import {
    clearAll, saveCriterias
} from '../reducers/searchBar';

const defaultCriteria = {
    Min: 0, Max: 100,
    Quantity: 10,

    ResultMin: 0,
    ResultMax:100,

    Category: 0, 
    Kind: 1, 
    Type: 0
};

function SearchBar(){
    const dispatch = useDispatch();
    const state = defaultCriteria;
    const [criteria, updateCriteria] = useState(defaultCriteria);

    const handleChange = (event, type) => {
        const min = 1;
        const max = 1000;

        const value =Number(event.target.value)
        switch (type) {
            case 0:
                updateCriteria({...criteria, Category: value});
                break;

            case 1:
                updateCriteria({...criteria, Min: value});
                break;
            case 2:
                updateCriteria({...criteria, Max: value});
                break;

            case 3:
                updateCriteria({...criteria, ResultMin: value});
                break;
            case 4:
                updateCriteria({...criteria, ResultMax: value});
                break;
            
            case 5:
                updateCriteria({...criteria, Kind: value});
                break;
            case 6:
                updateCriteria({...criteria, Type: value});
                break;
            case 7:
                const quantity = Math.max(min, Math.min(max, value));
                updateCriteria({...criteria, Quantity: quantity});
                break;

            default:
                break;
        }
    }

    const getCriteria = () => {
        const current = {
                Min: criteria.Min, 
                Max: criteria.Max,
                Quantity: criteria.Quantity,

                Range:{
                    Min: criteria.ResultMin,
                    Max: criteria.ResultMax,
                },

                Category: criteria.Category, 
                Kind: criteria.Kind, 
                Type: criteria.Type
        }

        console.log(current)
        return current
    }
      
    return (
        <div className="form-style">
        <div className="form-style-heading">配置题目生成模板</div>
        <div>
            <div>
                <span>算术类型:</span>
                <select className="select-field" value={state.Category} onChange={(e)=>  handleChange(e, 0)} >
                    <option value="0">加法</option>
                    <option value="1">减法</option>
                </select>
            </div>
            <div>
                <span>数字范围:</span>
                <input className="number-range-field" type="number" value={criteria.Min} onChange={(e)=> handleChange(e, 1)}/>
                -
                <input className="number-range-field" type="number" value={criteria.Max} onChange={(e)=> handleChange(e, 2)}/>
            </div>
            <div>
                <span>结果范围:</span>
                <input className="number-range-field" type="number" value={criteria.ResultMin} onChange={(e)=> handleChange(e, 3)}/>
                -
                <input className="number-range-field" type="number" value={criteria.ResultMax} onChange={(e)=> handleChange(e, 4)}/>
            </div>
            <div>
                <span>求值类型:</span>
                <select className="select-field" value={criteria.Kind} onChange={(e)=> handleChange(e, 5)} >
                    <option value="1">第一个数：? * b= c </option>
                    <option value="2">第二个数: a * ?= c </option>
                    <option value="3">结果: a * b= ? </option>
                </select>
            </div>
            <div>
                <span>输出格式:</span>
                <select className="select-field" value={criteria.Type} onChange={(e)=> handleChange(e, 6)} >
                    <option value="0">算术表达式: a+b=c</option>
                    <option value="1">应用题: 比a多b的数是c</option>
                </select>
            </div>
            <div>
                <span>题目数量:</span>
                <input className="input-field" type="number" min="1" max="1000" value={criteria.Quantity} onChange={(e)=> handleChange(e, 7)} />
            </div>
            <div>
                <input type="submit" value="加入队列" onClick = {() =>dispatch(saveCriterias(getCriteria())) } />
                <input type="button" value="清除所有" onClick = {() =>dispatch(clearAll()) } />
            </div>
        </div>
        </div>
      )
}

export default SearchBar;