import {
  Box,
  Container,
  CssBaseline,
} from "@mui/material";
import axios from "axios";
import { useEffect, useState } from "react";
import NavBar from "./NavBar";
import ActivityDashboard from "../../festures/activities/dashboard/ActivityDashboard";

function App() {
  const [activities, SetActivites] = useState<Activity[]>([]);
  const[selectedActivity,setselectedActivity]=useState<Activity|undefined>(undefined);
  const [editMode,setEditMode]=useState(false);
  useEffect(() => {
    axios
      .get<Activity[]>("https://localhost:5001/api/activities")
      .then((response) => SetActivites(response.data));
    return () => {};
  }, [])
  const handleSelectActivity=(id:string)=>{
    setselectedActivity(activities.find(x=>x.id===id));
  }
  const handleCancelSelectedActivity=()=>{
    setselectedActivity(undefined);
  }
  const handleOpenFourm=(id?:string)=>{
    if(id) handleSelectActivity(id);
  else handleCancelSelectedActivity();
  setEditMode(true)
  }
  const handleFormClose=()=>{
    setEditMode(false);
  }
  const handleSubmitForm=(activity:Activity)=>{
    if(activity.id){
      SetActivites(activities.map(x=>x.id===activity.id?activity:x))
  }else{
    const newActivity={...activity,id:activities.length.toString()}
    SetActivites([...activities,newActivity])
  }
  setEditMode(false);
  setselectedActivity(undefined);
}
const handleDelete=(id:string)=>{
  SetActivites(activities.filter(x=>x.id!==id))
}
  return (
    <Box sx={{bgcolor:'#eeeeee'}}>
      <CssBaseline />
      <NavBar openForm={handleOpenFourm}/>
      <Container maxWidth="xl" sx={{ mt: 3 }}>
        <ActivityDashboard 
          activities={activities}
          selectActivity={handleSelectActivity}
          cancelSelectActivity={handleCancelSelectedActivity}
          selectedActivity={selectedActivity}
          editMode={editMode}
          openForm={handleOpenFourm}
          closeForm={handleFormClose}
          submitForm={handleSubmitForm}
          deleteActivity={handleDelete}
        />
      </Container>
    </Box>
  );
}

export default App;
