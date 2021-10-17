import React, { Component } from "react";

import {I18nProvider, LOCALES } from './i18n';
import { ToastContainer } from 'react-toastify';

import Header from "./components/Layout/Header/Header";
import Routes from './Router/Routes';
import configureHeaders from './redux/securityUtils/configureHeaders';
import { getToken } from "./utils/webexConfig";
import { LOCALES_ARRAY } from "./i18n/locales";
import { LocalStorageKeys } from "./utils/constants";
import { changeIntlLanguage, configureIntl } from "./i18n/translate";

class App extends Component {

  constructor() {
    super();

    configureHeaders(getToken());
    configureIntl(LOCALES.ITALIAN);

    this.state = {
      locale: LOCALES.ITALIAN
    }
  }
  
  componentDidMount() {
    // During resizing, calculates window height correctly
    window.addEventListener('resize', () => this.calculateWindowHeight());

    let locale = localStorage.getItem(LocalStorageKeys.Language);
    if(locale && LOCALES_ARRAY.includes(locale))
      this.changeLanguage(locale);
  }

  componentWillUnmount() {
    window.removeEventListener('resize', null)
  }

  calculateWindowHeight = () => {
    // SOURCE: https://css-tricks.com/the-trick-to-viewport-units-on-mobile/
    // First we get the viewport height and we multiple it by 1% to get a value for a vh unit
    let vh = window.innerHeight * 0.01;
    // Then we set the value in the --vh custom property to the root of the document
    document.documentElement.style.setProperty('--vh', `${vh}px`);
  }

  changeLanguage = (locale) => {
    if(locale && LOCALES_ARRAY.includes(locale)) {
      localStorage.setItem(LocalStorageKeys.Language, locale);
      changeIntlLanguage(locale);
      this.setState({ locale });
    }
  }

  render() {
    const { locale } = this.state; 
    return (
      <>

        <div className="main-container">
          <Header locale={locale} changeLanguage={this.changeLanguage} />
          <Routes locale={locale} />
        </div>

        <ToastContainer
          position="bottom-right"
          autoClose={4000}
          hideProgressBar
          newestOnTop={false}
          closeOnClick
          rtl={false}
          pauseOnFocusLoss={false}
          draggable
          pauseOnHover={false}
        />
      </>
    );
  }
}

export default App;
