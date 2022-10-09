import { Grid, Paper, ThemeProvider } from "@mui/material";
import React, { useEffect } from "react";
import { customTheme } from "../../contracts/theme";
import * as operationActions from "../../actions/operation";
import * as walletActions from "../../actions/wallet";
import { connect } from "react-redux";
import { IOperation } from "../../contracts/Models";

const groupBy = <T, K extends keyof any>(arr: T[], key: (i: T) => K) =>
    arr.reduce((groups, item) => {
        (groups[key(item)] ||= []).push(item);
        return groups;
    }, {} as Record<K, T[]>);
const toDate = (dateTime: Date) => {
    const date = new Date(dateTime.getFullYear(), dateTime.getMonth(), dateTime.getDate());
    return date;
}

const Operations = (props: any) => {

    useEffect(() => {
        props.fetchAllOperations()
        props.fetchAllWallets()
    }, []);

    const now = new Date();
    const yearAndMonth = { year: now.getFullYear(), month: now.getMonth() }

    const groupedOperations = groupBy((props.operationList as IOperation[]), i => toDate(i.dateTime).toString());

    const days = Object.keys(groupedOperations).map(i => new Date(i)).filter(i => i.getFullYear() == yearAndMonth.year && i.getMonth() == yearAndMonth.month);

    return (
        <ThemeProvider theme={customTheme}>
            <Paper elevation={8}>
                <h1>All Operations</h1>
                <h2>{yearAndMonth.year}/{yearAndMonth.month}</h2>
                <Grid container>
                    {
                        days.map((record: Date, index: number) => {
                            return (
                                <Paper elevation={8} key={index}>
                                    <Grid container>
                                        <h3>{record.getDate().toString()}</h3>
                                        {
                                            groupedOperations[record.toString()].map((operation:IOperation, indexOperation:number)=>{
                                                return (
                                                    <Paper elevation={1} key={indexOperation}>
                                                        {operation.tags.join(' ')} {operation.amount}
                                                    </Paper>
                                                )
                                            })
                                        }
                                    </Grid>
                                </Paper>
                            )
                        })
                    }
                </Grid>

            </Paper>
        </ThemeProvider>
    );
}



const mapStateToProps = (state: any) => ({
    walletList: state.wallet.list,
    operationList: state.operation.list
})

const mapActionToProps = {
    fetchAllOperations: operationActions.fetchAll,
    fetchAllWallets: walletActions.fetchAll,
}

export default connect(mapStateToProps, mapActionToProps)(Operations);