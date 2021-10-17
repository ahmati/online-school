import { ADD_INVITEE, DELETE_INVITEE, GET_INVITEES, INVITEES_LOADING } from './types';
import { setError, setErrors } from './errorActions';
import axios from 'axios';
import { toast } from 'react-toastify';

export const getInvitees = (meetingId) => dispatch => {

    dispatch(setInviteesLoading(true));
    axios.get(`https://webexapis.com/v1/meetingInvitees?meetingId=${meetingId}`)
        .then(res => {
            let status = res.status;
            // Ok
            if(status === 200 || status === 204) {
                let invitees = res.data.items;
                dispatch({ type: GET_INVITEES, payload: invitees });
            }
            // Not ok
            else {
                let error = res.data;
                dispatch(setErrors(error));
            }
        })
        .catch(err => {
            console.log(err);
            dispatch(setError('An error occurred.'));
        })
        .finally(() => {
            dispatch(setInviteesLoading(false));
        });
}

export const setInviteesLoading = (isLoading) => dispatch => {
    dispatch({ type: INVITEES_LOADING, payload: isLoading });
}

export const addInvitee = (newInvitee) => dispatch => {

    axios.post('https://webexapis.com/v1/meetingInvitees', newInvitee)
        .then(res => {
            let status = res.status;
            // Ok
            if(status === 200 || status === 204) {
                let invitee = res.data;
                dispatch({ type: ADD_INVITEE, payload: invitee });
                toast.success(`${newInvitee.email} was successfully invited.`); // Internationalization
            }
            // Not ok
            else {
                let error = res.data;
                setErrors(error)
            }
        })
        .catch(err => {
            console.log(err);
            dispatch(setError('An error occurred.'));
        });
}

export const deleteInvitee = (inviteeId) => (dispatch, getState) => {
    let invitee = getState().inviteesReducer.current_invitees.find(i => i.id === inviteeId);

    axios.delete(`https://webexapis.com/v1/meetingInvitees/${inviteeId}`)
        .then(res => {
            let status = res.status;
            // Ok
            if(status === 200 || status === 204) {
                toast.info(`'${invitee.email}' was successfully removed from the invitees list.`); // Internationalization
                dispatch({ type: DELETE_INVITEE, payload: inviteeId });
            }
            // Not ok
            else {
                let error = res.data;
                setErrors(error);
            }
        })
        .catch(err => {
            console.log(err);
            dispatch(setError('An error occurred.'));
        });
}