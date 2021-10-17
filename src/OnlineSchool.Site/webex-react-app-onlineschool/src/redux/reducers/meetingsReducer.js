import { GET_MEETINGS, MEETINGS_LOADING, DELETE_MEETING, GET_MEETING, MEETING_LOADING } from '../actions/types';

const initialState = {
    meetings: [],
    isMeetingsLoading: false,

    current_meeting: {},
    isMeetingLoading: false
}

const meetingsReducer = (state = initialState, action) => {

    switch(action.type) {

        case MEETINGS_LOADING:
            return {
                ...state,
                isMeetingsLoading: action.payload
            };

        case GET_MEETINGS:
            return {
                ...state,
                meetings: action.payload
            };

        case DELETE_MEETING:
            return {
                ...state,
                meetings: state.meetings.filter(m => m.id !== action.payload)
            };

        case MEETING_LOADING:
            return {
                ...state,
                isMeetingLoading: action.payload
            }

        case GET_MEETING:
            return {
                ...state,
                current_meeting: action.payload
            }

        default:
            return state;
    }
}

export default meetingsReducer;