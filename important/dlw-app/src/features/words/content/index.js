import React from 'react';
import { styled } from '@mui/material/styles';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell, { tableCellClasses } from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import { Link } from '@mui/material';

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

export default function WordsContent() {
  return (
    <>
      <h3 style={{ margin: 15, display: 'flex' }}>学习助手字库列表</h3>
      <div style={{ padding: 15 }}>
        <TableContainer component={Paper} sx={{ minWidth: 700, maxWidth: 680, align: 'center' }}>
          <Table aria-label="customized table">
            <TableHead>
              <TableRow>
                <StyledTableCell align="center">教材版本</StyledTableCell>
                <StyledTableCell align="center">链接</StyledTableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              <StyledTableRow key="1">
                <StyledTableCell align="center">江苏版3年级上册语文和英语</StyledTableCell>
                <StyledTableCell align="center"><Link href="books/renjiaoban/2018/grade3/first/englishAndChinese.csv">字库</Link></StyledTableCell>
              </StyledTableRow>
              <StyledTableRow key="2">
                <StyledTableCell align="center">江苏版3年级下册语文和英语</StyledTableCell>
                <StyledTableCell align="center"><Link href="books/renjiaoban/2018/grade3/second/englishAndChinese.csv">字库</Link></StyledTableCell>
              </StyledTableRow>
            </TableBody>
          </Table>
        </TableContainer>
      </div>
      <div style={{ margin: 15, display: 'flex' }}>
        <a href="https://apps.microsoft.com/detail/9NR9KR5N3BX2?hl=zh-cn&gl=CN">Windows本版下载</a>
      </div>
    </>
  );
}
