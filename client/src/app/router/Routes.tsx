import {createBrowserRouter} from "react-router";
import App from "../layout/App";
import HomePage from "../../festures/home/HomePage";
import ActivityDashboard from "../../festures/activities/dashboard/ActivityDashboard";
import ActivityForm from "../../festures/activities/form/ActivityForm";
import ActivityDetailsPage from "../../festures/activities/details/ActivityDetailsPage";
import Counter from "../../festures/counter/Counter";

export const router=createBrowserRouter([
    {
        path:'/',
        element:<App />,
        children:[
            {path:'',element:<HomePage/>},
            {path:'activities',element:<ActivityDashboard />},
            {path:'activities/:id',element:<ActivityDetailsPage />},
            {path:'createActivity',element:<ActivityForm key='Create' />},
            {path:'manage/:id',element:<ActivityForm />},
            {path:'counter',element:<Counter />},
        ]

    }
])