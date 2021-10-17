import { GET_MEETING_PREFERENCES, MEETING_PREFERENCES_LOADING } from '../actions/types';

const initialState = {
    audio: {},
    video: {},
    schedulingOptions: {},
    sites: [],
    personalRoom: {},
    isMeetingPreferencesLoading: false
}

const meetingPreferencesReducer = (state = initialState, action) => {

    switch(action.type) {

        case MEETING_PREFERENCES_LOADING:
            return {
                ...state,
                isMeetingPreferencesLoading: action.payload
            }

        case GET_MEETING_PREFERENCES:
            let { audio, video, schedulingOptions, sites, personalMeetingRoom } = action.payload;
            return {
                ...state,
                isMeetingPreferencesLoading: false,
                audio,
                video,
                schedulingOptions,
                sites,
                personalRoom: personalMeetingRoom
            }

        default:
            return state;
    }
}

export default meetingPreferencesReducer;