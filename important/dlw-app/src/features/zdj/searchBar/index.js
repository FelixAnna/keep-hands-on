import './index.css';
import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import InputAdornment from '@mui/material/InputAdornment';
import TextField from '@mui/material/TextField';
import MultipleSelectCheckmarks from './select';
import { saveCriteria, clearAll } from '../reducer';

const defaultCriteria = {
  Districts: [],
  Keywords: '',

  MinPrice: '',
  MaxPrice: '',

  SortKey: '',

  Page: 1,
  Size: 10,
};

function SearchBar() {
  const dispatch = useDispatch();
  const [criteria, updateCriteria] = useState(defaultCriteria);
  const [district, updateDistrict] = useState([]);

  const handleChange = (prop) => (event) => {
    updateCriteria({ ...criteria, [prop]: event.target.value });
  };

  const handleDistrictChange = (event) => {
    const {
      target: { value },
    } = event;
    updateDistrict(
      // On autofill we get a stringified value.
      typeof value === 'string' ? value.split(',') : value,
    );
  };
  const reset = () => {
    updateCriteria({ ...defaultCriteria });
    updateDistrict([]);
    dispatch(clearAll());
  };
  const search = () => {
    const criteria2 = { ...criteria, Districts: district };
    dispatch(saveCriteria(criteria2));
  };

  // Distrct/Street/Community/MinPrice/MaxPrice/Version/SortKey/Page/Size
  return (
    <div style={{ display: 'flex' }}>
      <div className="zdj-style">
        <div className="zdj-style-heading">Search</div>
        <div>
          <div>
            <MultipleSelectCheckmarks
              handleChange={handleDistrictChange}
              district={district}
            />
          </div>
          <div>
            <TextField
              label="最低单价"
              id="outlined-adornment-max"
              sx={{ m: 1, width: '18ch' }}
              value={criteria.MinPrice}
              onChange={handleChange('MinPrice')}
              InputProps={{
                endAdornment: <InputAdornment position="end">¥/㎡</InputAdornment>,
                inputMode: 'numeric',
                pattern: '[0-9]*',
              }}
            />
          </div>
          <div>
            <TextField
              label="最高单价"
              id="outlined-adornment-max"
              sx={{ m: 1, width: '18ch' }}
              value={criteria.MaxPrice}
              onChange={handleChange('MaxPrice')}
              InputProps={{
                endAdornment: <InputAdornment position="end">¥/㎡</InputAdornment>,
                inputMode: 'numeric',
                pattern: '[0-9]*',
              }}
            />
          </div>
          <div>
            <TextField
              label="关键字"
              id="outlined-adornment-keywords"
              sx={{ m: 1, width: '20ch' }}
              value={criteria.Keywords}
              onChange={handleChange('Keywords')}
              InputProps={{
              }}
            />
          </div>
          <div>
            <input type="submit" value="Search" onClick={() => search()} />
            <input type="button" value="Clear" onClick={() => reset()} />
          </div>
        </div>
      </div>
    </div>
  );
}

export default SearchBar;
