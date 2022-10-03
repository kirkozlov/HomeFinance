import { Grid, TextField, Button } from "@mui/material";
import React,{useEffect, useState} from "react";
import useForm from "./useForm";
import { connect } from "react-redux";
import * as actions from "../actions/wallet";
const initialFieldValues={
    name:"",
    groupName:"",
    comment:""
}

const WalletForm=(props)=>{
    const validate=(fieldValues=values)=>{
        let tmp={...errors}
        if('name' in fieldValues)
            tmp.name = fieldValues.name?"":"This field is required"
        if('groupName' in fieldValues)
            tmp.groupName = fieldValues.groupName?"":"This field is required"
        setErrors({
            ...tmp
        })
        if(fieldValues==values)
            return Object.values(tmp).every(x=>x=="")
    }

    const{values,setValues,errors,setErrors,handleInputChanges, resetForm}=useForm(initialFieldValues, validate, props.setCurrentId)
   

   
    const handleSubmit=e=>{
        e.preventDefault()
        if(validate()){
            if(props.currentId!=0)
                props.updateWallet(values,()=>{window.alert("changed")})
            else
                props.createWallet(values,()=>{window.alert("saved")})
        }
        resetForm()
    }

    useEffect(()=>{
        if(props.currentId!=0){
            setValues({
                ...props.walletList.find(x=>x.id==props.currentId)
            })
            setErrors({})
        }
    },[props.currentId])

    return ( 
        <form autoComplete="off" noValidate onSubmit={handleSubmit}>
            <Grid container>
                <Grid item xs={6}>
                    <TextField name="name" variant="outlined" label="Name" value={values.name} onChange={handleInputChanges} {...(errors.name && {error:true, helperText: errors.name})}/>
                    
                    <TextField name="groupName" variant="outlined" label="Group" value={values.groupName} onChange={handleInputChanges} {...(errors.groupName && {error:true, helperText: errors.groupName})}/>
                    <TextField name="comment" variant="outlined" label="Comment" value={values.comment} onChange={handleInputChanges}/>
                </Grid>
                <Grid item xs={6}>
                    <Button variant="contained" color="primary" type="submit"> Save </Button>
                    <Button variant="contained" color="inherit" onClick={resetForm}> Reset </Button>
                </Grid>
            </Grid>
        </form>
     );
}
const mapStateToProps = state => ({
    walletList: state.wallet.list
})

const mapActionToProps = {
    createWallet: actions.create,
    updateWallet: actions.update
}

export default connect(mapStateToProps, mapActionToProps)(WalletForm);