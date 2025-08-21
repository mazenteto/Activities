import { List, ListItem, ListItemText, Typography } from "@mui/material";
import axios from "axios";
import { useEffect, useState } from "react";

function App() {
const [activities,SetActivites]= useState<Activity[]>([]);
useEffect(()=>{
axios.get<Activity[]>('https://localhost:5001/api/activities')
  .then(response=> SetActivites(response.data))
return()=>{}
},[])
  return (
    <>
      <Typography variant="h3"> Activities</Typography>
      <List>
        {activities.map((activity)=>(
          <ListItem key={activity.id}>
            <ListItemText>
              {activity.title}
            </ListItemText>
          </ListItem>
        ))}
      </List>
    </>
  )
}

export default App
 