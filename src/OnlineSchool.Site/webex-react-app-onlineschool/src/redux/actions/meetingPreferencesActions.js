import { GET_MEETING_PREFERENCES, MEETING_PREFERENCES_LOADING } from './types';
import { setError, setErrors } from './errorActions';
import axios from 'axios';

export const getMeetingPreferences = () => dispatch => {
    
    dispatch(setMeetingPreferencesLoading(true));
    axios.get(`https://webexapis.com/v1/meetingPreferences`)
        .then(res => {
            let status = res.status;
            // Ok
            if(status === 200 || status === 204) {
                let meetingPreferences = res.data;
                dispatch({ 
                    type: GET_MEETING_PREFERENCES, 
                    payload: meetingPreferences 
                });
            }
            // Not ok
            else {
                let errors = res.data;
                setErrors(errors);
            }
            
        })
        .catch(err => {
            console.log(err);
            dispatch(setError('An error occurred.'));
        })
        .finally(() => {
            dispatch(setMeetingPreferencesLoading(false));
        });
}

export const setMeetingPreferencesLoading = (isLoading) => dispatch => {
    dispatch({ type: MEETING_PREFERENCES_LOADING, payload: isLoading });
}