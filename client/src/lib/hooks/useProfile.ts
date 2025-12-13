import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import agent from "../api/agent";
import { useMemo } from "react";
import { Photo } from "@mui/icons-material";

export const useProfile=(id?:string)=>{
    const queryClient=useQueryClient();
    const{data:profile,isLoading: loadingProfile}=useQuery<profile>({
        queryKey:['profile',id],
        queryFn:async()=>{
            const response=await agent.get<profile>(`/profiles/${id}`);
            return response.data
        },
        enabled:!!id
    })
    const {data:photos, isLoading:loadingPhotos}=useQuery<Photo[]>({
        queryKey:['photos',id],
        queryFn:async()=>{
            const response=await agent.get<Photo[]>(`/profiles/${id}/photos`)
            return response.data;
        },
        enabled:!!id
    });

    const uploadPhoto=useMutation({
        mutationFn: async (file: Blob)=>{
            const formData= new FormData();
            formData.append('file',file);
            const response=await agent.post('/profiles/add-photo',formData,{
                headers:{"Content-Type":'multipart/formdata'}
            });
            return response.data;
        }, onSuccess:async(photo:Photo)=>{
            await queryClient.invalidateQueries({
                queryKey:['photos',id]
            });
            queryClient.setQueryData(['user'],(data:User)=>{
                if(!data) return data;
                return {
                    ...data,
                    imageUrl:data.imageUrl?? photo.url
                }
            });
            queryClient.setQueryData(['profile'],(data:profile)=>{
                if(!data) return data;
                return {
                    ...data,
                    imageUrl:data.imageUrl?? photo.url
                }
            })
        }
    })
    const setMainPhoto=useMutation({
        mutationFn:async(photo:Photo)=>{
            await agent.put(`/profiles/${photo.id}/setmain`)
        },
        onSuccess:(_,photo)=>{
            queryClient.setQueryData(['user'],(userData:User)=>{
                if(!userData)return userData;
                return{
                    ...userData,
                    imageUrl:photo.url
                }
            })
            queryClient.setQueryData(['profile',id],(profile:profile)=>{
                if(!profile)return profile;
                return{
                    ...profile,
                    imageUrl:photo.url
                }
            })
        }
    })

    const deletePhoto=useMutation({
        mutationFn:async(photoId:string)=>{
            await agent.delete(`/profiles/${photoId}/photos`)
        },
        // onSuccess:async()=>{
        //     await queryClient.invalidateQueries({
        //         queryKey:['photos',queryClient.getQueryData<User>(['user'])?.id]
        //     });
        // }
        onSuccess:(_,photoId)=>{
            queryClient.setQueryData(['photos',id],(photos:Photo[])=>{
                return photos.filter(x=>x.id!==photoId)
            })

        }
    })
    const isCurrentUser=useMemo(()=>{
        return id===queryClient.getQueryData<User>(['user'])?.id
    },[id,queryClient])
    return{
        profile,
        loadingProfile,
        photos,
        loadingPhotos,
        isCurrentUser,
        uploadPhoto,
        setMainPhoto,
        deletePhoto
    }
}