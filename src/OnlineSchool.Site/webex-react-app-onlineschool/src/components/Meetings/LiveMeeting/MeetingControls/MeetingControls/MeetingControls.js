import React, { Component } from 'react';
import PropTypes from 'prop-types';
import './index.css';

import { translateCap } from '../../../../../i18n/translate';
import { MaxNumberOfInterventions, QualityLevel, RecordingState } from '../../../../../utils/constants';
import OtherOptionsButton from '../OtherOptionsButton/OtherOptionsButton';
import { Dropdown } from 'react-bootstrap';

class MeetingControls extends Component {

    constructor() {
        super()
    }

    renderAudioControl = () => {
        let { self } = this.props;

        return (self.disallowUnmute && self.isAudioMuted) 
                ?
                <button disabled id="toggleAudioBtn" className="meeting-control btn btn-md btn-light rounded-pill shadow mx-1" title={translateCap('LiveMeeting.MeetingControls.unmuteDisallowed')}>
                    <i className='fas fa-lock'></i> 
                    <span> {translateCap('LiveMeeting.MeetingControls.unmuteAudio')}</span>
                </button>
                :
                <button id="toggleAudioBtn" className="meeting-control btn btn-md btn-light rounded-pill shadow mx-1" title={translateCap('LiveMeeting.MeetingControls.toggleAudio')} onClick={this.props.onToggleAudio}>
                    <i className={`fas fa-microphone${self.isAudioMuted ? '-slash' : ' my-text-primary'}`}></i> 
                    <span> {self.isAudioMuted ? translateCap('LiveMeeting.MeetingControls.unmuteAudio') : translateCap('LiveMeeting.MeetingControls.muteAudio')} </span>
                </button>
    }

    renderRaiseHand = () => {
        let { self, isRoomReady, isHandRaised, handRaisesCount } = this.props;

        if (!self.isModerator && isRoomReady)
            return (!isHandRaised && handRaisesCount >= MaxNumberOfInterventions) ?
                <button disabled id="toggleHandBtn" className="meeting-control btn btn-md btn-light rounded-pill shadow mx-1" title={translateCap('notifications.maxHandRaiseReached')} >
                    <i className="far fa-hand-paper"></i>
                    <span> {translateCap('LiveMeeting.raiseHand')} </span>
                </button>
                :
                <button id="toggleHandBtn" className="meeting-control btn btn-md btn-light rounded-pill shadow mx-1" title={translateCap('LiveMeeting.MeetingControls.intervention')} onClick={async () => await this.props.onToggleHand()}>
                    { isHandRaised ? <i className="far fa-hand-point-down"></i> : <i className="far fa-hand-paper"></i> }
                    <span> {isHandRaised ? translateCap('LiveMeeting.lowerHand') : translateCap('LiveMeeting.raiseHand')} </span>
                </button> 
        return;
    }
    
    render() {
        let { self, isRoomReady, isLocalVideoHidden, isLocalSharing, recordingState, isHandRaised, localVideoQuality, remoteVideoQuality, layoutType } = this.props;
        let { onToggleLocalVideoVisibility, onQuitMeeting, onToggleAudio, onToggleVideo, onToggleShare, onStartRecording, onPauseRecording, onResumeRecording, onStopRecording, onToggleHand, onChangeLocalVideoQuality, onChangeRemoteVideoQuality, onChangeVideoLayout, onToggleParticipantsDrawer, onToggleChatDrawer } = this.props

        if(!self)
            return null;
        
        return (
            <div id="meeting-controls" className="row m-0 p-0">
                <div className="col-12 m-0 p-0">

                    <div className="absolute-left left-controls">
                        {this.renderAudioControl()}
                        <button id="toggleVideoBtn" className="meeting-control btn btn-md btn-light rounded-pill shadow mx-1" title={translateCap('LiveMeeting.MeetingControls.toggleVideo')} onClick={onToggleVideo}>
                            <i className={`fas fa-video${self.isVideoMuted ? '-slash' : ' my-text-primary'}`}></i> 
                            <span> {self.isVideoMuted ? translateCap('LiveMeeting.MeetingControls.unmuteVideo') : translateCap('LiveMeeting.MeetingControls.muteVideo')} </span>
                        </button>
                        <button id="toggleShareScreenBtn" className="meeting-control btn btn-md btn-light rounded-pill shadow mx-1" title={translateCap('LiveMeeting.MeetingControls.toggleScreenShare')} onClick={async () => await onToggleShare()}>
                            <i className={`fas fa-desktop ${isLocalSharing ? ' my-text-primary' : ''}`}></i> 
                            <span> {isLocalSharing ? translateCap('LiveMeeting.MeetingControls.stopShare') : translateCap('LiveMeeting.MeetingControls.shareScreen')} </span>
                        </button>
                        {this.renderRaiseHand()}
                        <OtherOptionsButton
                            isModerator={self.isModerator}
                            isRoomReady={isRoomReady}
                            isLocalVideoHidden={isLocalVideoHidden}
                            onToggleLocalVideoVisibility={onToggleLocalVideoVisibility}
                            isHandRaised={isHandRaised}
                            // onToggleHand={onToggleHand}
                            recordingState={recordingState}
                            onStartRecording={onStartRecording} 
                            onPauseRecording={onPauseRecording} 
                            onResumeRecording={onResumeRecording} 
                            onStopRecording={onStopRecording}
                            localVideoQuality={localVideoQuality}
                            remoteVideoQuality={remoteVideoQuality}
                            onChangeLocalVideoQuality={onChangeLocalVideoQuality}
                            onChangeRemoteVideoQuality={onChangeRemoteVideoQuality}
                            layoutType={layoutType}
                            onChangeVideoLayout={onChangeVideoLayout}
                        />
                        <button 
                            id="endMeetingBtn" 
                            className="meeting-control btn btn-md btn-danger rounded-circle shadow mx-1" 
                            title={self.isHost ? translateCap('LiveMeeting.MeetingControls.endMeeting') : translateCap('LiveMeeting.MeetingControls.leaveMeeting')} 
                            onClick={async () => { await onQuitMeeting() }}>
                            <i className="fas fa-times"></i>
                        </button>
                    </div>

                    {/* COMMENTED: Meeting is left when leaving tab/window. */}
                    <div className="absolute-center center-controls">
                        <button 
                            id="endMeetingBtn" 
                            className="btn btn-md btn-danger rounded-circle shadow mx-1" 
                            title={self.isHost ? translateCap('LiveMeeting.MeetingControls.endMeeting') : translateCap('LiveMeeting.MeetingControls.leaveMeeting')} 
                            onClick={async () => { await onQuitMeeting() }}>
                            <i className="fas fa-times"></i>
                        </button>
                    </div>

                    <div className="absolute-right right-controls mr-2">
                        <div className="btn-group shadow" role="group" aria-label="Participants | Chat">
                            <button id="toggleParticipantsBtn" className="meeting-control btn btn-md btn-light" title={translateCap('LiveMeeting.participants')} onClick={onToggleParticipantsDrawer}>
                                <i className="fas fa-users"></i>
                                <span> {translateCap('LiveMeeting.participants')} </span>
                            </button>
                            <button id="toggleChatBtn" className="meeting-control btn btn-md btn-light border-left" title={translateCap('LiveMeeting.chat')} onClick={onToggleChatDrawer}>
                                <i className="fas fa-comments"></i>
                                <span> {translateCap('LiveMeeting.chat')} </span>
                            </button>
                        </div>
                    </div>

                </div>
            </div>
        );
    }
}

