import React from 'react';
import { useSelector } from 'react-redux';
import Box from '@mui/material/Box';
import { DataGrid } from '@mui/x-data-grid';
import { MathCategory, MathKind, MathType } from '../const';
import { currentCriterias } from '../reducers/searchBar';

const convertToText = (value, type) => {
  let result = '';
  switch (type) {
    case 'Category':
      result = MathCategory.find((op) => op.value === value).text;
      break;
    case 'Kind':
      result = MathKind.find((op) => op.value === value).text;
      break;
    case 'Type':
      MathType.forEach((group) => {
        const option = group.options.find((op) => op.value === value);
        if (option !== undefined) {
          result = option.text;
        }
      });
      break;

    default:
      break;
  }

  return result;
};
const columns = [
  { field: 'id', headerName: 'ID', width: 70 },
  {
    field: 'inputRange',
    headerName: '数字范围',
    width: 130,
    valueGetter: (params) => `${params.row.Min || '0'} ~ ${params.row.Max || '0'}`,
  },
  {
    field: 'outputRange',
    headerName: '结果范围',
    width: 130,
    valueGetter: (params) => `${params.row.Range.Min || '0'} ~ ${params.row.Range.Max || '0'}`,
  },
  { field: 'Quantity', headerName: '题目数量', width: 70 },
  {
    field: 'Category',
    headerName: '算术类型',
    width: 70,
    valueGetter: (params) => `${convertToText(params.row.Category, 'Category')}`,
  },
  {
    field: 'Kind',
    headerName: '求值类型',
    width: 180,
    valueGetter: (params) => `${convertToText(params.row.Kind, 'Kind')}`,
  },
  {
    field: 'Type',
    headerName: '输出格式',
    width: 380,
    valueGetter: (params) => `${convertToText(params.row.Type, 'Type')}`,
  },
];

function CriteriaList() {
  const criterias = useSelector(currentCriterias);
  const rows = criterias.map((q, i) => ({ id: i + 1, ...q }));
  const height = 52 * (rows.length < 10 ? rows.length : 10) + 56 * 2;
  return (
    <Box sx={{ height: { height }, width: '100%' }}>
      <DataGrid
        rows={rows}
        columns={columns}
        pageSize={10}
        rowsPerPageOptions={[10]}
        checkboxSelection
      />
    </Box>
  );
}

export default CriteriaList;
