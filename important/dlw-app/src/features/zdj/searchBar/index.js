import './index.css';
import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import InputAdornment from '@mui/material/InputAdornment';
import TextField from '@mui/material/TextField';
import MultipleSelectCheckmarks from './select';
import { saveCriteria, clearAll } from '../reducer';

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

  const handleChange = (prop) => (event) => {
    updateCriteria({ ...criteria, [prop]: event.target.value });
  };

  const getCriteria = () => {
    const current = {
      Distrct: criteria.Distrct,
      Keywords: criteria.Keywords,

      MinPrice: criteria.MinPrice,
      MaxPrice: criteria.MaxPrice,

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
            <MultipleSelectCheckmarks />
          </div>
          <div>
            <TextField
              label="最低价"
              id="outlined-adornment-max"
              sx={{ m: 1, width: '25ch' }}
              value={criteria.MinPrice}
              onChange={handleChange('MinPrice')}
              InputProps={{
                endAdornment: <InputAdornment position="end">RMB</InputAdornment>,
                inputMode: 'numeric',
                pattern: '[0-9]*',
              }}
            />
          </div>
          <div>
            <TextField
              label="最高价"
              id="outlined-adornment-max"
              sx={{ m: 1, width: '25ch' }}
              value={criteria.MaxPrice}
              onChange={handleChange('MaxPrice')}
              InputProps={{
                endAdornment: <InputAdornment position="end">RMB</InputAdornment>,
                inputMode: 'numeric',
                pattern: '[0-9]*',
              }}
            />
          </div>
          <div>
            <TextField
              label="关键字"
              id="outlined-adornment-keywords"
              sx={{ m: 1, width: '25ch' }}
              value={criteria.Keywords}
              onChange={handleChange('Keywords')}
              InputProps={{
                endAdornment: <InputAdornment position="end">RMB</InputAdornment>,
              }}
            />
          </div>
          <div>
            <input type="submit" value="加入队列" onClick={() => dispatch(saveCriteria(getCriteria()))} />
            <input type="button" value="清除所有" onClick={() => dispatch(clearAll())} />
          </div>
        </div>
      </div>
    </div>
  );
}

export default SearchBar;
