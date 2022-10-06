import { useState } from "react"


const useForm = (initialFieldValues:any, validate:any, setCurrentId:any)=>{

    const [values,setValues]=useState(initialFieldValues)
    let b:any ={};
    const [errors,setErrors]=useState(b)
    
    const handleInputChanges= (e:any)=>{
        const {name,value}=e.target
        const fieldValue={[name]:value}
        setValues({
            ...values,
            ...fieldValue
        })
        validate(fieldValue)
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