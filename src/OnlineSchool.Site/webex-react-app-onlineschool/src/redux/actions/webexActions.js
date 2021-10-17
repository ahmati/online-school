import { SET_WEBEX_INSTANCE } from './types';
import jwtBuilder from 'jwt-builder';

import { getToken } from './../../utils/helpers';

export const initialize = () => {

    let jwt = getToken();
    localStorage.setItem('webex-token', jwt);

    
}