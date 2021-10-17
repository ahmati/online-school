import React from 'react';
import PropTypes from 'prop-types';
import { translateCap } from '../../../../i18n/translate';

const Participant = ({ self, participant: p, onLowerHand, onToggleAudio, onRemove, onMakeHost }) => {

    const renderHand = () => {
        if(!p.handRaised)
            return null;

        if(p.isSelf || self.isModerator)
            return (
                <button className="btn btn-sm btn-default rounded-circle" onClick={() => onLowerHand(p.id)} title={translateCap('LiveMeeting.lowerHand')}>
                    <i className="far fa-hand-paper"></i>
                </button>
            )
        else
        return (
            <button disabled className="btn btn-sm btn-default rounded-circle" title={translateCap('LiveMeeting.raiseHand')}>
                <i className="far fa-hand-paper"></i>
            </button>
        )
    }

    if(!p)
        return null;
    else
        return (
            <li className="list-group-item">
                {renderHand()}
                <span className="pl-1"> {p.name} </span>
                {p.isHost ? <small className="text-muted"> ({translateCap('LiveMeeting.Participant.host')}) </small> : null} {p.isSelf ? <small className="text-muted"> ({translateCap('LiveMeeting.Participant.me')}) </small> : null}
                {
                    (self.isModerator && !p.isSelf && !p.isModerator) ?
                        <>
                            {/* <div className="d-inline-block float-right">
                                <Dropdown className="p-0">
                                    <Dropdown.Toggle variant="default" id="participant-menu" className="px-1 py-1 ml-1">
                                        <i className="fas fa-ellipsis-h"></i>
                                    </Dropdown.Toggle>
                                    <Dropdown.Menu>
                                        <Dropdown.Item onClick={() => onMakeHost(p.id)}> Make moderator </Dropdown.Item>
                                    </Dropdown.Menu>
                                </Dropdown>
                            </div> */}
                            <button className="btn btn-sm float-right text-danger" title={translateCap('LiveMeeting.Participant.expel')} onClick={() => onRemove(p.id)}>
                                <i className="fas fa-times"></i>
                            </button>
                        </> :
                        null
                }
                {
                    (p.isSelf || self.isModerator) ?
                        <button className="btn btn-sm text-success float-right participant-audio" title={`${p.isAudioMuted ? translateCap('LiveMeeting.unmute') : translateCap('LiveMeeting.mute')}`} onClick={() => onToggleAudio(p.id, !p.isAudioMuted)}>
                            <i className={`fas fa-microphone${p.isAudioMuted ? '-slash' : ''}`}></i>
                        </button> 
                        : null
                }
            </li>
        );
}

Participant.propTypes = {
    self: PropTypes.object.isRequired,
    participant: PropTypes.object.isRequired,
    onLowerHand: PropTypes.func.isRequired,
    onToggleAudio: PropTypes.func.isRequired,
    onRemove: PropTypes.func.isRequired,
    onMakeHost: PropTypes.func.isRequired
}

Participant.defaultProps = {
    self: { isHost: false, isModerator: false },
    participant: null,
    onLowerHand: () => console.log('onLowerHand'),
    onToggleAudio: () => console.log('onToggleAudio'),
    onRemove: () => console.log('onRemove'),
    onMakeHost: () => console.log('onMakeHost'),
}

export default Participant;