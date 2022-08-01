import * as React from 'react';
import OutlinedInput from '@mui/material/OutlinedInput';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import ListItemText from '@mui/material/ListItemText';
import Select from '@mui/material/Select';
import Checkbox from '@mui/material/Checkbox';
import PropTypes from 'prop-types';
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
function MultipleSelectCheckmarks({ district, handleChange }) {
  return (
    <FormControl sx={{ m: 1, width: 300 }}>
      <InputLabel id="district-multiple-checkbox-label">行政区</InputLabel>
      <Select
        labelId="district-multiple-checkbox-label"
        id="district-multiple-checkbox"
        multiple
        value={district}
        onChange={handleChange}
        input={<OutlinedInput label="行政区" />}
        renderValue={(selected) => selected.join(', ')}
        MenuProps={MenuProps}
      >
        {DistrictCategory.map((dist) => (
          <MenuItem key={dist.key} value={dist.text}>
            <Checkbox checked={district.indexOf(dist.text) > -1} />
            <ListItemText primary={dist.text} />
          </MenuItem>
        ))}
      </Select>
    </FormControl>
  );
}

MultipleSelectCheckmarks.propTypes = {
  handleChange: PropTypes.func.isRequired,
  district: PropTypes.arrayOf(PropTypes.string).isRequired,
};

export default MultipleSelectCheckmarks;
