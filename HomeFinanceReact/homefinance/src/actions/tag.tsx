import { ACTION_TYPES_TAG, ITag } from "../contracts/Models";
import api from './api'

export const fetchAll = ()=> (dispatch:any)=>{
    api.tag().fetchAll()
    .then(
        response=>{
            console.log(response);
            dispatch({
                type:ACTION_TYPES_TAG.FETCH_ALL,
                payload:response.data
            })
        }
    )
    .catch(err=> console.log(err));
}

export const create = (data:ITag, onSuccess:any)=> (dispatch:any)=>{
    api.tag().create(data)
    .then(
        response=>{
            console.log(response);
            dispatch({
                type:ACTION_TYPES_TAG.CREATE,
                payload:response.data
            })
            onSuccess()
        }
    )
    .catch(err=> console.log(err));
}

export const update = (data:ITag, onSuccess:any)=> (dispatch:any)=>{
    api.tag().update(data)
    .then(
        response=>{
            console.log(response);
            dispatch({
                type:ACTION_TYPES_TAG.UPDATE,
                payload:response.data
            })
            onSuccess()
        }
    )
    .catch(err=> console.log(err));
}

export const deleteWallet = (name:string, onSuccess:any)=> (dispatch:any)=>{
    api.tag().delete(name)
    .then(
        response=>{
            console.log(response);
            dispatch({
                type:ACTION_TYPES_TAG.DELETE,
                payload:{name:name}
            })
            onSuccess()
        }
    )
    .catch(err=> console.log(err));
}