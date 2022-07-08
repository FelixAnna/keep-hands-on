import React from 'react';
import Criteria from "../criteria/Criteria";
import { useSelector, useDispatch } from 'react-redux';
import './CriteriaList.css';

import {
    loadAsync,currentCriterias
} from '../reducers/searchBar';

function CriteriaList(){
    const dispatch = useDispatch();
    const criterias = useSelector(currentCriterias)

    return (
        <div className="form-style-right">
        <div className="form-style-right-heading">配置列表</div>
        <div>
            <div key ="head">
                <span>No.</span>
                <span>数字范围</span>
                <span>结果范围</span>
                <span>题目数量</span>
                <span>算术类型</span>
                <span>求值类型</span>
                <span>输出格式</span>
            </div>
            {
                criterias !== undefined ? 
                criterias.map((q,i) => {
                    return <Criteria index={i} {...q}  />
                }):(<div>No data</div>)
            }
            <div>
                <input type="button" value="刷新" onClick={()=>dispatch(loadAsync(null))} />
            </div>
        </div>
        </div>)
}

export default CriteriaList;