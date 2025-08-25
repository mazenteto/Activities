import {
  Box,
  Container,
  CssBaseline,
  Typography,
} from "@mui/material";
import {useState } from "react";
import NavBar from "./NavBar";
import ActivityDashboard from "../../festures/activities/dashboard/ActivityDashboard";
import { useActivities } from "../../lib/types/hooks/useActivities";

function App() {

  const[selectedActivity,setselectedActivity]=useState<Activity|undefined>(undefined);
  const [editMode,setEditMode]=useState(false);
  const {activities,isPending}=useActivities();
  const handleSelectActivity=(id:string)=>{
    setselectedActivity(activities!.find(x=>x.id===id));
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
  return (
    <Box sx={{bgcolor:'#eeeeee',minHeight:'100vh'}}>
      <CssBaseline />
      <NavBar openForm={handleOpenFourm}/>
      <Container maxWidth="xl" sx={{ mt: 3 }}>
        {!activities||isPending?(
          <Typography>Loading....</Typography>
        ):(
          <ActivityDashboard 
          activities={activities}
          selectActivity={handleSelectActivity}
          cancelSelectActivity={handleCancelSelectedActivity}
          selectedActivity={selectedActivity}
          editMode={editMode}
          openForm={handleOpenFourm}
          closeForm={handleFormClose}
        />

        )}

      </Container>
    </Box>
  );
}

export default App;
