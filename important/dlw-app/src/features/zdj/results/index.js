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

export default function CustomizedTables() {
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
      <TableContainer component={Paper} sx={{ minWidth: 700, maxWidth: 1280, align: 'center' }}>
        <Table aria-label="customized table">
          <TableHead>
            <TableRow>
              <StyledTableCell>Id</StyledTableCell>
              <StyledTableCell align="right">Distrct</StyledTableCell>
              <StyledTableCell align="right">Street</StyledTableCell>
              <StyledTableCell align="right">Community</StyledTableCell>
              <StyledTableCell align="right">Price</StyledTableCell>
              <StyledTableCell align="right">Version</StyledTableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {items.map((row) => (
              <StyledTableRow key={row.name}>
                <StyledTableCell component="th" scope="row">
                  {row.Id}
                </StyledTableCell>
                <StyledTableCell align="right">{row.Distrct}</StyledTableCell>
                <StyledTableCell align="right">{row.Street}</StyledTableCell>
                <StyledTableCell align="right">{row.Community}</StyledTableCell>
                <StyledTableCell align="right">{row.Price}</StyledTableCell>
                <StyledTableCell align="right">{row.Version}</StyledTableCell>
              </StyledTableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </div>
  );
}
