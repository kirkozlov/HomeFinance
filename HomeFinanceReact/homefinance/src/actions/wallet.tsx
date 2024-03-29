import { ACTION_TYPES_WALLET, IWallet } from "../contracts/Models";
import api from './api'

export const fetchAll = ()=> (dispatch:any)=>{
    api.wallet().fetchAll()
    .then(
        response=>{
            console.log(response);
            dispatch({
                type:ACTION_TYPES_WALLET.FETCH_ALL,
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
                type:ACTION_TYPES_WALLET.CREATE,
                payload:{...response.data, balance:0}
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
                type:ACTION_TYPES_WALLET.UPDATE,
                payload:{...response.data, balance:data.balance}
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
                type:ACTION_TYPES_WALLET.DELETE,
                payload:{id:id}
            })
            onSuccess()
        }
    )
    .catch(err=> console.log(err));
}