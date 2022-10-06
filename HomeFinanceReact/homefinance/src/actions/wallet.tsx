
import api from './api'

export const ACTION_TYPES ={
    CREATE : 'CREATE',
    UPDATE : 'UPDATE',
    DELETE : 'DELETE',
    FETCH_ALL : 'FETCH_ALL'

}

export interface IWallet{
    id?:string;
    name:string;
    groupName:string;
    comment:string;
    balance?:number;
}

export const fetchAll = ()=> (dispatch:any)=>{
    api.wallet().fetchAll()
    .then(
        response=>{
            console.log(response);
            dispatch({
                type:ACTION_TYPES.FETCH_ALL,
                payload:response.data
            })
        }
    )
    .catch(err=> console.log(err));
}

export const create = (data:IWallet, onSuccess:any)=> (dispatch:any)=>{
    api.wallet().create(data)
    .then(
        response=>{
            console.log(response);
            dispatch({
                type:ACTION_TYPES.CREATE,
                payload:response.data
            })
            onSuccess()
        }
    )
    .catch(err=> console.log(err));
}

export const update = (data:IWallet, onSuccess:any)=> (dispatch:any)=>{
    api.wallet().update(data)
    .then(
        response=>{
            console.log(response);
            dispatch({
                type:ACTION_TYPES.UPDATE,
                payload:response.data
            })
            onSuccess()
        }
    )
    .catch(err=> console.log(err));
}

export const deleteWallet = (id:string, onSuccess:any)=> (dispatch:any)=>{
    api.wallet().delete(id)
    .then(
        response=>{
            console.log(response);
            dispatch({
                type:ACTION_TYPES.DELETE,
                payload:{id:id}
            })
            onSuccess()
        }
    )
    .catch(err=> console.log(err));
}