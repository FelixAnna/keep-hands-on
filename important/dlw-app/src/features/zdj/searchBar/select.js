import * as React from 'react';
import Checkbox from '@mui/material/Checkbox';
import TextField from '@mui/material/TextField';
import Autocomplete from '@mui/material/Autocomplete';
import CheckBoxOutlineBlankIcon from '@mui/icons-material/CheckBoxOutlineBlank';
import CheckBoxIcon from '@mui/icons-material/CheckBox';
import { DistrictCategory } from '../const';

const icon = <CheckBoxOutlineBlankIcon fontSize="small" />;
const checkedIcon = <CheckBoxIcon fontSize="small" />;
export default function MultipleSelectCheckmarks() {
  return (
    <Autocomplete
      multiple
      id="checkboxes-tags-demo"
      options={DistrictCategory}
      disableCloseOnSelect
      getOptionLabel={(option) => option.text}
      renderOption={(props, option, { selected }) => (
        <li {...props}>
          <Checkbox
            icon={icon}
            checkedIcon={checkedIcon}
            style={{ marginRight: 8 }}
            checked={selected}
          />
          {option.text}
        </li>
      )}
      style={{ width: 380 }}
      renderInput={(params) => (
        <TextField {...params} label="行政区" placeholder="请选择" />
      )}
    />
  );
}
