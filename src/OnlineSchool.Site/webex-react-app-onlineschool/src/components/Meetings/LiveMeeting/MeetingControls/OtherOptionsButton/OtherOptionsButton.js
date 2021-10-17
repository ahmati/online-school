import React, { Component } from 'react';
import PropTypes from 'prop-types';
import './index.css';
import { Dropdown } from 'react-bootstrap';
import { LayoutType, QualityLevel, RecordingState } from '../../../../../utils/constants';
import { translate, translateCap } from '../../../../../i18n/translate';

class OtherOptionsButton extends Component {

    constructor() {
        super()
    }

    renderLocalVideoVisibilitySubmenu = () => (
        <>
            <Dropdown.Header>
                {translateCap('LiveMeeting.OtherOptionsButton.layout')}
            </Dropdown.Header>
            <Dropdown.Item onClick={this.props.onToggleLocalVideoVisibility} >
                {
                    this.props.isLocalVideoHidden ?
                        <>
                            <i className="far fa-eye"></i> {translateCap('LiveMeeting.OtherOptionsButton.displayMyVideo')}
                        </> :
                        <>
                        <i className="far fa-eye-slash"></i> {translateCap('LiveMeeting.OtherOptionsButton.hideMyVideo')}
                    </>
                }
            </Dropdown.Item>
        </>
    )

    renderRecordingSubmenu = () => {
        let { isModerator, recordingState } = this.props;

        if(!isModerator)
            return null;

        else {
            switch(recordingState) {
                case RecordingState.Idle:
                    return (
                        <>
                            <Dropdown.Header>
                                {translateCap('LiveMeeting.recording')}
                            </Dropdown.Header>
                            <Dropdown.Item onClick={this.props.onStartRecording}>
                                <i className="fas fa-play"></i> {translateCap('LiveMeeting.OtherOptionsButton.startRecording')}
                            </Dropdown.Item>
                        </>
                    );
                case RecordingState.Paused:
                    return (
                        <>
                            <Dropdown.Header>
                                {translateCap('LiveMeeting.recording')}
                            </Dropdown.Header>
                            <Dropdown.Item onClick={this.props.onResumeRecording}>
                                <i className="fas fa-play"></i> {translateCap('LiveMeeting.OtherOptionsButton.resumeRecording')}
                            </Dropdown.Item>
                            <Dropdown.Item onClick={this.props.onStopRecording}>
                                <i className="fas fa-stop"></i> {translateCap('LiveMeeting.OtherOptionsButton.stopRecording')}
                            </Dropdown.Item>
                        </>
                    );
                case RecordingState.Recording:
                    return (
                        <>
                            <Dropdown.Header>
                                {translateCap('LiveMeeting.recording')}
                            </Dropdown.Header>
                            <Dropdown.Item onClick={this.props.onPauseRecording}>
                                <i className="fas fa-pause"></i> {translateCap('LiveMeeting.OtherOptionsButton.pauseRecording')}
                            </Dropdown.Item>
                            <Dropdown.Item onClick={this.props.onStopRecording}>
                                <i className="fas fa-stop"></i> {translateCap('LiveMeeting.OtherOptionsButton.stopRecording')}
                            </Dropdown.Item>
                        </>
                    );
                default:
                    return null;
            }
        }
    }

    renderHandRaiseSubmenu = () => {
        let { isModerator, isRoomReady, isHandRaised, onToggleHand } = this.props;

        return (!isModerator && isRoomReady) ?
            <>
                <Dropdown.Item onClick={() => onToggleHand()}>
                    {
                        isHandRaised ?
                            <> <i className="far fa-hand-point-down"></i> {translateCap('LiveMeeting.lowerHand')} </> :
                            <> <i className="far fa-hand-paper"></i> {translateCap('LiveMeeting.raiseHand')} </>
                    }
                </Dropdown.Item>
            </> : null
    }

    renderLocalVideoQualitySubmenu = () => (
        <>
            <Dropdown.Header>
                    {translateCap('LiveMeeting.OtherOptionsButton.videoQuality')}
                </Dropdown.Header>
                <Dropdown.Item
                        active={this.props.localVideoQuality === QualityLevel.High}
                        onClick={() => this.props.onChangeLocalVideoQuality(QualityLevel.High)} >
                    {translateCap('LiveMeeting.OtherOptionsButton.high')}
                </Dropdown.Item>
                <Dropdown.Item
                        active={this.props.localVideoQuality === QualityLevel.Medium}
                        onClick={() => this.props.onChangeLocalVideoQuality(QualityLevel.Medium)} >
                    {translateCap('LiveMeeting.OtherOptionsButton.medium')}
                </Dropdown.Item>
                <Dropdown.Item
                        active={this.props.localVideoQuality === QualityLevel.Low}
                        onClick={() => this.props.onChangeLocalVideoQuality(QualityLevel.Low)} >
                    {translateCap('LiveMeeting.OtherOptionsButton.low')}
                </Dropdown.Item>
        </>
    )

