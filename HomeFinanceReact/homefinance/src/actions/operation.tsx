import { ACTION_TYPES_OPERATION, IWallet } from "../contracts/Models";
import api from './api'




export const create = (data:IWallet, onSuccess:any)=> (dispatch:any)=>{
    api.wallet().create(data)
    .then(
        response=>{
            console.log(response);
            dispatch({
                type:ACTION_TYPES_OPERATION.CREATE,
                payload:{...response.data, balance:0}
            })
            onSuccess()
        }
    )
    .catch(err=> console.log(err));
}
