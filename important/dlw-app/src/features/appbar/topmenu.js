import * as React from 'react';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import { Link } from 'react-router-dom';
import { pages } from './const';

function ResponsiveTopMenu() {
  return (
    <Box sx={{ flexGrow: 1, display: { xs: 'none', md: 'flex' } }}>
      {pages.map((page) => (
        <Button
          key={page.Path}
          component={Link}
          to={page.Path}
          sx={{ my: 2, color: 'white', display: 'block' }}
        >
          {page.Text}
        </Button>
      ))}
    </Box>
  );
}
export default ResponsiveTopMenu;
