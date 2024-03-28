import { Link } from '@mui/material';
import React from 'react';

function Home() {
  return (
    <div>
      <div><Link href="zhuyin">文字转拼音</Link></div>
      <div><Link href="math">数学</Link></div>
      <div><Link href="zdj">ZDJ</Link></div>
      <div><Link href="memo">记事本</Link></div>
    </div>
  );
}

export default Home;
