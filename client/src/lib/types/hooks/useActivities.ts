import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agent from "./API/Agent";

export const useActivities = ()=>{
    const queryClient=useQueryClient();
     const {data:activities,isPending}=useQuery({
      queryKey:['Activities'],
      queryFn:async ()=>{
        const response = await agent.get<Activity[]>('/activities')
        return response.data;
      }
     });

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
            await agent.post('/activities',activity)
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
        deleteActivity
     }
}