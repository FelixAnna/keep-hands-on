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
import {
  currentCriteria,
  currentPage,
  currentSize,
  currentItems,
  currentDisplayItems,
  refreshData,
  loadAsync,
} from '../reducer';

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
  const dispatch = useDispatch();
  useEffect(() => {
    dispatch(loadAsync(criteria))
      .then(() => {
        console.log('refreshed');
      });
  }, [criteria]);

  const items = useSelector(currentItems);
  const page = useSelector(currentPage);
  const size = useSelector(currentSize);
  useEffect(() => {
    dispatch(refreshData());
  }, [items, page, size]);

  const displayItems = useSelector(currentDisplayItems);
  const start = new Date('1970-01-01');
  return (
    <div style={{ padding: 15 }}>
      <TableContainer component={Paper} sx={{ minWidth: 700, maxWidth: 1280, align: 'center' }}>
        <Table aria-label="customized table">
          <TableHead>
            <TableRow>
              <StyledTableCell>Id</StyledTableCell>
              <StyledTableCell align="right">Subject</StyledTableCell>
              <StyledTableCell align="right">Description</StyledTableCell>
              <StyledTableCell align="right">MonthDay</StyledTableCell>
              <StyledTableCell align="right">StartYear</StyledTableCell>
              <StyledTableCell align="right">Lunar</StyledTableCell>
              <StyledTableCell align="right">Distance</StyledTableCell>
              <StyledTableCell align="right">CreateTime</StyledTableCell>
              <StyledTableCell align="right">LastModifiedTime</StyledTableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {displayItems.map((row) => (
              <StyledTableRow key={row.Id}>
                <StyledTableCell component="th" scope="row">
                  {row.Id}
                </StyledTableCell>
                <StyledTableCell align="right">{row.Subject}</StyledTableCell>
                <StyledTableCell align="right">{row.Description}</StyledTableCell>
                <StyledTableCell align="right">{row.MonthDay}</StyledTableCell>
                <StyledTableCell align="right">{row.StartYear > 0 ? row.StartYear : '/' }</StyledTableCell>
                <StyledTableCell align="right">{row.Lunar}</StyledTableCell>
                <StyledTableCell align="right">
                  {row.Distance[0]}
                  ,
                  {row.Distance[1]}
                </StyledTableCell>
                <StyledTableCell align="right">{(new Date(start.getTime() + row.CreateTime * 1000)).toISOString()}</StyledTableCell>
                <StyledTableCell align="right">{row.LastModifiedTime}</StyledTableCell>
              </StyledTableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </div>
  );
}
