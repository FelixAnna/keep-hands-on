import * as React from 'react';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemText from '@mui/material/ListItemText';
import ListItemAvatar from '@mui/material/ListItemAvatar';
import Avatar from '@mui/material/Avatar';
import SendIcon from '@mui/icons-material/Send';

function Home() {
  return (
    <div>
      <List sx={{ width: '100%', maxWidth: 360, bgcolor: 'background.paper' }}>
        <ListItem>
          <ListItemAvatar>
            <Avatar>
              <SendIcon />
            </Avatar>
          </ListItemAvatar>
          <ListItemText primary="拼音" secondary="转换简体字为拼音" />
        </ListItem>
        <ListItem>
          <ListItemAvatar>
            <Avatar>
              <SendIcon />
            </Avatar>
          </ListItemAvatar>
          <ListItemText primary="字库" secondary="提供学习助手软件的字库下载" />
        </ListItem>
        <ListItem>
          <ListItemAvatar>
            <Avatar>
              <SendIcon />
            </Avatar>
          </ListItemAvatar>
          <ListItemText primary="数学" secondary="出数学题（需要调用其他服务）" />
        </ListItem>
        <ListItem>
          <ListItemAvatar>
            <Avatar>
              <SendIcon />
            </Avatar>
          </ListItemAvatar>
          <ListItemText primary="记事本" secondary="记录重要的日期（需要调用其他服务）" />
        </ListItem>
        <ListItem>
          <ListItemAvatar>
            <Avatar>
              <SendIcon />
            </Avatar>
          </ListItemAvatar>
          <ListItemText primary="指导价" secondary="查询深圳小区指导价（需要调用其他服务）" />
        </ListItem>
      </List>
    </div>
  );
}

export default Home;
