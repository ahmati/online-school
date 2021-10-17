import React, { Fragment } from 'react';
import flatten from 'flat';

import { IntlProvider } from 'react-intl';
import { LOCALES } from './locales';
import messages from './messages/index';

const Provider = ({ children, locale = LOCALES.ENGLISH }) => {

    return (
        <IntlProvider
            locale={locale}
            textComponent={Fragment}
            messages={flatten(messages[locale])}
        >
            {children}
        </IntlProvider>
    )
}

export default Provider;