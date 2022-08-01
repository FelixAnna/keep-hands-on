import React from 'react';
import { useDispatch } from 'react-redux';
import SearchBar from './searchBar';
import CustomizedTables from './results';
import { loadMore } from './reducer';

function ZdjSearch() {
  const dispatch = useDispatch();
  return (
    <>
      <SearchBar />
      <CustomizedTables />
      <input type="button" value="Load more" onClick={() => dispatch(loadMore())} />
    </>
  );
}

export default ZdjSearch;
