import { useState } from "react"


const useForm = <T,>(initialFieldValues:T, validate:any, setCurrentId:any)=>{

    const [values,setValues]=useState(initialFieldValues)
    let b:any ={};
    const [errors,setErrors]=useState(b)
    
    const handleInputChanges= (e:any):T=>{
        const {name,value}=e.target
        const fieldValue={[name]:value}
        const newValues={
            ...values,
            ...fieldValue
        }

        setValues(newValues)
        validate(fieldValue)

        return newValues;
    }

    const resetForm=()=>{
        setValues({
            ...initialFieldValues
        })
        setErrors({})
        setCurrentId(0)
    }

    return {
        values,
        setValues,
        errors,
        setErrors,
        handleInputChanges,
        resetForm
    }
}

export default useForm;