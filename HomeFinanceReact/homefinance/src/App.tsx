// import logo from './logo.svg';
import './App.css';
import React from 'react';
import { store } from "./actions/store";
import { Provider } from "react-redux";
import Wallets from './components/Wallets';
import { Container } from "@mui/material";


const App=()=> {
  return (
    <Provider store={store}>
      <Container maxWidth="lg">
        <Wallets/>
      </Container>
    </Provider>
  );
}

export default App;
