import { GET_ERRORS, CLEAR_ERRORS } from '../actions/types';

const initialState = {
    errors: []
};

const errorsReducer = ( state = initialState, action ) => {
    
    switch(action.type) {

        case GET_ERRORS:
            return {
                ...state,
                errors: action.payload
            }

        case CLEAR_ERRORS:
            return {
                errors: []
            };

        default:
            return state;
            
    }
}

export default errorsReducer;