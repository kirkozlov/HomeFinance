import { Grid, Paper, TableCell, TableContainer, Table, TableHead, TableRow, TableBody, ButtonGroup, Button } from "@mui/material";
import { styled } from "@mui/system";
import { padding, createTheme, ThemeProvider } from "@mui/material/styles";
import React, { useState, useEffect } from "react";
import { connect } from "react-redux";
import * as actions from "../actions/wallet";
import WalletForm from "./WalletForm";
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
const customTheme = createTheme({
    margin: 16,
    padding: 16,
    components: {

        MuiPaper: {
            styleOverrides: {
                root: {
                    margin: 16,
                    padding: 16,
                },
            },
        },
        MuiTableCell: {
            styleOverrides: {
                head: {
                    fontSize: "1.25rem"
                },
            },
        },
        MuiTextField: {
            styleOverrides: {
                root: {
                    margin: 4,
                },
            },
        },
        MuiButton: {
            styleOverrides: {
                root: {
                    margin: 4,
                },
            },
        },
    },
});

const StyledPaper = styled(Paper)(({ theme }) => ({
    margin: theme.margin,
    padding: theme.padding
}));

const Wallets = (props) => {

    const[currentId,setCurrentId]=useState(0);

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
                                        props.walletList.map((record, index) => {
                                            return (
                                                <TableRow key={index} hover>
                                                    <TableCell>{record.name}</TableCell>
                                                    <TableCell>{record.balance}</TableCell>
                                                    <TableCell>
                                                        <ButtonGroup>
                                                            <Button><EditIcon color="primary" onClick={()=>{setCurrentId(record.id)}}/></Button>
                                                            <Button><DeleteIcon color="secondary"  onClick={()=>props.deleteWallet(record.id,()=>{window.alert("deleted")})}  /></Button>
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

const mapStateToProps = state => ({
    walletList: state.wallet.list
})

const mapActionToProps = {
    fetchAllWallets: actions.fetchAll,
    deleteWallet: actions.deleteWallet
}

export default connect(mapStateToProps, mapActionToProps)(Wallets);