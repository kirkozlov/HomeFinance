import { ACTION_TYPES_WALLET, IWallet } from "../contracts/Models";

const initialState = {
    list: [] as IWallet[]
}

export const wallet = (state = initialState, action:{type:any,payload:IWallet | IWallet[]| string} ) => {
    switch (action.type) {
        case ACTION_TYPES_WALLET.FETCH_ALL:
            return {
                ...state,
                list: [...action.payload as IWallet[]]
            }

        case ACTION_TYPES_WALLET.CREATE:
            return {
                ...state,
                list: [...state.list, action.payload]
            }

        case ACTION_TYPES_WALLET.UPDATE:
            return {
                ...state,
                list: state.list.map(x=>x.id==(action.payload as IWallet).id?action.payload:x)
            }
        case ACTION_TYPES_WALLET.DELETE:
            return {
                ...state,
                list: state.list.filter(x=>x.id!=action.payload)
            }

        default:
            return state;
    }
}