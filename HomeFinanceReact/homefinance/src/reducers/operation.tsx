import { ACTION_TYPES_OPERATION, IOperation } from "../contracts/Models";

const initialState = {
    list: [] as IOperation[]
}

export const operation = (state = initialState, action:{type:any,payload:IOperation | IOperation[] | string}) => {
    switch (action.type) {
        case ACTION_TYPES_OPERATION.FETCH_ALL:
            return {
                ...state,
                list: [...action.payload as IOperation[]]
            }

        case ACTION_TYPES_OPERATION.CREATE:
            return {
                ...state,
                list: [...state.list, action.payload]
            }

        case ACTION_TYPES_OPERATION.UPDATE:
            return {
                ...state,
                list: state.list.map(x=>x.id==(action.payload as IOperation).id?action.payload:x)
            }
        case ACTION_TYPES_OPERATION.DELETE:
            return {
                ...state,
                list: state.list.filter(x=>x.id!=action.payload)
            }

        default:
            return state;
    }
}