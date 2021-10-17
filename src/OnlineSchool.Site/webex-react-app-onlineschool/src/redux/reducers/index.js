import { combineReducers } from 'redux';
import webexReducer from './webexReducer';
import errorsReducer from './errorsReducer';
import peopleReducer from './peopleReducer';
import meetingPreferencesReducer from './meetingPreferencesReducer';
import meetingsReducer from './meetingsReducer';
import inviteesReducer from './inviteesReducer';

export default combineReducers ({
    errorsReducer,
    webexReducer,
    peopleReducer,
    meetingPreferencesReducer,
    meetingsReducer,
    inviteesReducer
});