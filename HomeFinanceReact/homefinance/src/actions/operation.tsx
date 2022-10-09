import { ACTION_TYPES_OPERATION, IOperation } from "../contracts/Models";
import api from './api'



export const fetchAll = ()=> (dispatch:any)=>{
    api.operation().fetchAll()
    .then(
        response=>{
            console.log(response);
            dispatch({
                type:ACTION_TYPES_OPERATION.FETCH_ALL,
                payload:response.data
            })
        }
    )
    .catch(err=> console.log(err));
}

export const create = (data:IOperation, onSuccess:any)=> (dispatch:any)=>{
    api.operation().create(data)
    .then(
        response=>{
            console.log(response);
            dispatch({
                type:ACTION_TYPES_OPERATION.CREATE,
                payload:{...response.data}
            })
            onSuccess()
        }
    )
    .catch(err=> console.log(err));
}

export const update = (data:IOperation, onSuccess:any)=> (dispatch:any)=>{
    api.operation().update(data)
    .then(
        response=>{
            console.log(response);
            dispatch({
                type:ACTION_TYPES_OPERATION.UPDATE,
                payload:{...response.data}
            })
            onSuccess()
        }
    )
    .catch(err=> console.log(err));
}

export const deleteWallet = (id:string, onSuccess:any)=> (dispatch:any)=>{
    api.operation().delete(id)
    .then(
        response=>{
            console.log(response);
            dispatch({
                type:ACTION_TYPES_OPERATION.DELETE,
                payload:{id:id}
            })
            onSuccess()
        }
    )
    .catch(err=> console.log(err));
}


