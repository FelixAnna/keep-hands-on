import * as React from 'react';
import PropTypes from 'prop-types';
import { useDispatch } from 'react-redux';
import Button from '@mui/material/Button';
import { styled } from '@mui/material/styles';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import IconButton from '@mui/material/IconButton';
import CloseIcon from '@mui/icons-material/Close';
import Typography from '@mui/material/Typography';
import TextField from '@mui/material/TextField';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';
import { addAsync } from '../reducer';

const BootstrapDialog = styled(Dialog)(({ theme }) => ({
  '& .MuiDialogContent-root': {
    padding: theme.spacing(2),
  },
  '& .MuiDialogActions-root': {
    padding: theme.spacing(1),
  },
}));

function BootstrapDialogTitle(props) {
  const { children, onClose, ...other } = props;

  return (
    <DialogTitle sx={{ m: 0, p: 2 }} {...other}>
      {children}
      {onClose ? (
        <IconButton
          aria-label="close"
          onClick={onClose}
          sx={{
            position: 'absolute',
            right: 8,
            top: 8,
            color: (theme) => theme.palette.grey[500],
          }}
        >
          <CloseIcon />
        </IconButton>
      ) : null}
    </DialogTitle>
  );
}

BootstrapDialogTitle.propTypes = {
  children: PropTypes.node.isRequired,
  onClose: PropTypes.func.isRequired,
};

const defaultMemoItem = {
  Subject: '',
  Description: '',
  MonthDay: '',
  StartYear: 2022,
  Lunar: 0,
};

export default function CreateNewItemDialogs() {
  const [memoItem, setMemoItem] = React.useState(defaultMemoItem);
  const [open, setOpen] = React.useState(false);
  const handleClickOpen = () => {
    setOpen(true);
  };
  const dispatch = useDispatch();
  const handleClose = () => {
    dispatch(addAsync({
      Subject: memoItem.Subject,
      Description: memoItem.Description,
      MonthDay: Number(memoItem.MonthDay),
      StartYear: memoItem.StartYear,
      Lunar: memoItem.Lunar,
    }))
      .then(() => {
        setMemoItem({ ...defaultMemoItem });
      });
    setOpen(false);
  };
  const handleChange = (prop) => (event) => {
    setMemoItem({ ...memoItem, [prop]: event.target.value });
  };

  return (
    <>
      <Button variant="contained" color="success" onClick={() => handleClickOpen()}>
        New Item
      </Button>
      <BootstrapDialog
        onClose={handleClose}
        aria-labelledby="customized-dialog-title"
        open={open}
      >
        <BootstrapDialogTitle id="customized-dialog-title" onClose={handleClose}>
          Create New Item
        </BootstrapDialogTitle>
        <DialogContent
          sx={{
            width: 500,
            maxWidth: '100%',
          }}
          dividers
        >
          <Typography gutterBottom>
            <TextField
              fullWidth
              label="Subject"
              id="subject"
              value={memoItem.Subject}
              onChange={handleChange('Subject')}
              required
            />
          </Typography>
          <Typography gutterBottom>
            <TextField
              id="description"
              label="Description"
              multiline
              rows={4}
              value={memoItem.Description}
              onChange={handleChange('Description')}
              sx={{
                width: 500,
                maxWidth: '100%',
              }}
              required
            />
          </Typography>
          <Typography gutterBottom>
            <TextField
              id="monthDay"
              label="MonthDay"
              placeholder="MMdd, like: 1231"
              value={memoItem.MonthDay}
              onChange={handleChange('MonthDay')}
              required
              InputLabelProps={{
                shrink: true,
              }}
            />
          </Typography>
          <Typography gutterBottom>
            <TextField
              id="startYear"
              label="StartYear"
              value={memoItem.StartYear}
              onChange={handleChange('StartYear')}
              type="number"
              InputLabelProps={{
                shrink: true,
              }}
            />
          </Typography>
          <Typography gutterBottom>
            <FormControl fullWidth>
              <InputLabel id="demo-simple-select-label">Lunar</InputLabel>
              <Select
                labelId="lunar-select"
                id="lunar-select"
                value={memoItem.Lunar}
                onChange={handleChange('Lunar')}
                label="Lunar"
              >
                <MenuItem value={0}>Georgian</MenuItem>
                <MenuItem value={1}>Lunar</MenuItem>
              </Select>
            </FormControl>
          </Typography>
        </DialogContent>
        <DialogActions>
          <Button autoFocus onClick={handleClose}>
            Save
          </Button>
        </DialogActions>
      </BootstrapDialog>
    </>
  );
}
