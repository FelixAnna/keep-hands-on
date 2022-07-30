import * as React from 'react';
import OutlinedInput from '@mui/material/OutlinedInput';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import ListItemText from '@mui/material/ListItemText';
import Select from '@mui/material/Select';
import Checkbox from '@mui/material/Checkbox';
import { DistrictCategory } from '../const';

const ITEM_HEIGHT = 48;
const ITEM_PADDING_TOP = 8;
const MenuProps = {
  PaperProps: {
    style: {
      maxHeight: ITEM_HEIGHT * 4.5 + ITEM_PADDING_TOP,
      width: 250,
    },
  },
};

export default function MultipleSelectCheckmarks() {
  const [districts, setDistricts] = React.useState([]);

  const handleChange = (event) => {
    const {
      target: { value },
    } = event;
    setDistricts(
      // On autofill we get a stringified value.
      typeof value === 'string' ? value.split(',') : value,
    );
  };

  return (
    <div>
      <FormControl sx={{ m: 1, width: 300 }}>
        <InputLabel id="demo-multiple-checkbox-label">District</InputLabel>
        <Select
          labelId="demo-multiple-checkbox-label"
          id="demo-multiple-checkbox"
          multiple
          value={districts}
          onChange={handleChange}
          input={<OutlinedInput label="District" />}
          renderValue={(selected) => selected.join(', ')}
          MenuProps={MenuProps}
        >
          {DistrictCategory.map((item) => (
            <MenuItem key={item.key} value={item.text}>
              <Checkbox checked={districts.indexOf(item.text) > -1} />
              <ListItemText primary={item.text} />
            </MenuItem>
          ))}
        </Select>
      </FormControl>
    </div>
  );
}
