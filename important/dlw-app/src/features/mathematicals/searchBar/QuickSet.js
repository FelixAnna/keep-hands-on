import './SearchBar.css';
import React from 'react';
import { useDispatch } from 'react-redux';
import Chip from '@mui/material/Chip';
import Stack from '@mui/material/Stack';
import Slider from '@mui/material/Slider';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import {
  addCriteriaBatch,
} from '../reducers/searchBar';
import {
  MathCategory, MathKind, MathType,
  IORanges,
} from '../const';

const defaultCriteria = {
  Categories: [],
  Kind: [],
  Types: [],

  InputRange: [30, 60],
  OutputRange: [30, 60],

  Quantity: 5,
};

export default function QuickSet() {
  const dispatch = useDispatch();
  const [criteria, setCriteria] = React.useState(defaultCriteria);
  const handleAppendElement = (arr, element) => {
    if (arr.indexOf(element) >= 0) {
      return arr.filter((x) => x !== element);
    }

    return [...arr, element];
  };
  const handleClick = (type, value) => {
    switch (type) {
      case 'category': {
        const categories = handleAppendElement(criteria.Categories, Number(value));
        setCriteria({ ...criteria, Categories: categories });
        break;
      }
      case 'kind': {
        const kind = handleAppendElement(criteria.Kind, Number(value));
        setCriteria({ ...criteria, Kind: kind });
        break;
      }
      case 'type': {
        const types = handleAppendElement(criteria.Types, Number(value));
        setCriteria({ ...criteria, Types: types });
        break;
      }
      default:
        break;
    }
  };

  const handleChange = (prop) => (event) => {
    setCriteria({ ...criteria, [prop]: event.target.value });
  };

  const categoryOptions = MathCategory.map((op) => {
    const selected = criteria.Categories.indexOf(op.value) >= 0;
    let variant = 'outlined';
    const color = 'primary';
    if (selected) {
      variant = '';
    }
    return (<Chip label={op.text} key={op.key} onClick={() => handleClick('category', op.value)} variant={variant} color={color} />);
  });

  const kindOptions = MathKind.map((op) => {
    const selected = criteria.Kind.indexOf(op.value) >= 0;
    let variant = 'outlined';
    const color = 'success';
    if (selected) {
      variant = '';
    }
    return (<Chip label={op.symble} key={op.key} onClick={() => handleClick('kind', op.value)} variant={variant} color={color} />);
  });

  const calculateValue = (value) => IORanges.find((x) => x.value === value).val;

  return (
    <div>
      <Stack direction="row" spacing={1}>
        {categoryOptions}
      </Stack>
      <Stack direction="row" spacing={1}>
        {kindOptions}
      </Stack>
      <Stack direction="row" spacing={1}>
        {MathType.map((op) => (<Chip label={op.text} key={op.key} onClick={() => handleClick('type', op.options[0].value)} variant={criteria.Types.indexOf(op.options[0].value) >= 0 ? '' : 'outlined'} color="primary" />))}
      </Stack>
      <Typography gutterBottom>输入范围</Typography>
      <Slider
        step={10}
        marks={IORanges}
        scale={calculateValue}
        value={criteria.InputRange}
        onChange={handleChange('InputRange')}
        min={0}
        max={80}
        valueLabelDisplay="on"
      />
      <Typography gutterBottom>结果范围</Typography>
      <Slider
        step={10}
        marks={IORanges}
        scale={calculateValue}
        value={criteria.OutputRange}
        onChange={handleChange('OutputRange')}
        min={0}
        max={80}
        valueLabelDisplay="on"
      />
      <Typography gutterBottom>每种数量</Typography>
      <Slider
        value={criteria.Quantity}
        onChange={handleChange('Quantity')}
        min={0}
        max={100}
        aria-label="Default"
        valueLabelDisplay="auto"
      />
      <Button variant="outlined" onClick={() => dispatch(addCriteriaBatch(criteria))}>提交</Button>
    </div>
  );
}
