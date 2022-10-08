import { ACTION_TYPES_TAG, ITag } from "../contracts/Models";

const initialState = {
    list: [] as ITag[]
}

export const tag = (state = initialState, action:{type:any,payload:ITag | ITag[]| string}) => {
    switch (action.type) {
        case ACTION_TYPES_TAG.FETCH_ALL:
            return {
                ...state,
                list: [...action.payload as ITag[]]
            }

        case ACTION_TYPES_TAG.CREATE:
            return {
                ...state,
                list: [...state.list, action.payload]
            }

        case ACTION_TYPES_TAG.UPDATE:
            return {
                ...state,
                list: state.list.map(x=>x.name==(action.payload as  ITag).name?action.payload:x)
            }
        case ACTION_TYPES_TAG.DELETE:
            return {
                ...state,
                list: state.list.filter(x=>x.name!=action.payload )
            }

        default:
            return state;
    }
}