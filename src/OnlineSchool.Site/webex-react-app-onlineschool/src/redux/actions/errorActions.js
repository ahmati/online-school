import { CLEAR_ERRORS, GET_ERRORS } from './types';

export const clearErrors = () => dispatch => {
    dispatch({ type: CLEAR_ERRORS });
};

export const setError = (error) => dispatch => {
    dispatch({
        type: GET_ERRORS,
        payload: [error]
    })
}

export const setErrors = (errors) => dispatch => {
    dispatch({
        type: GET_ERRORS,
        payload: errors
    })
}