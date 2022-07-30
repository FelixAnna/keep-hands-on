import './index.css';
import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import MultipleSelectCheckmarks from './select';
import { loadAsync, clearAll } from '../reducer';

const defaultCriteria = {
  Criteria: {
    Distrct: '',
    Keywords: '',

    MinPrice: 0,
    MaxPrice: 0,

    SortKey: '',

    Page: 1,
    Size: 10,
  },
  ZdjItems: [],
};
function SearchBar() {
  const dispatch = useDispatch();
  const [criteria, updateCriteria] = useState(defaultCriteria);

  const handleChange = (event, type) => {
    const value = Number(event.target.value);
    switch (type) {
      case 0:
        updateCriteria({ ...criteria, Category: value });
        break;
      case 1:
        updateCriteria({ ...criteria, Min: value });
        break;
      default:
        break;
    }
  };
  const getCriteria = () => {
    const current = {
      Distrct: criteria.Min,
      Keywords: criteria.Max,

      MinPrice: criteria.Quantity,
      MaxPrice: criteria.Quantity,

      SortKey: criteria.SortKey,

      Page: criteria.Page,
      Size: criteria.Size,
    };

    return current;
  };

  // Distrct/Street/Community/MinPrice/MaxPrice/Version/SortKey/Page/Size
  return (
    <div style={{ display: 'flex' }}>
      <div className="form-style">
        <div className="form-style-heading">Search</div>
        <div>
          <div>
            <span>Distrct:</span>
            <MultipleSelectCheckmarks />
          </div>
          <div>
            <span>Price:</span>
            <input className="number-range-field" type="number" value={criteria.Min} onChange={(e) => handleChange(e, 1)} />
            -
            <input className="number-range-field" type="number" value={criteria.Max} onChange={(e) => handleChange(e, 2)} />
          </div>
          <div>
            <input type="submit" value="加入队列" onClick={() => dispatch(loadAsync(getCriteria()))} />
            <input type="button" value="清除所有" onClick={() => dispatch(clearAll())} />
          </div>
        </div>
      </div>
    </div>
  );
}

export default SearchBar;
