import './SearchBar.css';
import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import Button from '@mui/material/Button';
import {
  clearAll, addCriteria, addCriteriaTemplate,
} from '../reducers/searchBar';
import {
  MathCategory, MathKind, MathType,
} from '../const';

import QuickSet from './QuickSet';

const defaultCriteria = {
  Min: 0,
  Max: 100,
  Quantity: 10,

  ResultMin: 0,
  ResultMax: 100,

  Category: 0,
  Kind: 1,
  Type: 0,
};

function SearchBar() {
  const dispatch = useDispatch();
  const [criteria, updateCriteria] = useState(defaultCriteria);

  const handleChange = (event, type) => {
    const min = 1;
    const max = 1000;

    const value = Number(event.target.value);
    switch (type) {
      case 0:
        updateCriteria({ ...criteria, Category: value });
        break;

      case 1:
        updateCriteria({ ...criteria, Min: value });
        break;
      case 2:
        updateCriteria({ ...criteria, Max: value });
        break;

      case 3:
        updateCriteria({ ...criteria, ResultMin: value });
        break;
      case 4:
        updateCriteria({ ...criteria, ResultMax: value });
        break;

      case 5:
        updateCriteria({ ...criteria, Kind: value });
        break;
      case 6:
        updateCriteria({ ...criteria, Type: value });
        break;
      case 7:
        updateCriteria({ ...criteria, Quantity: Math.max(min, Math.min(max, value)) });
        break;

      default:
        break;
    }
  };

  const getCriteria = () => {
    const current = {
      Min: criteria.Min,
      Max: criteria.Max,
      Quantity: criteria.Quantity,

      Range: {
        Min: criteria.ResultMin,
        Max: criteria.ResultMax,
      },

      Category: criteria.Category,
      Kind: criteria.Kind,
      Type: criteria.Type,
    };

    return current;
  };

  return (
    <div style={{ display: 'flex' }}>
      <div className="math-question-style">
        <div className="math-question-style-heading">快速配置</div>
        <div>
          <div>100以内</div>
          <div>
            <Button variant="outlined" onClick={() => dispatch(addCriteriaTemplate({ category: -1, quantity: 2, max: 100 }))}>任意各出2道</Button>
&nbsp;
            <Button variant="outlined" onClick={() => dispatch(addCriteriaTemplate({ category: -1, quantity: 5, max: 100 }))}>任意各出5道</Button>
&nbsp;
            <Button variant="outlined" onClick={() => dispatch(addCriteriaTemplate({ category: -1, quantity: 10, max: 100 }))}>任意各出10道</Button>
          </div>
          <div>
            <Button variant="outlined" onClick={() => dispatch(addCriteriaTemplate({ category: 0, quantity: 2, max: 100 }))}>加法各出2道</Button>
&nbsp;
            <Button variant="outlined" onClick={() => dispatch(addCriteriaTemplate({ category: 0, quantity: 5, max: 100 }))}>加法各出5道</Button>
&nbsp;
            <Button variant="outlined" onClick={() => dispatch(addCriteriaTemplate({ category: 0, quantity: 10, max: 100 }))}>加法各出10道</Button>
          </div>
          <div>
            <Button variant="outlined" onClick={() => dispatch(addCriteriaTemplate({ category: 1, quantity: 2, max: 100 }))}>减法各出2道</Button>
&nbsp;
            <Button variant="outlined" onClick={() => dispatch(addCriteriaTemplate({ category: 1, quantity: 5, max: 100 }))}>减法各出5道</Button>
&nbsp;
            <Button variant="outlined" onClick={() => dispatch(addCriteriaTemplate({ category: 1, quantity: 10, max: 100 }))}>减法各出10道</Button>
          </div>
          <div>1000以内</div>
          <div>
            <Button variant="outlined" onClick={() => dispatch(addCriteriaTemplate({ category: -1, quantity: 2, max: 1000 }))}>任意各出2道</Button>
&nbsp;
            <Button variant="outlined" onClick={() => dispatch(addCriteriaTemplate({ category: -1, quantity: 5, max: 1000 }))}>任意各出5道</Button>
&nbsp;
            <Button variant="outlined" onClick={() => dispatch(addCriteriaTemplate({ category: -1, quantity: 10, max: 1000 }))}>任意各出10道</Button>
          </div>
          <div>
            <Button variant="outlined" onClick={() => dispatch(addCriteriaTemplate({ category: 0, quantity: 2, max: 1000 }))}>加法各出2道</Button>
&nbsp;
            <Button variant="outlined" onClick={() => dispatch(addCriteriaTemplate({ category: 0, quantity: 5, max: 1000 }))}>加法各出5道</Button>
&nbsp;
            <Button variant="outlined" onClick={() => dispatch(addCriteriaTemplate({ category: 0, quantity: 10, max: 1000 }))}>加法各出10道</Button>
          </div>
          <div>
            <Button variant="outlined" onClick={() => dispatch(addCriteriaTemplate({ category: 1, quantity: 2, max: 1000 }))}>减法各出2道</Button>
&nbsp;
            <Button variant="outlined" onClick={() => dispatch(addCriteriaTemplate({ category: 1, quantity: 5, max: 1000 }))}>减法各出5道</Button>
&nbsp;
            <Button variant="outlined" onClick={() => dispatch(addCriteriaTemplate({ category: 1, quantity: 10, max: 1000 }))}>减法各出10道</Button>
          </div>
        </div>
      </div>
      <div>
        <div className="math-question-style-heading">快速配置</div>
        <div>
          <QuickSet />
        </div>
      </div>
      <div className="math-question-style">
        <div className="math-question-style-heading">配置题目生成模板</div>
        <div>
          <div>
            <span>算术类型:</span>
            <select className="select-field" value={criteria.Category} onChange={(e) => handleChange(e, 0)}>
              {MathCategory.map((op) => <option key={op.key} value={op.key}>{op.text}</option>)}
            </select>
          </div>
          <div>
            <span>数字范围:</span>
            <input className="number-range-field" type="number" value={criteria.Min} onChange={(e) => handleChange(e, 1)} />
            -
            <input className="number-range-field" type="number" value={criteria.Max} onChange={(e) => handleChange(e, 2)} />
          </div>
          <div>
            <span>结果范围:</span>
            <input className="number-range-field" type="number" value={criteria.ResultMin} onChange={(e) => handleChange(e, 3)} />
            -
            <input className="number-range-field" type="number" value={criteria.ResultMax} onChange={(e) => handleChange(e, 4)} />
          </div>
          <div>
            <span>求值类型:</span>
            <select className="select-field" value={criteria.Kind} onChange={(e) => handleChange(e, 5)}>
              {MathKind.map((op) => <option key={op.key} value={op.key}>{op.text}</option>)}
            </select>
          </div>
          <div>
            <span>输出格式:</span>
            <select className="select-field" value={criteria.Type} onChange={(e) => handleChange(e, 6)}>
              {MathType.map((gp) => (
                <optgroup label={gp.text} key={gp.key}>
                  {gp.options.map((op) => <option key={op.key} value={op.key}>{op.text}</option>)}
                </optgroup>
              ))}
            </select>
          </div>
          <div>
            <span>题目数量:</span>
            <input className="input-field" type="number" min="1" max="1000" value={criteria.Quantity} onChange={(e) => handleChange(e, 7)} />
          </div>
          <div>
            <input type="submit" value="加入队列" onClick={() => dispatch(addCriteria(getCriteria()))} />
            <input type="button" value="清除所有" onClick={() => dispatch(clearAll())} />
          </div>
        </div>
      </div>
    </div>
  );
}

export default SearchBar;
