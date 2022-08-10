import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import AdbIcon from '@mui/icons-material/Adb';
import Button from '@mui/material/Button';
import { Link } from 'react-router-dom';
import ResponsiveUserSettings from './settings';
import ResponsiveTopMenu from './topmenu';
import ResponsiveLeftMenu from './leftmenu';
import { currentLoginStatus, reloadLogin } from '../login/reducer';

function ResponsiveAppBar() {
  const dispatch = useDispatch();
  useEffect(() => {
    dispatch(reloadLogin());
  }, []);

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
            Daily Life Web
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
          { loginStatus ? <ResponsiveUserSettings /> : (
            <Button
              key="/login"
              component={Link}
              to="/login"
              sx={{ my: 2, color: 'white', display: 'block' }}
            >
              Login
            </Button>
          )}
        </Toolbar>
      </Container>
    </AppBar>
  );
}
export default ResponsiveAppBar;
