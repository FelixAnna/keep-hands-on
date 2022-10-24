import './index.css';
import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import InputAdornment from '@mui/material/InputAdornment';
import TextField from '@mui/material/TextField';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';
import Stack from '@mui/material/Stack';
import Button from '@mui/material/Button';
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
        <div className="zdj-style-heading">ZDJ</div>
        <Stack direction="row" spacing={2}>
          <MultipleSelectCheckmarks
            handleChange={handleDistrictChange}
            district={district}
          />
          <TextField
            label="最低单价"
            id="outlined-adornment-max"
            sx={{ m: 1, width: '22ch' }}
            value={criteria.MinPrice}
            onChange={handleChange('MinPrice')}
            InputProps={{
              endAdornment: <InputAdornment position="end">¥/㎡</InputAdornment>,
              inputMode: 'numeric',
              pattern: '[0-9]*',
            }}
          />
          <TextField
            label="最高单价"
            id="outlined-adornment-max"
            sx={{ m: 1, width: '22ch' }}
            value={criteria.MaxPrice}
            onChange={handleChange('MaxPrice')}
            InputProps={{
              endAdornment: <InputAdornment position="end">¥/㎡</InputAdornment>,
              inputMode: 'numeric',
              pattern: '[0-9]*',
            }}
          />
          <TextField
            label="关键字"
            id="outlined-adornment-keywords"
            sx={{ m: 1, width: '20ch' }}
            value={criteria.Keywords}
            onChange={handleChange('Keywords')}
            InputProps={{
            }}
          />
          <FormControl sx={{ m: 1, minWidth: 150 }}>
            <InputLabel id="demo-simple-select-autowidth-label">排序</InputLabel>
            <Select
              labelId="demo-simple-select-autowidth-label"
              id="demo-simple-select-autowidth"
              value={criteria.SortKey}
              onChange={handleChange('SortKey')}
              autoWidth
              label="排序"
            >
              <MenuItem value="id">
                <em>None</em>
              </MenuItem>
              <MenuItem value="price_asc">单价升序</MenuItem>
              <MenuItem value="price_desc">单价降序</MenuItem>
              <MenuItem value="district_asc">行政区升序</MenuItem>
              <MenuItem value="district_desc">行政区降序</MenuItem>
              <MenuItem value="street_asc">街道升序</MenuItem>
              <MenuItem value="street_desc">街道降序</MenuItem>
              <MenuItem value="community_asc">小区升序</MenuItem>
              <MenuItem value="community_desc">小区降序</MenuItem>
            </Select>
          </FormControl>
          <Button variant="contained" onClick={() => search()}>Search</Button>
          <Button variant="outlined" onClick={() => reset()}>Reset</Button>
        </Stack>
      </div>
    </div>
  );
}

export default SearchBar;
