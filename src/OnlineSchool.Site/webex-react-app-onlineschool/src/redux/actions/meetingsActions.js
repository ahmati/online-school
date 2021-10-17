import { GET_MEETINGS, MEETINGS_LOADING, DELETE_MEETING, GET_MEETING, MEETING_LOADING } from './types';
import { clearErrors, setError, setErrors } from './errorActions';
import axios from 'axios';
import { toast } from 'react-toastify';

import { getMinutesDiff, scrollToTop } from '../../utils/helpers';
import { join_before_host_minutes } from '../../utils/constants';

export const getUpcomingMeetings = () => dispatch => {
    
    dispatch(setMeetingsLoading(true));
    axios.get('https://webexapis.com/v1/meetings')
        .then(res => {
            let status = res.status;
            // Ok
            if(status === 200 || status === 204) {
                let meetings = res.data.items;
                let sortedMeetings = meetings.sort((a, b) => b.start - a.start);
                dispatch({ type: GET_MEETINGS, payload: sortedMeetings });
            }
            // Not ok
            else {
                dispatch(setErrors(res.data));
            }
        })
        .catch(err => {
            console.log(err);
            dispatch(setError('An error occurred.'));
        })
        .finally(() => {
            dispatch(setMeetingsLoading(false));
        })
}

export const setMeetingsLoading = (isLoading) => dispatch => {
    dispatch({ type: MEETINGS_LOADING, payload: isLoading });
}

export const getMeetingById = (meetingId) => dispatch => {

    dispatch(setMeetingLoading(true));
    axios.get(`https://webexapis.com/v1/meetings/${meetingId}`)
        .then(res => {
            let status = res.status;
            // Ok
            if(status === 200 || status === 204) {
                let meeting = res.data;
                dispatch({ type: GET_MEETING, payload: meeting });
            }
            // Not ok
            else {
                let error = res.data;
                dispatch(setErrors(error));
            }
        })
        .catch(err => {
            console.log(err);
            dispatch(setError('An error occurred while getting meeting information.'));
        })
        .finally(() => {
            dispatch(setMeetingLoading(false));
        })
}

export const setMeetingLoading = (isLoading) => dispatch => {
    dispatch({ type: MEETING_LOADING, payload: isLoading });
}

export const deleteMeeting = (meetingId) => (dispatch, getState) => {

    let meeting = getState().meetingsReducer.meetings.find(m => m.id === meetingId);
    axios.delete(`https://webexapis.com/v1/meetings/${meetingId}`)
        .then(res => {
            let status = res.status;
            // Ok
            if(status === 200 || status === 204) {
                dispatch({
                    type: DELETE_MEETING,
                    payload: meetingId
                });
                toast.info(`Meeting "${meeting.title}" was deleted successfully.`); // Internationalization
            }
            // Not ok
            else {
                dispatch(setErrors(res.data));
            }
        })
        .catch(err => {
            console.log(err);
            dispatch(setError('An error occurred.'));
        });
}

export const addMeeting = (newMeeting, history) => dispatch => {

    axios.post('https://webexapis.com/v1/meetings', newMeeting)
        .then(res => {
            let statusCode = res.status;
            // Response: Ok
            if(statusCode === 200 || statusCode === 204) {
                toast.success("Meeting created successfully."); // Internationalization
                history.push("/");
                dispatch(clearErrors());
            }
            // Response: Not ok
            else if(statusCode >= 400) {
                let error = res.data;
                scrollToTop();
                dispatch(setErrors(error));
            }
        })
        .catch(err => {
            console.log(err);
            scrollToTop();
            dispatch(setError(err.response.data.message));
        });
}

export const updateMeeting = (meetingId, updatedMeeting, history) => dispatch => {

    axios.put(`https://webexapis.com/v1/meetings/${meetingId}`, updatedMeeting)
    .then(res => {
        let statusCode = res.status;
        // Response: Ok
        if(statusCode === 200 || statusCode === 204) {
            toast.success('Meeting updated successfully.'); // Internationalization
            history.push("/");
        }
        // Response: Not ok
        else if(statusCode >= 400) {
            let error = res.data;
            setErrors(error);
        }
    })
    .catch(err => {
        console.log(err);
        dispatch(setError(err.response.data.message));
    });
}