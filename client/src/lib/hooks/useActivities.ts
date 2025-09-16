import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agent from "../api/Agent";
import { useLocation } from "react-router";


export const useActivities = (id?:string)=>{
    const queryClient=useQueryClient();
    const location=useLocation();

     const {data:activities,isPending}=useQuery({
      queryKey:['Activities'],
      queryFn:async ()=>{
        const response = await agent.get<Activity[]>('/activities')
        return response.data;
      }, 
      enabled:!id && (location.pathname==='/activities'||location.pathname==='/Activities'),
    //   staleTime:1000*60*5 //cash for 5 mins
     });

     const {data:activity,isLoading:isLoadingActivity}=useQuery({
        queryKey:['activities',id],
        queryFn:async()=>{
            const response = await agent.get<Activity>(`/activities/${id}`);
            return response.data;
        },
        enabled:!!id
     })

     const updateActivity=useMutation({
        mutationFn:async (activity:Activity)=>{
            await agent.put('/activities',activity)
        },
        onSuccess:async()=>{
            await queryClient.invalidateQueries({
                queryKey:['Activities']
            })

        }
     });
    const createActivity=useMutation({
        mutationFn:async (activity:Activity)=>{
          const response=  await agent.post('/activities',activity);
          return response.data;
        },
        onSuccess:async()=>{
            await queryClient.invalidateQueries({
                queryKey:['Activities']
            })

        }
     });
     const deleteActivity=useMutation({
        mutationFn:async (id:string)=>{
            await agent.delete(`/activities/${id}`)
        },
        onSuccess:async()=>{
            await queryClient.invalidateQueries({
                queryKey:['Activities']
            })

        }
     });
     return {
        activities,
        isPending,
        updateActivity,
        createActivity,
        deleteActivity,
        activity,
        isLoadingActivity
     }
}