    renderVideoLayoutSubmenu = () => {
        let { layoutType, onChangeVideoLayout } = this.props;

        return (
            <>
                <Dropdown.Header>
                    {translateCap('LiveMeeting.OtherOptionsButton.videoLayout')}
                </Dropdown.Header>
                <Dropdown.Item 
                        active={layoutType === LayoutType.Single}
                        onClick={() => onChangeVideoLayout(LayoutType.Single)} >
                    {translateCap('LiveMeeting.OtherOptionsButton.single')}
                </Dropdown.Item>
                <Dropdown.Item
                        active={layoutType === LayoutType.Equal}
                        onClick={() => onChangeVideoLayout(LayoutType.Equal)} >
                    {translateCap('LiveMeeting.OtherOptionsButton.equal')}
                </Dropdown.Item>
                <Dropdown.Item
                        active={layoutType === LayoutType.ActivePresence}
                        onClick={() => onChangeVideoLayout(LayoutType.ActivePresence)} >
                    {translateCap('LiveMeeting.OtherOptionsButton.activePresence')}
                </Dropdown.Item>
                <Dropdown.Item
                        active={layoutType === LayoutType.Prominent}
                        onClick={() => onChangeVideoLayout(LayoutType.Prominent)} >
                    {translateCap('LiveMeeting.OtherOptionsButton.prominent')}
                </Dropdown.Item>
                <Dropdown.Item
                        active={layoutType === LayoutType.OnePlusN}
                        onClick={() => onChangeVideoLayout(LayoutType.OnePlusN)} >
                    {LayoutType.OnePlusN} {/* No translation */}
                </Dropdown.Item>
            </>
        )
    }

    renderIncomingVideoQualitySubmenu = () => (
        <>
            <Dropdown.Header>
                    {translateCap('LiveMeeting.OtherOptionsButton.remoteVideoQuality')}
                </Dropdown.Header>
                <Dropdown.Item 
                        active={this.props.remoteVideoQuality === QualityLevel.High}
                        onClick={() => this.props.onChangeRemoteVideoQuality(QualityLevel.High)} >
                    {translateCap('LiveMeeting.OtherOptionsButton.high')}
                </Dropdown.Item>
                <Dropdown.Item 
                        active={this.props.remoteVideoQuality === QualityLevel.Medium}
                        onClick={() => this.props.onChangeRemoteVideoQuality(QualityLevel.Medium)} >
                    {translateCap('LiveMeeting.OtherOptionsButton.medium')}
                </Dropdown.Item>
                <Dropdown.Item 
                        active={this.props.remoteVideoQuality === QualityLevel.Low}
                        onClick={() => this.props.onChangeRemoteVideoQuality(QualityLevel.Low)} >
                    {translateCap('LiveMeeting.OtherOptionsButton.low')}
            </Dropdown.Item>
        </>
    )

    render() {
        let { direction } = this.props

        return (
            <Dropdown drop={direction} id="moreOptions" className="d-inline-block">
                <Dropdown.Toggle variant="light" className="meeting-control rounded-circle shadow mx-1 px-3" title={translateCap('LiveMeeting.OtherOptionsButton.moreOptions')}>
                    <i className="fas fa-ellipsis-v"></i>
                </Dropdown.Toggle>
                <Dropdown.Menu>
                    {this.renderLocalVideoVisibilitySubmenu()}
                    {/* {this.renderRecordingSubmenu()} */}
                    {/* {this.renderHandRaiseSubmenu()} */}
                    {this.renderLocalVideoQualitySubmenu()}
                    {/* {this.renderIncomingVideoQualitySubmenu()} */}
                    {this.renderVideoLayoutSubmenu()}
                </Dropdown.Menu>
            </Dropdown>
        );
    }
}

OtherOptionsButton.propTypes = {
    direction: PropTypes.string.isRequired,
    isModerator: PropTypes.bool.isRequired,
    isRoomReady: PropTypes.bool.isRequired,

    isLocalVideoHidden: PropTypes.bool.isRequired,
    onToggleLocalVideoVisibility: PropTypes.func.isRequired,

    isHandRaised: PropTypes.bool.isRequired,
    // onToggleHand: PropTypes.func.isRequired,

    recordingState: PropTypes.string.isRequired,
    onStartRecording: PropTypes.func.isRequired,
    onPauseRecording: PropTypes.func.isRequired,
    onResumeRecording: PropTypes.func.isRequired,
    onStopRecording: PropTypes.func.isRequired,

    localVideoQuality: PropTypes.string.isRequired,
    remoteVideoQuality: PropTypes.string.isRequired,
    onChangeLocalVideoQuality: PropTypes.func.isRequired,
    onChangeRemoteVideoQuality: PropTypes.func.isRequired,

    layoutType: PropTypes.string,
    onChangeVideoLayout: PropTypes.func.isRequired
}

OtherOptionsButton.defaultProps = {
    direction: 'up',
    isModerator: false,
    isRoomReady: false,

    isLocalVideoHidden: false,
    onToggleLocalVideoVisibility: () => console.log('onToggleLocalVideoVisibility'),

    isHandRaised: false,
    // onToggleHand: () => console.log('onToggleHand'),

    recordingState: RecordingState.idle,
    onStartRecording: () => console.log('onStartRecording'),
    onPauseRecording: () => console.log('onPauseRecording'),
    onResumeRecording: () => console.log('onResumeRecording'),
    onStopRecording: () => console.log('onStopRecording'),

    localVideoQuality: QualityLevel.Low,
    remoteVideoQuality: QualityLevel.Low,
    onChangeLocalVideoQuality: () => console.log('onChangeLocalVideoQuality'),
    onChangeRemoteVideoQuality: () => console.log('onChangeRemoteVideoQuality'),

    layoutType: undefined,
    onChangeVideoLayout: () => console.log('onChangeVideoLayout')
}

export default OtherOptionsButton;