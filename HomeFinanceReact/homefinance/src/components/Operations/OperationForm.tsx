import React, { useEffect, useState } from "react";
import { IOperation, ITag, IWallet, OperationType } from "../../contracts/Models";
import * as operationActions from "../../actions/operation";
import * as walletActions from "../../actions/wallet";
import * as tagActions from "../../actions/tag";
import { connect } from "react-redux";
import { Grid, TextField, Button, ThemeProvider, Paper } from "@mui/material";
import useForm from "../useForm";
import { notify } from "../../App";
import { customTheme } from "../../contracts/theme";
import { DateTimePicker } from '@mui/x-date-pickers/DateTimePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import dayjs, { Dayjs } from 'dayjs';
import BackspaceIcon from '@mui/icons-material/Backspace';
const initialFieldValues: IOperation = {
    id: null,
    walletId: "",
    operationType: OperationType.Expense,
    tags: [],
    amount: 0.0,
    comment: "",
    walletIdTo: null,
    dateTime: new Date(),
}



const SelectAmount = ({ initialValue, handleAmountOk }: { initialValue: string, handleAmountOk: any }) => {


    const [amount, setAmount] = React.useState(initialValue)
    const handleAmountChange = (e: any) => {
        setAmount(e.target.value);
    }

    const okClicked = () => {
        handleAmountOk(Number(amount))
    }

    const numberClick = (value: string) => {
        setAmount((amount + value).replace(/^0+/, ''));
    }
    const backspaceClick = () => {
        setAmount(amount.substring(0,amount.length-1));
    }

    return (
        <div>
            <Grid>
                <h2>Select Amount</h2>
                <TextField name="amount" variant="outlined" label="Amount" value={amount} onChange={handleAmountChange} inputProps={{ readOnly: true }}/>

                <Grid >
                    <Button variant="outlined" onClick={() => numberClick('7')}> 7 </Button>
                    <Button variant="outlined" onClick={() => numberClick('8')}> 8 </Button>
                    <Button variant="outlined" onClick={() => numberClick('9')}> 9 </Button>
                    <Button variant="outlined" onClick={() => backspaceClick()}> <BackspaceIcon /> </Button>
                </Grid>
                <Grid>
                    <Button variant="outlined" onClick={() => numberClick('4')}> 4 </Button>
                    <Button variant="outlined" onClick={() => numberClick('5')}> 5 </Button>
                    <Button variant="outlined" onClick={() => numberClick('6')}> 6 </Button>
                </Grid>
                <Grid >
                    <Button variant="outlined" onClick={() => numberClick('1')}> 1 </Button>
                    <Button variant="outlined" onClick={() => numberClick('2')}> 2 </Button>
                    <Button variant="outlined" onClick={() => numberClick('3')}> 3 </Button>
                </Grid>
                <Grid >
                    <Button variant="outlined" onClick={() => numberClick('0')}> 0 </Button>
                    <Button variant="outlined" onClick={() => numberClick('00')}> 00 </Button>
                    <Button variant="outlined" onClick={() => numberClick('.')}> , </Button>
                </Grid>

                <Button onClick={() => okClicked()}> Next </Button>
            </Grid>
        </div>
    )
}


