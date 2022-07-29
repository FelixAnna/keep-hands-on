import * as React from 'react';
import { useSelector } from 'react-redux';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import AdbIcon from '@mui/icons-material/Adb';
import Link from '@mui/material/Link';
import ResponsiveUserSettings from './settings';
import ResponsiveTopMenu from './topmenu';
import ResponsiveLeftMenu from './leftmenu';
import { currentLoginStatus } from '../social/reducer';

function ResponsiveAppBar() {
  const loginStatus = useSelector(currentLoginStatus);
  return (
    <AppBar position="static">
      <Container maxWidth="xl">
        <Toolbar disableGutters>
          <AdbIcon sx={{ display: { xs: 'none', md: 'flex' }, mr: 1 }} />
          <Typography
            variant="h6"
            noWrap
            component="a"
            href="/"
            sx={{
              mr: 2,
              display: { xs: 'none', md: 'flex' },
              fontFamily: 'monospace',
              fontWeight: 700,
              letterSpacing: '.3rem',
              color: 'inherit',
              textDecoration: 'none',
            }}
          >
            Daily Life App
          </Typography>

          <ResponsiveLeftMenu />
          <AdbIcon sx={{ display: { xs: 'flex', md: 'none' }, mr: 1 }} />
          <Typography
            variant="h5"
            noWrap
            component="a"
            href="/"
            sx={{
              mr: 2,
              display: { xs: 'flex', md: 'none' },
              flexGrow: 1,
              fontFamily: 'monospace',
              fontWeight: 700,
              letterSpacing: '.3rem',
              color: 'inherit',
              textDecoration: 'none',
            }}
          >
            DLW
          </Typography>
          <ResponsiveTopMenu />
          { loginStatus ? <ResponsiveUserSettings /> : (<Link href="/login" color="inherit">Login</Link>) }
        </Toolbar>
      </Container>
    </AppBar>
  );
}
export default ResponsiveAppBar;