MeetingControls.propTypes = {
    self: PropTypes.object.isRequired,
    isRoomReady: PropTypes.bool.isRequired,
    isLocalVideoHidden: PropTypes.bool.isRequired,
    onToggleLocalVideoVisibility: PropTypes.func.isRequired,
    onQuitMeeting: PropTypes.func.isRequired,
    
    onToggleAudio: PropTypes.func.isRequired, 
    onToggleVideo: PropTypes.func.isRequired, 
    onToggleShareScreen: PropTypes.func.isRequired,
    isLocalSharing: PropTypes.bool.isRequired,

    recordingState: PropTypes.string.isRequired,
    onStartRecording: PropTypes.func.isRequired, 
    onPauseRecording: PropTypes.func.isRequired, 
    onResumeRecording: PropTypes.func.isRequired, 
    onStopRecording: PropTypes.func.isRequired,

    isHandRaised: PropTypes.bool.isRequired,
    handRaisesCount: PropTypes.number.isRequired,
    onToggleHand: PropTypes.func.isRequired,

    localVideoQuality: PropTypes.string.isRequired,
    remoteVideoQuality: PropTypes.string.isRequired,
    onChangeLocalVideoQuality: PropTypes.func.isRequired,
    onChangeRemoteVideoQuality: PropTypes.func.isRequired,

    layoutType: PropTypes.string,
    onChangeVideoLayout: PropTypes.func.isRequired,

    onToggleParticipantsDrawer: PropTypes.func.isRequired, 
    onToggleChatDrawer: PropTypes.func.isRequired
}

MeetingControls.defaultProps = {
    self: null,
    isRoomReady: false,
    isLocalVideoHidden: false,
    onToggleLocalVideoVisibility: () => console.log('onToggleLocalVideoVisibility'),
    onQuitMeeting: () => console.log('onQuitMeeting'),

    onToggleAudio: () => console.log('onToggleAudio'),
    onToggleVideo: () => console.log('onToggleVideo'),
    onToggleShareScreen: () => console.log('onToggleShareScreen'),
    isLocalSharing: false,

    recordingState: RecordingState.Idle,
    onStartRecording: () => console.log('onStartRecording'),
    onPauseRecording: () => console.log('onPauseRecording'),
    onResumeRecording: () => console.log('onResumeRecording'),
    onStopRecording: () => console.log('onStopRecording'),

    isHandRaised: false,
    handRaisesCount: 0,
    onToggleHand: () => console.log('onToggleHand'),

    localVideoQuality: QualityLevel.Low,
    remoteVideoQuality: QualityLevel.Low,
    onChangeLocalVideoQuality: () => console.log('onChangeLocalVideoQuality'),
    onChangeRemoteVideoQuality: () => console.log('onChangeRemoteVideoQuality'),

    layoutType: undefined,
    onChangeVideoLayout: () => console.log('onChangeVideoLayout'),

    onToggleParticipantsDrawer: () => console.log('onToggleParticipantsDrawer'),
    onToggleChatDrawer: () => console.log('onToggleChatDrawer')
}

export default MeetingControls;