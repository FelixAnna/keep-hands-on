import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import LoadingButton from '@mui/lab/LoadingButton';
import SearchBar from './searchBar';
import CustomizedTables from './items';
import { loadMore, loadingStatus } from './reducer';

function MemoSearch() {
  const dispatch = useDispatch();
  const status = useSelector(loadingStatus);
  return (
    <>
      <SearchBar />
      <CustomizedTables />
      <LoadingButton loading={status !== 'idle'} loadingIndicator="Loading…" variant="outlined" onClick={() => dispatch(loadMore())}>
        Load More
      </LoadingButton>
    </>
  );
}

export default MemoSearch;
