import React, { Component } from 'react';
import PropTypes from 'prop-types';
import './index.css';

import { ParticipantStatus } from '../../../../utils/constants';
import { translateCap } from '../../../../i18n/translate';

import SlideDrawer from '../../../Layout/Shared/SlideDrawer/SlideDrawer';
import { Dropdown } from 'react-bootstrap';
import ParticipantsSection from './ParticipantsSection';
import { sortObjArrayByDateProp_ASC } from '../../../../utils/helpers';

class Participants extends Component {

    constructor() {
        super()
        this.state = {
            isHandRaisedOpen: true,
            isHostsOpen: true,
            isModsOpen: true,
            isGuestsOpen: true
        }
    }

    /**
     * Based on the 'handRaises' provided, adds to each participant (only the ones with hand raised) 
     * the following property: 'handRaised', which is a string timestamp
     */
    prepareHandRaised = () => {
        let { participants, handRaises } = this.props;

        let inMeeting = participants.filter(p => p.status === ParticipantStatus.InMeeting).sort((x, y) => { return x-y; });
        let hr = [];

        handRaises.forEach(h => {
            let current = { ...inMeeting.filter(p => p.id === h.participantId)[0] }
            current.handRaised = h.timestamp;
            hr.push(current)
        });

        // Sorting by date: oldest to newest
        sortObjArrayByDateProp_ASC(hr, 'handRaised');

        return hr;
    } 

    render() {
        let { isHandRaisedOpen, isHostsOpen, isModsOpen, isGuestsOpen } = this.state;
        // Fields
        let { id, isOpen, participants } = this.props;
        let self = participants.filter(p => p.isSelf === true)[0];
        
        let inMeeting = participants.filter(p => p.status === ParticipantStatus.InMeeting).sort((x, y) => { return x-y; });
        // let inLobby = participants.filter(p => p.status === ParticipantStatus.InLobby)
        // let notInMeeting = participants.filter(p => p.status === ParticipantStatus.NotInMeeting)

        let handRaised = this.prepareHandRaised();
        let hosts = inMeeting.filter(p => p.isHost);
        let mods = inMeeting.filter(p => p.isModerator);
        let guests = inMeeting.filter(p => !p.isHost && !p.isModerator);
        
        return (
            <SlideDrawer id={id} isOpen={isOpen} onClose={this.props.onClose}>

                <div className="slide-drawer-header">
                    <b> {translateCap('LiveMeeting.participants')} ({inMeeting.length}) </b>
                    <div className="d-inline-block float-right">
                        {
                            self.isModerator ?
                                <Dropdown className="d-inline-block p-0">
                                    <Dropdown.Toggle variant="default" id="participants-menu" className="p-0 mr-3">
                                        <i className="fas fa-ellipsis-h"></i>
                                    </Dropdown.Toggle>
                                    <Dropdown.Menu>
                                        <Dropdown.Item onClick={this.props.onMuteAll}> {translateCap('LiveMeeting.Participants.muteAll')} </Dropdown.Item>
                                        <Dropdown.Item onClick={this.props.onUnmuteAll}> {translateCap('LiveMeeting.Participants.unmuteAll')} </Dropdown.Item>
                                    </Dropdown.Menu>
                                </Dropdown> : null
                        }
                        <button className="btn btn-sm btn-default ml-1" title={translateCap('general.close')}>
                            <i className="fas fa-times fa-lg text-danger" onClick={this.props.onClose}></i>
                        </button>      
                    </div>
                </div>

                <div className="slide-drawer-body">
                    <ul className="list-group list-group-flush h-100">
                        <ParticipantsSection
                            id='handRaised-section'
                            self={self}
                            isOpen={isHandRaisedOpen}
                            onToggleOpen={() => this.setState({ isHandRaisedOpen: !isHandRaisedOpen })}
                            participants={handRaised}
                            onLowerHand={this.props.onLowerHand}
                            onToggleAudio={this.props.onToggleAudio}
                            onRemove={this.props.onRemove}
                            onMakeHost={this.props.onMakeHost}
                        > 
                            <span> {translateCap('LiveMeeting.Participants.raisedHands')} </span>
                        </ParticipantsSection>
                        <ParticipantsSection
                            id='hosts-section'
                            self={self}
                            isOpen={isHostsOpen}
                            onToggleOpen={() => this.setState({ isHostsOpen: !isHostsOpen })}
                            participants={hosts}
                            onLowerHand={this.props.onLowerHand}
                            onToggleAudio={this.props.onToggleAudio}
                            onRemove={this.props.onRemove}
                            onMakeHost={this.props.onMakeHost}
                        >
                            <span> {translateCap('LiveMeeting.Participants.hosts')} </span>
                        </ParticipantsSection>
                        <ParticipantsSection
                            id='moderators-section'
                            self={self}
                            isOpen={isModsOpen}
                            onToggleOpen={() => this.setState({ isModsOpen: !isModsOpen })}
                            participants={mods}
                            onLowerHand={this.props.onLowerHand}
                            onToggleAudio={this.props.onToggleAudio}
                            onRemove={this.props.onRemove}
                            onMakeHost={this.props.onMakeHost}
                        >
                            <span> {translateCap('LiveMeeting.Participants.moderators')} </span>
                        </ParticipantsSection>
                        <ParticipantsSection
                            id='guests-section'
                            self={self}
                            isOpen={isGuestsOpen}
                            onToggleOpen={() => this.setState({ isGuestsOpen: !isGuestsOpen })}
                            participants={guests}
                            onLowerHand={this.props.onLowerHand}
                            onToggleAudio={this.props.onToggleAudio}
                            onRemove={this.props.onRemove}
                            onMakeHost={this.props.onMakeHost}
                        >
                            <span> {translateCap('LiveMeeting.Participants.guests')} </span>
                        </ParticipantsSection>
                    </ul>
                </div>

            </SlideDrawer>
        );
    }
}

Participants.propTypes = {
    id: PropTypes.string,
    participants: PropTypes.array.isRequired,
    handRaises: PropTypes.array.isRequired,
    isOpen: PropTypes.bool.isRequired,
    onClose: PropTypes.func.isRequired,
    onMuteAll: PropTypes.func.isRequired,
    onUnmuteAll: PropTypes.func.isRequired,
    onLowerHand: PropTypes.func.isRequired,
    onToggleAudio: PropTypes.func.isRequired,
    onRemove: PropTypes.func.isRequired,
    onMakeHost: PropTypes.func.isRequired
};

Participants.defaultProps = {
    id: 'participants-drawer',
    participants: [],
    handRaises: [],
    isOpen: false,
    onClose: () => console.log('onClose'),
    onMuteAll: () => console.log('onMuteAll'),
    onUnmuteAll: () => console.log('onUnmuteAll'),
    onLowerHand: () => console.log('onLowerHand'),
    onToggleAudio: () => console.log('onToggleAudio'),
    onRemove: () => console.log('onRemove'),
    onMakeHost: () => console.log('onMakeHost')
};


export default Participants;