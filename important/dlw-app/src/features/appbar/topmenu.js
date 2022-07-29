import * as React from 'react';
import Box from '@mui/material/Box';
import { Link } from 'react-router-dom';
import { pages } from './const';

function ResponsiveTopMenu() {
  return (
    <Box sx={{ flexGrow: 1, display: { xs: 'none', md: 'flex' } }}>
      {pages.map((page) => (
        <Link
          color="white"
          to={page.Path}
        >
          {page.Text}
        </Link>
      ))}
    </Box>
  );
}
export default ResponsiveTopMenu;
