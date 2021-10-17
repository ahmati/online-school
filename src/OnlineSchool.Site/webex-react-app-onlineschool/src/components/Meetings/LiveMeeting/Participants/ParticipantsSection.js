import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { Collapse } from 'react-bootstrap';
import Participant from './Participant';
import isEqual from 'react-fast-compare';
import { translateCap } from '../../../../i18n/translate';

class ParticipantsSection extends Component {

    constructor() {
        super();
        this.state = {
            displaySet: [],
            displayCount: 10,
            searchKey: ""
        }
    }

    componentDidMount() {
        this.setState({ displaySet: this.props.participants });
    }

    componentDidUpdate(prevProps) {
        let { searchKey } = this.state;
        let { participants } = this.props;

        if(isEqual(participants, prevProps.participants) === false) {
            let displaySet = participants;
            if(searchKey.length >= 2)
                displaySet = participants.filter(p => p.name.toLowerCase().includes(searchKey));
            this.setState({ displaySet });
        }
    }

    onSearch = (e) => {
        let { participants } = this.props;

        let searchKey = e.target.value.trim().toLowerCase();
        let displaySet = participants;
        if(searchKey.length >= 2) {
            displaySet = participants.filter(p => p.name.toLowerCase().includes(searchKey));
        }

        this.setState({ displaySet, searchKey });
    }

    adjustDisplayCount = (size) => {
        this.setState({ displayCount: this.state.displayCount + size });
    }

    render() {
        let { displaySet, displayCount } = this.state;
        let { id, title, self, isOpen, onToggleOpen, participants, onLowerHand, onToggleAudio, onRemove, onMakeHost, children } = this.props;

        return (
            <>
                <li id="participants-section-title" className="list-group-item border cursor-pointer border-bottom" onClick={onToggleOpen}> 
                    {children} 
                    <i className={`fas fa-chevron-${isOpen ? 'up' : 'down'} absolute-right mr-3`}></i>
                </li>
                <Collapse in={isOpen}>
                    <div id={id}>
                        <input type="search" placeholder={`${translateCap('general.search')}...`} className="search-input w-100" onChange={this.onSearch} />
                        {
                            (displaySet.length === 0) ?
                                <li className="list-group-item">
                                    <small className="text-muted ml-2">
                                        <i className="fas fa-info"></i> {translateCap('general.empty')}
                                    </small>
                                </li>
                                :
                                <>
                                    <div className="list-container">
                                    {
                                        displaySet.slice(0, displayCount).map(p => {
                                            return (
                                                <Participant
                                                    key={p.id}
                                                    self={self}
                                                    participant={p}
                                                    onLowerHand={onLowerHand}
                                                    onToggleAudio={onToggleAudio}
                                                    onRemove={onRemove}
                                                    onMakeHost={onMakeHost}
                                                />
                                            )
                                        })
                                    }
                                    {
                                        (displaySet.length > displayCount) ?
                                            <button className="btn btn-sm btn-block" onClick={() => this.adjustDisplayCount(10)}>
                                                {translateCap('general.showMore')}
                                            </button>
                                            : null
                                    }
                                    {
                                        (displayCount > 10) ?
                                            <button className="btn btn-sm btn-block" onClick={() => this.adjustDisplayCount(-10)}>
                                                {translateCap('general.showLess')}
                                            </button>
                                            : null
                                    }
                                    </div>
                                </>
                        }
                    </div>
                </Collapse>
            </>
        );
    }
}

ParticipantsSection.propTypes = {
    id: PropTypes.string,
    self: PropTypes.object.isRequired,
    isOpen: PropTypes.bool.isRequired,
    onToggleOpen: PropTypes.func.isRequired,
    isModerator: PropTypes.bool.isRequired,
    participants: PropTypes.array.isRequired,
    onLowerHand: PropTypes.func.isRequired,
    onToggleAudio: PropTypes.func.isRequired,
    onRemove: PropTypes.func.isRequired,
    onMakeHost: PropTypes.func.isRequired
}

ParticipantsSection.defaultProps = {
    id: 'participants-section',
    self: { isHost: false, isModerator: false },
    isOpen: false,
    onToggleOpen: () => console.log('onToggleOpen'),
    isModerator: false,
    participants: [],
    onLowerHand: () => console.log('onLowerHand'),
    onToggleAudio: () => console.log('onToggleAudio'),
    onRemove: () => console.log('onRemove'),
    onMakeHost: () => console.log('onMakeHost')
}

export default ParticipantsSection;