import { SET_WEBEX_INSTANCE } from './../actions/types';

const initialState = {
    webex: {}
};

const webexReducer = (state = initialState, action) => {

    switch(action.type) {
        
        case SET_WEBEX_INSTANCE:
            return {
                ...state,
                webex: action.payload
            }

        default:
            return state;  
    }
}

export default webexReducer;