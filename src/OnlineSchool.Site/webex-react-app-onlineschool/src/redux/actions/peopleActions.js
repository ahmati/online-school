import { GET_PERSONAL_DETAILS, PERSONAL_DETAILS_LOADING } from './types';
import { setError, setErrors } from './errorActions';
import axios from 'axios';

export const getPersonalDetails = () => dispatch => {

    dispatch(setPersonalDetailsLoading(true));
    axios.get(`https://webexapis.com/v1/people/me`)
        .then(res => {
            let status = res.status;
            // Ok
            if(status === 200 || status === 204) {
                let personalDetails = res.data
                dispatch({
                    type: GET_PERSONAL_DETAILS,
                    payload: personalDetails
                });
            }
            // Not ok
            else {
                let errors = res.data;
                dispatch(setErrors(errors));
            }
            
        })
        .catch(err => {
            console.log(err);
            dispatch(setError('An error occurred.'));
        })
        .finally(() => {
            dispatch(setPersonalDetailsLoading(false));
        });
}

export const setPersonalDetailsLoading = (isLoading) => dispatch => {
    dispatch({ type: PERSONAL_DETAILS_LOADING, payload: isLoading });
}