import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { styled } from '@mui/material/styles';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell, { tableCellClasses } from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import { currentCriteria, currentItems, loadAsync } from '../reducer';

const StyledTableCell = styled(TableCell)(({ theme }) => ({
  [`&.${tableCellClasses.head}`]: {
    backgroundColor: '#009879',
    color: theme.palette.common.white,
  },
  [`&.${tableCellClasses.body}`]: {
    fontSize: 14,
  },
}));

const StyledTableRow = styled(TableRow)(({ theme }) => ({
  '&:nth-of-type(odd)': {
    backgroundColor: theme.palette.action.hover,
  },
  // hide last border
  '&:last-child td, &:last-child th': {
    border: 0,
  },
}));

export default function ZhuyinContent() {
  const criteria = useSelector(currentCriteria);
  const items = useSelector(currentItems);
  const dispatch = useDispatch();
  useEffect(() => {
    dispatch(loadAsync(criteria))
      .then(() => {
        console.log('refreshed');
      });
  }, [criteria]);

  return (
    <div style={{ padding: 15 }}>
      <TableContainer component={Paper} sx={{ minWidth: 700, maxWidth: 680, align: 'center' }}>
        <Table aria-label="customized table">
          <TableHead>
            <TableRow>
              <StyledTableCell align="right">汉字</StyledTableCell>
              <StyledTableCell align="right">拼音</StyledTableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            <StyledTableRow key="1">
                <StyledTableCell align="right">{criteria.Keywords}</StyledTableCell>
                <StyledTableCell align="right">{items}</StyledTableCell>
              </StyledTableRow>
          </TableBody>
        </Table>
      </TableContainer>
    </div>
  );
}
