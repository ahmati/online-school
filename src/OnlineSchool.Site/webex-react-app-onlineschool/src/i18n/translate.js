import { createIntl, createIntlCache } from 'react-intl';
import flatten from 'flat';
import { capitalize } from '../utils/helpers';
import messages from './messages';

let cache;
let intl;

export const configureIntl = (locale) => {
    cache = createIntlCache();
    intl = createIntl(
        {
            locale: locale,
            messages: flatten(messages[locale])
        },
        cache
    );
}

export const changeIntlLanguage = (locale) => {
    intl = createIntl(
        {
            locale,
            messages: flatten(messages[locale])
        },
        cache
    );
}

export const translate = (id, value={}) => intl.formatMessage({ id }, value);

/*** Capitalized translation */
export const translateCap = (id, value={}) => capitalize(intl.formatMessage({ id }, value));