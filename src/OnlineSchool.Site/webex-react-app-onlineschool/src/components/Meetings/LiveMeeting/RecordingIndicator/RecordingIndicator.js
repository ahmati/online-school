import React from 'react';
import PropTypes from 'prop-types';
import './index.css';
import { RecordingState, RecordingStates } from '../../../../utils/constants';
import { translateCap } from '../../../../i18n/translate';

const RecordingIndicator = ({ recordingState }) => {
    
    if (!recordingState || recordingState === RecordingState.Idle)
        return null;

    if(recordingState && recordingState === RecordingState.Recording)
        return (
            <div id="recording-indicator">
                <small>
                    <i className="fas fa-record-vinyl text-danger"> {translateCap('LiveMeeting.recording')} </i>
                </small> 
            </div>
        )

    if(recordingState && recordingState === RecordingState.Paused)
        return (
            <div id="recording-indicator">
                <small>
                    <i className="fas fa-record-vinyl text-warning"> {translateCap('LiveMeeting.recordingPaused')} </i>
                </small> 
            </div>
        )
}

RecordingIndicator.propTypes = {
    recordingState: PropTypes.oneOf(RecordingStates)
}

RecordingIndicator.defaultProps = {
    recordingState: RecordingState.Idle
}

export default RecordingIndicator;
