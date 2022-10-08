export interface IWallet{
    id:string|null;
    name:string;
    groupName:string;
    comment:string;
    balance?:number;
}

export interface ITag{
    name:string;
    comment:string;
}

export enum OperationType{
    Income, 
    Expense, 
    Transfer
}

export interface IOperation{
    id:string|null;
    walletId:string;
    operationType:OperationType;
    tags:string[];
    amount:number;
    comment:string;
    walletIdTo:string|null;
    dateTime:Date;
}

export const ACTION_TYPES_WALLET ={
    CREATE : 'CREATE_WALLET',
    UPDATE : 'UPDATE_WALLET',
    DELETE : 'DELETE_WALLET',
    FETCH_ALL : 'FETCH_ALL_WALLET'
}
export const ACTION_TYPES_OPERATION ={
    CREATE : 'CREATE_OPERATION',
    UPDATE : 'UPDATE_OPERATION',
    DELETE : 'DELETE_OPERATION',
    FETCH_ALL : 'FETCH_ALL_OPERATION'
}
export const ACTION_TYPES_TAG ={
    CREATE : 'CREATE_TAG',
    UPDATE : 'UPDATE_TAG',
    DELETE : 'DELETE_TAG',
    FETCH_ALL : 'FETCH_ALL_TAG'
}