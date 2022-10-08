import { Grid, Paper, TableCell, TableContainer, Table, TableHead, TableRow, TableBody, ButtonGroup, Button } from "@mui/material";
import { styled } from "@mui/system";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import React, { useState, useEffect } from "react";
import { connect } from "react-redux";
import * as actions from "../actions/wallet";
import WalletForm from "./WalletForm";
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import { notify } from "../App";
import { IWallet } from "../contracts/Models";
import { customTheme } from "../contracts/theme";


const StyledPaper = styled(Paper)(({ theme }) => ({
    margin: 16,
    padding: 16
}));

const Wallets = (props:any) => {

    const[currentId,setCurrentId]=useState("");

    useEffect(() => {
        props.fetchAllWallets()

    }, []);

    return (
        <ThemeProvider theme={customTheme}>
            <Paper elevation={8}>
                <Grid container>
                    <Grid item xs={6}>
                        <WalletForm {...({currentId, setCurrentId})}/>
                    </Grid>
                    <Grid item xs={6}>
                        <TableContainer>
                            <Table>
                                <TableHead>
                                    <TableRow>
                                        <TableCell>Name</TableCell>
                                        <TableCell>Balance</TableCell>
                                        <TableCell></TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {
                                        props.walletList.map((record:IWallet, index:number) => {
                                            return (
                                                <TableRow key={index} hover>
                                                    <TableCell>{record.name}</TableCell>
                                                    <TableCell>{record.balance}</TableCell>
                                                    <TableCell>
                                                        <ButtonGroup>
                                                            <Button><EditIcon color="primary" onClick={()=>{setCurrentId(record.id!)}}/></Button>
                                                            <Button><DeleteIcon color="secondary"  onClick={()=>props.deleteWallet(record.id,()=>{notify("deleted")})}  /></Button>
                                                        </ButtonGroup>
                                                    </TableCell>
                                                </TableRow>)
                                        })
                                    }
                                </TableBody>
                            </Table>
                        </TableContainer>
                    </Grid>
                </Grid>
            </Paper>
        </ThemeProvider>
    );
}

const mapStateToProps = (state:any) => ({
    walletList: state.wallet.list
})

const mapActionToProps = {
    fetchAllWallets: actions.fetchAll,
    deleteWallet: actions.deleteWallet
}

export default connect(mapStateToProps, mapActionToProps)(Wallets);