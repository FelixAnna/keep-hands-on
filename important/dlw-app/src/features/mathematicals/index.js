import React from 'react';
import SearchBar from "./searchBar/SearchBar";
import QuestionList from './questionsList/QuestionList';

export function Mathematicals() {

  return (
    <div style={{display: 'flex'}}>
        <SearchBar />
        <QuestionList />
    </div>
    )
}