// import logo from './logo.svg';
import './App.css';
import React from 'react';
import { store } from "./actions/store";
import { Provider } from "react-redux";
import Wallets from './components/Wallets';
import { Container } from "@mui/material";
import toast, { Toaster } from 'react-hot-toast';

import { BrowserRouter, Routes, Route } from "react-router-dom";
import OperationForm from './components/Operations/OperationForm';
import Layout from './Layout';

export const notify = (s: string) => toast(s);

const App = () => {
  return (
    <Provider store={store}>
      <Container maxWidth="lg">
        <BrowserRouter>
          <Routes>
            <Route path="/" element={<Layout />}>
              <Route index element={<Wallets />} />
              <Route path="newOperation" element={<OperationForm />} />
              {/* <Route path="*" element={<NoPage />} /> */}
            </Route>
          </Routes>
        </BrowserRouter>
        <Toaster />
      </Container>
    </Provider>


  );
}

export default App;
