import { createTheme } from "@mui/material/styles";


export const customTheme = createTheme({
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