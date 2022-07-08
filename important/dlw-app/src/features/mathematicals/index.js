import React from 'react';
import SearchBar from "./searchBar/SearchBar";
import QuestionList from './questionsList/QuestionList';
import CriteriaList from './criteriaList/CriteriaList';

export function Mathematicals() {

  return (
    <div style={{display: 'flex'}}>
        <SearchBar />
        <CriteriaList />
        <QuestionList />
    </div>
    )
}