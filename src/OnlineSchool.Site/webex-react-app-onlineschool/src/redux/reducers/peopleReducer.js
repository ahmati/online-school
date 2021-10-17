import { GET_PERSONAL_DETAILS, PERSONAL_DETAILS_LOADING } from '../actions/types';

const initialState = {
    personalDetails: {},
    isPersonalDetailsLoading: false
}

const peopleReducer = (state = initialState, action) => {

    switch(action.type) {

        case PERSONAL_DETAILS_LOADING:
            return {
                ...state,
                isPersonalDetailsLoading: action.payload
            }

        case GET_PERSONAL_DETAILS:
            return {
                ...state,
                personalDetails: action.payload
            }

        default:
            return state;
    }
}

export default peopleReducer;