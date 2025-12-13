import { Box, Paper, Tab, Tabs } from "@mui/material";
import { useState, type SyntheticEvent } from "react"
import ProfilePhotos from "./ProfilePhotos";
import ProfileAbout from "./ProfileAbout";


export default function ProfileContent() {
  const [value,setValue]=useState(0);
  const handelChange=(_: SyntheticEvent, newValue: number)=>{
    setValue(newValue);
  }
  const tabContent=[
    {Label:'About', content:<ProfileAbout/>},
    {Label:'Photos', content:<ProfilePhotos/>},
    {Label:'Events', content:<div>Events</div>},
    {Label:'Followers', content:<div>Followers</div>},
    {Label:'Following', content:<div>Following</div>},
  ]
    return (
    <Box
    component={Paper}
    mt={2}
    p={3}
    elevation={3}
    height={500}
    sx={{display:'flex', alignItems:'flex-start', borderRadius:3}}
    >
        <Tabs
        orientation="vertical"
        value={value}
        onChange={handelChange}
        sx={{borderRight:1, height:450,minWidth:200}}
        >
            {tabContent.map((tab,index)=>(
                <Tab key={index} label={tab.Label} sx={{mr:3}}/>
            ))}
        </Tabs>
        <Box sx={{flexGrow:1, p:3, pt:0}}>
            {tabContent[value].content}
        </Box>
    </Box>
  )
}