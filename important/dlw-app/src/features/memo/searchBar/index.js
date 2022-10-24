import './index.css';
import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import TextField from '@mui/material/TextField';
import Stack from '@mui/material/Stack';
import Button from '@mui/material/Button';
import { saveCriteria, clearAll } from '../reducer';
import CreateNewItemDialogs from './add';

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
        <div className="memo-style-heading">Memo Management</div>
        <Stack direction="row" spacing={2}>
          <TextField
            id="date"
            label="Start Date"
            type="date"
            sx={{ width: 220 }}
            value={criteria.StartDate}
            onChange={handleChange('StartDate')}
            InputLabelProps={{
              shrink: true,
            }}
          />
          <TextField
            id="date"
            label="End Date"
            type="date"
            sx={{ width: 220 }}
            value={criteria.EndDate}
            onChange={handleChange('EndDate')}
            InputLabelProps={{
              shrink: true,
            }}
          />
          <Button variant="contained" onClick={() => search()}>Search</Button>
          <Button variant="outlined" onClick={() => reset()}>Reset</Button>
          <CreateNewItemDialogs />
        </Stack>
      </div>
    </div>
  );
}

export default SearchBar;
