import { ACTION_TYPES_OPERATION, IOperation } from "../contracts/Models";
import api from './api'




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
