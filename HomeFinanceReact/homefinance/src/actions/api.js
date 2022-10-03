import axios from "axios";

const baseUrl = "https://localhost:7080/api/";
//const baseUrl="http://localhost:5080/api/";

export default {
    wallet(url = baseUrl + 'wallet/') {
        const token ="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiI2MDVlMTdiZC1jMzgzLTRiZmQtODJlYS1mM2EwMWM4MjdmZTAiLCJuYmYiOjE2NjQ5MTMwMDQsImV4cCI6MTY2NTM0NTAwMywiaWF0IjoxNjY0OTEzMDA0fQ.PA3gKItIyDn1untjpTs1yVyx7RzfMXZv5H9r50gI5VU"
        const config = {
            headers: { Authorization: `Bearer ${token}` }
        };
        return {
           
            fetchAll: () => axios.get(url, config),
            create: newWallet => axios.post(url, newWallet, config),
            update: wallet => axios.put(url, wallet,config),
            delete: id => axios.delete(url + id, config),
        }
    }
}