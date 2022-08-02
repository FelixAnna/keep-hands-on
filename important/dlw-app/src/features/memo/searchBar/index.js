import './index.css';
import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import TextField from '@mui/material/TextField';
import { saveCriteria, clearAll } from '../reducer';

const defaultCriteria = {
  StartDate: '2022-01-01',
  EndDate: '2022-12-31',
};
function SearchBar() {
  const dispatch = useDispatch();
  const [criteria, updateCriteria] = useState(defaultCriteria);

  const handleChange = (prop) => (event) => {
    updateCriteria({ ...criteria, [prop]: event.target.value });
  };

  const reset = () => {
    updateCriteria({ ...defaultCriteria });
    dispatch(clearAll());
  };
  const search = () => {
    const payload = {
      StartDate: criteria.StartDate,
      EndDate: criteria.EndDate,
    };
    dispatch(saveCriteria(payload));
  };

  return (
    <div style={{ display: 'flex' }}>
      <div className="memo-style">
        <div className="memo-style-heading">Search</div>
        <div>
          <div>
            <TextField
              id="date"
              label="Birthday"
              type="date"
              defaultValue="2022-01-01"
              sx={{ width: 220 }}
              value={criteria.StartDate}
              onChange={handleChange('StartDate')}
              InputLabelProps={{
                shrink: true,
              }}
            />
          </div>
          <div>
            <TextField
              id="date"
              label="Birthday"
              type="date"
              defaultValue="2022-12-31"
              sx={{ width: 220 }}
              value={criteria.EndDate}
              onChange={handleChange('EndDate')}
              InputLabelProps={{
                shrink: true,
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
