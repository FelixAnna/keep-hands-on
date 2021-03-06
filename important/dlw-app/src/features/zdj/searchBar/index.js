import './index.css';
import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import InputAdornment from '@mui/material/InputAdornment';
import TextField from '@mui/material/TextField';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';
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
    const payload = {
      Districts: district,
      Keywords: criteria.Keywords,
      MinPrice: Number(criteria.MinPrice),
      MaxPrice: Number(criteria.MaxPrice),
      SortKey: criteria.SortKey,
      Page: 1,
      Size: 10,
    };
    dispatch(saveCriteria(payload));
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
              label="????????????"
              id="outlined-adornment-max"
              sx={{ m: 1, width: '18ch' }}
              value={criteria.MinPrice}
              onChange={handleChange('MinPrice')}
              InputProps={{
                endAdornment: <InputAdornment position="end">??/???</InputAdornment>,
                inputMode: 'numeric',
                pattern: '[0-9]*',
              }}
            />
          </div>
          <div>
            <TextField
              label="????????????"
              id="outlined-adornment-max"
              sx={{ m: 1, width: '18ch' }}
              value={criteria.MaxPrice}
              onChange={handleChange('MaxPrice')}
              InputProps={{
                endAdornment: <InputAdornment position="end">??/???</InputAdornment>,
                inputMode: 'numeric',
                pattern: '[0-9]*',
              }}
            />
          </div>
          <div>
            <TextField
              label="?????????"
              id="outlined-adornment-keywords"
              sx={{ m: 1, width: '20ch' }}
              value={criteria.Keywords}
              onChange={handleChange('Keywords')}
              InputProps={{
              }}
            />
          </div>
          <div>
            <FormControl sx={{ m: 1, minWidth: 150 }}>
              <InputLabel id="demo-simple-select-autowidth-label">??????</InputLabel>
              <Select
                labelId="demo-simple-select-autowidth-label"
                id="demo-simple-select-autowidth"
                value={criteria.SortKey}
                onChange={handleChange('SortKey')}
                autoWidth
                label="??????"
              >
                <MenuItem value="id">
                  <em>None</em>
                </MenuItem>
                <MenuItem value="price_asc">????????????</MenuItem>
                <MenuItem value="price_desc">????????????</MenuItem>
                <MenuItem value="district_asc">???????????????</MenuItem>
                <MenuItem value="district_desc">???????????????</MenuItem>
                <MenuItem value="street_asc">????????????</MenuItem>
                <MenuItem value="street_desc">????????????</MenuItem>
                <MenuItem value="community_asc">????????????</MenuItem>
                <MenuItem value="community_desc">????????????</MenuItem>
              </Select>
            </FormControl>
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
