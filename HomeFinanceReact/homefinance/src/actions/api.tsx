import axios from "axios";
import { OperationCanceledException } from "typescript";
import { IOperation, ITag, IWallet } from "../contracts/Models";
//const baseUrl = "https://192.168.1.6:7080/api/";
const baseUrl = "https://localhost:7080/api/";
//const baseUrl="http://localhost:5080/api/";
const token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiJlMTQ4N2IzMS05NTY1LTRlNjEtOWI4NS1lZGQ3YjNjM2FkNGUiLCJuYmYiOjE2NjUyMjYxMDYsImV4cCI6MTY2NTY1ODEwNiwiaWF0IjoxNjY1MjI2MTA2fQ.PmaMTNTLd7msC70wV5ZFDzOqM3aARAJqTsctrAOCAYk";
const config = {
    headers: { Authorization: `Bearer ${token}`, Accept: 'application/json' }
};

export default {
    wallet(url = baseUrl + 'wallet/') {


        return {

            fetchAll: () => axios.get(url, config),
            create: (newWallet: IWallet) => axios.post(url, newWallet, config),
            update: (wallet: IWallet) => axios.put(url, wallet, config),
            delete: (id: string) => axios.delete(url + id, config),
        }
    },

    operation(url = baseUrl + 'operation/') {
        return {
            fetchAll: () => axios.get<IOperation>(url, config),
            create: (newOperation: IOperation) => axios.post(url, newOperation, config),
            update: (operation: IOperation) => axios.put(url, operation, config),
            delete: (id: string) => axios.delete(url + id, config),
        }
    },
    tag(url = baseUrl + 'tag/') {
        return {
            fetchAll: () => axios.get(url, config),
            create: (newTag: ITag) => axios.post(url, newTag, config),
            update: (tag: ITag) => axios.put(url, tag, config),
            delete: (name: string) => axios.delete(url + name, config),
        }
    },
}