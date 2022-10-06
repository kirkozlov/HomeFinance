import { ACTION_TYPES } from "../actions/wallet";




interface IWallet{
    id:string;
    name:string;
    groupName:string;
    comment:string;
    balance:number;
}
interface IWalletList{
    list: IWallet[]
}

const initialState:IWalletList = {
    list: []
}



export const wallet = (state = initialState, action:{type:any,payload:any}) => {
    switch (action.type) {
        case ACTION_TYPES.FETCH_ALL:
            return {
                ...state,
                list: [...action.payload]
            }

        case ACTION_TYPES.CREATE:
            return {
                ...state,
                list: [...state.list, action.payload]
            }

        case ACTION_TYPES.UPDATE:
            return {
                ...state,
                list: state.list.map(x=>x.id==action.payload.id?action.payload:x)
            }
        case ACTION_TYPES.DELETE:
            return {
                ...state,
                list: state.list.filter(x=>x.id!=action.payload.id)
            }

        default:
            return state;
    }
}