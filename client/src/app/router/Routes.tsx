import {createBrowserRouter} from "react-router";
import App from "../layout/App";
import HomePage from "../../festures/home/HomePage";
import ActivityDashboard from "../../festures/activities/dashboard/ActivityDashboard";
import ActivityForm from "../../festures/activities/form/ActivityForm";
import ActivityDetails from "../../festures/activities/details/ActivityDetails";

export const router=createBrowserRouter([
    {
        path:'/',
        element:<App />,
        children:[
            {path:'',element:<HomePage/>},
            {path:'activities',element:<ActivityDashboard />},
            {path:'activities/:id',element:<ActivityDetails />},
            {path:'createActivity',element:<ActivityForm key='Create' />},
            {path:'manage/:id',element:<ActivityForm />},
        ]

    }
])