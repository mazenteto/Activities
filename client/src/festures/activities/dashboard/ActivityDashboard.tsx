import { Grid } from "@mui/material";
import ActivityList from "./ActivityList";
import ActivityDetails from "../details/ActivityDetails";
import ActivityForm from "../form/ActivityForm";
type props={
    activities:Activity[]
    selectActivity:(id:string)=>void;
    cancelSelectActivity:()=>void;
    selectedActivity?:Activity;
    openForm:(id:string)=>void;
    closeForm:()=>void;
    editMode:boolean

}

export default function ActivityDashboard({
  activities,
  cancelSelectActivity,
  selectActivity,
  selectedActivity,
  openForm,
  closeForm,
  editMode,

}:props) {
  return (
    <Grid container  spacing={3}>
        <Grid size={7}>
            <ActivityList 
              activities={activities}
              selectActivity={selectActivity}

            />
        </Grid>
        <Grid size={5}>
          {selectedActivity&&!editMode&&<ActivityDetails 
            selectedActivity={selectedActivity}
            cancelSelectActivity={cancelSelectActivity}
            openForm={openForm}
          
          />
          }
          {editMode&&
          <ActivityForm 
            closeForm={closeForm}
            activity={selectedActivity}
          />}
        </Grid>
    </Grid>
  )
}