import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';

import { getPersonalDetails } from '../../redux/actions/peopleActions';
import { getMeetingPreferences } from '../../redux/actions/meetingPreferencesActions';

import BeatLoader from "react-spinners/BeatLoader";
import {CopyToClipboard} from 'react-copy-to-clipboard';

class PersonalInfo extends Component {

    constructor() {
        super();
        this.state = {
            copied: false
        }
    }

    componentDidMount() {
        this.props.getPersonalDetails();
        this.props.getMeetingPreferences();
    }
    
    copiedToClipboard() {
        this.setState({ copied: true });
        setTimeout(
            () => this.setState({ copied: false }), 
            3000
          )
    }

    render() {
        let { copied } = this.state;
        let { personalDetails, isPersonalDetailsLoading, personalRoom, isPersonalRoomLoading } = this.props;
        return (
            <div className="row p-2 border-bottom shadow">
                <div className="col-12">
                    {
                        isPersonalDetailsLoading === true ?
                            <BeatLoader color="#00a0d1" loading={isPersonalDetailsLoading} size={10} />
                            :
                            <>
                                <span className="dot bg-success"></span> <b> {personalDetails.displayName} </b>
                                <small className="d-block">
                                    {
                                        isPersonalRoomLoading === true ?
                                            <BeatLoader color="#00a0d1" loading={isPersonalRoomLoading} size={10} />
                                            :
                                            <>
                                                <i className="mx-2">{personalRoom.personalMeetingRoomLink}</i>
                                                <CopyToClipboard text={personalRoom.personalMeetingRoomLink} onCopy={() => this.copiedToClipboard()}>
                                                    <>
                                                        <i className="far fa-copy cursor-pointer" title="Copy"></i> {/* Internationalization */}
                                                    </>
                                                </CopyToClipboard>
                                            </>
                                    }
                                    {
                                        copied ?
                                            <>
                                                <small className="text-muted"> (Copied) </small> {/* Internationalization */}
                                            </>
                                            : null
                                    }
                                </small>
                            </>
                    }
                </div>
            </div>
        )
    }
}

PersonalInfo.propTypes = {
    personalDetails: PropTypes.object.isRequired,
    isPersonalDetailsLoading: PropTypes.bool.isRequired,
    personalRoom: PropTypes.object.isRequired,
    isMeetingPreferencesLoading: PropTypes.bool.isRequired,
    errors: PropTypes.array
}

const mapStateToProps = state => ({
    personalDetails: state.peopleReducer.personalDetails,
    isPersonalDetailsLoading: state.peopleReducer.isPersonalDetailsLoading,
    personalRoom: state.meetingPreferencesReducer.personalRoom,
    isMeetingPreferencesLoading: state.meetingPreferencesReducer.isMeetingPreferencesLoading,
    errors: state.errorsReducer.errors
});

export default connect(mapStateToProps, { getPersonalDetails, getMeetingPreferences })(PersonalInfo);
