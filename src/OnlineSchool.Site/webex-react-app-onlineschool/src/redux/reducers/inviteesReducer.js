import { ADD_INVITEE, DELETE_INVITEE, GET_INVITEES, INVITEES_LOADING } from '../actions/types';

const initialState = {
    current_invitees: [],
    isInviteesLoading: false
}

const inviteesReducer = (state = initialState, action) => {
    
    switch(action.type) {

        case INVITEES_LOADING:
            return {
                ...state,
                isInviteesLoading: action.payload
            };

        case GET_INVITEES:
            return {
                ...state,
                current_invitees: action.payload
            };

        case ADD_INVITEE:
            return {
                ...state,
                current_invitees: [...state.current_invitees, action.payload]
            };

        case DELETE_INVITEE:
            return {
                ...state,
                current_invitees: [...state.current_invitees.filter(i => i.id !== action.payload)]
            };

        default:
            return state;
    }
}

export default inviteesReducer;