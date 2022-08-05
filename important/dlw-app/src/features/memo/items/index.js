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
import IconButton from '@mui/material/IconButton';
import DeleteIcon from '@mui/icons-material/Delete';
import {
  currentCriteria,
  currentPage,
  currentSize,
  currentItems,
  currentDisplayItems,
  refreshData,
  loadAsync,
  deleteAsync,
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
  const formatLunar = (lunar) => (lunar === true ? 'Lunar' : 'Gregorian');
  const pad2 = (n) => String(n).padStart(2, '0');
  const formatMMdd = (md) => `${pad2(Math.floor(md / 100))}-${pad2(md % 100)}`;
  const start = new Date('1970-01-01');
  const formatDateTime = (seconds) => {
    if (seconds === undefined) {
      return '';
    }
    const date = new Date(start.getTime() + seconds * 1000);
    const datepart = `${date.getFullYear().toString()}-${pad2(date.getMonth() + 1)}-${pad2(date.getDate())}`;
    const timepart = `${pad2(date.getHours())}:${pad2(date.getMinutes())}:${pad2(date.getSeconds())}`;

    return `${datepart} ${timepart}`;
  };
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
              <StyledTableCell align="right">Last Date</StyledTableCell>
              <StyledTableCell align="right">Next Date</StyledTableCell>
              <StyledTableCell align="right">CreateTime</StyledTableCell>
              <StyledTableCell align="right">LastModifiedTime</StyledTableCell>
              <StyledTableCell align="right">Operation</StyledTableCell>
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
                <StyledTableCell align="right">{formatMMdd(row.MonthDay)}</StyledTableCell>
                <StyledTableCell align="right">{row.StartYear > 0 ? row.StartYear : '/' }</StyledTableCell>
                <StyledTableCell align="right">{formatLunar(row.Lunar)}</StyledTableCell>
                <StyledTableCell align="right">
                  <b>{row.Distance[0] * -1}</b>
                  &nbsp;days ago
                </StyledTableCell>
                <StyledTableCell align="right">
                  <b>{row.Distance[1]}</b>
                  &nbsp;days later
                </StyledTableCell>
                <StyledTableCell align="right">{formatDateTime(row.CreateTime)}</StyledTableCell>
                <StyledTableCell align="right">{formatDateTime(row.LastModifiedTime)}</StyledTableCell>
                <StyledTableCell align="right">
                  <IconButton
                    aria-label="delete"
                    onClick={() => {
                      console.log(row.Id);
                      dispatch(deleteAsync({ id: row.Id }));
                    }}
                  >
                    <DeleteIcon />
                  </IconButton>
                </StyledTableCell>
              </StyledTableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </div>
  );
}
