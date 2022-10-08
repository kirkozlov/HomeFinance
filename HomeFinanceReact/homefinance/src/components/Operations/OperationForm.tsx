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

    const centeredStyle={display: 'flex',  alignItems: 'center', justifyContent: 'center' }


    const [showSelectWallet, setShowSelectWallet] = React.useState(true)
    const [showSelectType, setShowSelectType] = React.useState(false)
    const [showSelectTag, setShowSelectTag] = React.useState(false)
    const [showSelectAmount, setShowSelectAmount] = React.useState(false)
    const [showSelectDateTime, setShowSelectDateTime] = React.useState(false)

    const handleWalletClick = (id: string) => {
        setValues({ ...values, walletId: id });
        setShowSelectWallet(false);
        setShowSelectType(true);
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

    const handleAmountChange = (e: any) => {
        handleInputChanges({ target: { name: e.target.name, value: Number(e.target.value) } });
    }
    const handleAmountOk = () => {
        if (values.amount > 0) {
            setShowSelectAmount(false);
            setShowSelectDateTime(true);
        }
    }
    const SelectAmount = () => (
        <div>
            <Grid >
                <h2 >Select Amount</h2>
                <Grid container>
                    <TextField name="amount" variant="outlined" label="Amount" value={values.amount} type="number" onChange={handleAmountChange} inputProps={{ min: 0 }} />
                </Grid>
                <Button onClick={() => handleAmountOk()}> Next </Button>
            </Grid>
        </div>
    )

    const handleDateTimeChange = (e: any) => {
        // handleInputChanges({ target: { name: e.target.name, value: Number(e.target.value) } });
    }
    const handleDateTimeOk = () => {
        setShowSelectDateTime(false);
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

    return (
        <ThemeProvider theme={customTheme}>
            <Paper elevation={8}>
                {showSelectWallet ? <SelectWallet /> : null}
                {showSelectType ? <SelectType /> : null}
                {showSelectTag ? <SelectTags /> : null}
                {showSelectAmount ? <SelectAmount /> : null}
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