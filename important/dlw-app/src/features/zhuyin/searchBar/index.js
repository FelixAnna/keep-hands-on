import './index.css';
import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import TextField from '@mui/material/TextField';
import Stack from '@mui/material/Stack';
import Button from '@mui/material/Button';
import { saveCriteria, clearAll } from '../reducer';

const defaultCriteria = {
  Keywords: '',
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
      Keywords: criteria.Keywords,
    };
    dispatch(saveCriteria(payload));
  };

  // Distrct/Street/Community/MinPrice/MaxPrice/Version/SortKey/Page/Size
  return (
    <div style={{ display: 'flex' }}>
      <div className="zhuyin-style">
        <div className="zhuyin-style-heading">中文注音</div>
        <Stack direction="row" spacing={2}>
          <TextField
            label="简体中文"
            id="outlined-adornment-keywords"
            sx={{ m: 1, width: '20ch' }}
            value={criteria.Keywords}
            onChange={handleChange('Keywords')}
            InputProps={{
            }}
          />
          <Button variant="contained" onClick={() => search()}>Sumbit</Button>
          <Button variant="outlined" onClick={() => reset()}>Reset</Button>
        </Stack>
      </div>
    </div>
  );
}

export default SearchBar;
