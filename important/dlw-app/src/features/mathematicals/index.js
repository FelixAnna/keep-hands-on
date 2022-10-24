import React, { useState } from 'react';
import Box from '@mui/material/Box';
import Tab from '@mui/material/Tab';
import TabContext from '@mui/lab/TabContext';
import TabList from '@mui/lab/TabList';
import TabPanel from '@mui/lab/TabPanel';
import Button from '@mui/material/Button';
import Stack from '@mui/material/Stack';
import { useSelector, useDispatch } from 'react-redux';
import CriteriaList from './criteriaList/CriteriaList';
import QuestionList from './questionsList/QuestionList';
import SearchBar from './searchBar/SearchBar';
import {
  loadAsync, currentCriterias, currentQuestions,
} from './reducers/searchBar';

function Mathematicals() {
  const [value, setValue] = useState('1');
  const dispatch = useDispatch();
  const criterias = useSelector(currentCriterias);
  const questions = useSelector(currentQuestions);

  const handleChange = (event, newValue) => {
    setValue(newValue);
  };

  const handleGenerate = () => {
    if (criterias === undefined || criterias.length === 0) {
      return;
    }
    dispatch(loadAsync(criterias));
    setValue('2');
  };

  return (
    <Box sx={{ width: '100%', typography: 'body1' }}>
      <TabContext value={value}>
        <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
          <TabList onChange={handleChange} aria-label="lab API tabs example">
            <Tab label="题目配置" value="1" />
            <Tab label="题目列表" value="2" disabled={questions === undefined || questions.length === 0} />
          </TabList>
        </Box>
        <TabPanel value="1">
          <Stack direction="column" spacing={1} alignItems="center">
            <SearchBar />
            <CriteriaList />
            <Button variant="outlined" onClick={handleGenerate}>刷新题目列表</Button>
          </Stack>
        </TabPanel>
        <TabPanel value="2">
          <QuestionList />
        </TabPanel>
      </TabContext>
    </Box>
  );
}
export default Mathematicals;