const OptionsForm = (props: any) => {

    useEffect(() => {
        props.fetchAllWallets()
        props.fetchAllTags()
    }, []);


    const validate: any = (fieldValues = values) => {
        let tmp = errors;
        if ('amount' in fieldValues)
            tmp.amount = fieldValues.amount > 0 ? "" : "Amount can not be negative"
        setErrors({
            ...tmp
        })
        if (fieldValues == values)
            return Object.values(tmp).every(x => x == "")
    }

    const { values, setValues, errors, setErrors, handleInputChanges, resetForm } = useForm(initialFieldValues, validate, props.setCurrentId)

    const handleSubmit = (e: any) => {
        e.preventDefault()
        if (validate()) {
            if (props.currentId != 0)
                props.createOperation(values, () => { notify("changed"); resetForm() })
            else
                props.createOperation(values, () => { notify("saved"); resetForm() })
        }

    }

    const centeredStyle = { display: 'flex', alignItems: 'center', justifyContent: 'center' }


    const [showSelectWallet, setShowSelectWallet] = React.useState(true)
    const [showSelectDateTime, setShowSelectDateTime] = React.useState(false)

    const [showSelectType, setShowSelectType] = React.useState(false)
    const [showSelectTag, setShowSelectTag] = React.useState(false)
    const [showSelectAmount, setShowSelectAmount] = React.useState(false)

    const handleWalletClick = (id: string) => {
        setValues({ ...values, walletId: id });
        setShowSelectWallet(false);
        setShowSelectDateTime(true);
    }
    const SelectWallet = () => (
        <div >
            <Grid >
                <h2 style={centeredStyle}>Select Wallet</h2>
                <Grid container style={centeredStyle}>
                    {
                        props.walletList.map((record: IWallet, index: number) => {
                            return (
                                <Button key={index} onClick={() => handleWalletClick(record.id!)} variant="outlined" >{record.name}</Button>
                            )
                        })
                    }
                </Grid>
            </Grid>
        </div>
    )


    const handleDateTimeChange = (e: any) => {
        // handleInputChanges({ target: { name: e.target.name, value: Number(e.target.value) } });
    }
    const handleDateTimeOk = () => {
        setShowSelectDateTime(false);
        setShowSelectType(true);
    }
    const SelectDateTime = () => (
        <div>
            <Grid >
                <h2 >Select DateTime</h2>
                <Grid container>
                    <LocalizationProvider dateAdapter={AdapterDayjs}>
                        <DateTimePicker
                            label="Date&Time picker"
                            value={values.dateTime}
                            onChange={handleDateTimeChange}
                            renderInput={(params) => <TextField {...params} />}
                        />
                    </LocalizationProvider>
                </Grid>
                <Button onClick={() => handleDateTimeOk()}> Next </Button>
            </Grid>
        </div>
    )

    const handleTypeClick = (operationType: OperationType) => {
        setValues({ ...values, operationType: operationType });
        setShowSelectType(false);
        setShowSelectTag(true);
    }
    const SelectType = () => (
        <div>
            <Grid >
                <h2 >Select Type</h2>
                <Grid container>
                    {
                        (Object.keys(OperationType).filter((v) => isNaN(Number(v))) as (keyof typeof OperationType)[]).map((record: keyof typeof OperationType, index: number) => {
                            return (
                                <Button key={index} onClick={() => handleTypeClick(OperationType[record])} variant="outlined" >{record}</Button>
                            )
                        })
                    }
                </Grid>
            </Grid>
        </div>
    )

    const handleTagClick = (tagName: string) => {
        if (values.tags.some(x => x == tagName)) {
            setValues({ ...values, tags: values.tags.filter(x => x != tagName) });
        }
        else {
            setValues({ ...values, tags: [...values.tags, tagName] });
        }
    }
    const handleTagOk = () => {
        if (values.tags.length > 0) {
            setShowSelectTag(false);
            setShowSelectAmount(true);
        }
    }
    const SelectTags = () => (
        <div>
            <Grid >
                <h2 >Select Tags</h2>
                <Grid container>
                    {
                        props.tagList.map((record: ITag, index: number) => {
                            return (
                                <Button key={index} onClick={() => handleTagClick(record.name)} variant="contained" {...(values.tags.some(x => x == record.name) && { color: "success" })} >{record.name}</Button>
                            )
                        })
                    }
                </Grid>
                <Button onClick={() => handleTagOk()}> Next </Button>
            </Grid>
        </div>
    )


    const handleAmountOk = (amount: number) => {
        const newValues = handleInputChanges({ target: { name: "amount", value: amount } });
        if (newValues.amount > 0) {
            setShowSelectAmount(false);
            props.createOperation(newValues, () => { notify("saved"); resetForm() });
            setShowSelectWallet(true);
        }
    }


    return (
        <ThemeProvider theme={customTheme}>
            <Paper elevation={8}>
                {showSelectWallet ? <SelectWallet /> : null}
                {showSelectType ? <SelectType /> : null}
                {showSelectTag ? <SelectTags /> : null}
                {showSelectAmount ? <SelectAmount initialValue={String(values.amount)} handleAmountOk={handleAmountOk} /> : null}
                {showSelectDateTime ? <SelectDateTime /> : null}
            </Paper>
        </ThemeProvider>
    );
}


const mapStateToProps = (state: any) => ({
    walletList: state.wallet.list,
    tagList: state.tag.list
})

const mapActionToProps = {
    createOperation: operationActions.create,
    fetchAllWallets: walletActions.fetchAll,
    fetchAllTags: tagActions.fetchAll,
}

export default connect(mapStateToProps, mapActionToProps)(OptionsForm);