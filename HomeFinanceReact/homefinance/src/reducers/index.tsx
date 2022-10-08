import { combineReducers } from "redux";
import { wallet } from "./wallet";
import { tag } from "./tag";
import { operation } from "./operation";

export const reducers = combineReducers({
    wallet,
    tag,
    operation
})