import React, { Component } from 'react';
import {CopyToClipboard} from 'react-copy-to-clipboard';
import { Link, withRouter } from 'react-router-dom';

import { confirmation } from '../../utils/helpers';
import { token } from '../../utils/secrets';

class MeetingListItem extends Component {

    constructor(props) {
        super(props)
        this.state = {
            meetingId_clipboard: false,
            meetingNumber_clipboard: false,
            meetingSipAddress_clipboard: false,
            meetingLink_clipboard: false
        }
    }

    copiedToClipboard(targetName) {
        Object.keys(this.state).filter(p => p.includes('_clipboard')).forEach(k => {
            if(k !== targetName)
                this.setState({ [k]: false });
        })
        this.setState({ [targetName]: true });
        setTimeout(
            () => this.setState({ [targetName]: false }), 
            3000
          )
    }

    startMeeting = (sipAddress) => {
        /* Internationalization */
        confirmation('You are about to start this meeting. Continue?', () => {
            localStorage.setItem('meetingSipAddress', sipAddress)
            localStorage.setItem('ciscoAuthToken', token)
            this.props.history.push('/riunioneWebex/meetings/live');
        }, null);
    }

    goToEditPage = (meetingId) => {
        localStorage.setItem('ciscoAuthToken', token);
        localStorage.setItem('webexMeetingId', meetingId);
        this.props.history.push('/riunioneWebex/riunione/modifica');
    }

    render() {
        let { meeting } = this.props;
        let { meetingId_clipboard, meetingNumber_clipboard, meetingSipAddress_clipboard, meetingLink_clipboard } = this.state;
        return (
            <div className="col-12 p-0 my-1">
                <div className="card">
                    <div className="card-header p-0">
                        <button className="btn btn-sm text-danger float-right" onClick={() => this.props.onDelete(meeting.id)}>
                            <i className="fas fa-trash-alt" title="Delete"></i> {/* Internationalization */}
                        </button>
                        <button className="btn btn-sm text-secondary float-right" onClick={() => this.goToEditPage(meeting.id)}>
                            <i className="fas fa-edit" title="Edit"></i> {/* Internationalization */}
                        </button>
                    </div>
                    <div className="card-body p-2">
                        <h5 className="card-title"> {meeting.title} </h5>
                        <span className="d-block border-bottom mb-2">
                            <b> {(new Date(meeting.start)).toLocaleString()} </b> - <b> {(new Date(meeting.end)).toLocaleString()} </b>
                        </span>
                        <span className="d-block">
                            Meeting id: {meeting.id} 
                            <CopyToClipboard text={meeting.id} onCopy={() => this.copiedToClipboard('meetingId_clipboard')}>
                                <i className="ml-1 far fa-copy cursor-pointer" title="Copy"> {/* Internationalization */} </i>
                            </CopyToClipboard>
                            {
                                meetingId_clipboard ?
                                    <small className="text-muted"> (Copied) {/* Internationalization */} </small> 
                                    : null
                            }
                        </span>
                        <span className="d-block">
                            Meeting number: {meeting.meetingNumber} {/* Internationalization */}
                            <CopyToClipboard text={meeting.meetingNumber} onCopy={() => this.copiedToClipboard('meetingNumber_clipboard')}>
                                <i className="ml-1 far fa-copy cursor-pointer" title="Copy"> {/* Internationalization */} </i>
                            </CopyToClipboard>
                            {
                                meetingNumber_clipboard ?
                                    <small className="text-muted"> (Copied) {/* Internationalization */} </small> 
                                    : null
                            }
                        </span>
                        <span className="d-block">
                            Sip Address: {meeting.sipAddress}
                            <CopyToClipboard text={meeting.sipAddress} onCopy={() => this.copiedToClipboard('meetingSipAddress_clipboard')}>
                                <i className="ml-1 far fa-copy cursor-pointer" title="Copy"> {/* Internationalization */} </i>
                            </CopyToClipboard>
                            {
                                meetingSipAddress_clipboard ?
                                    <small className="text-muted"> (Copied) {/* Internationalization */} </small> 
                                    : null
                            }
                        </span>
                        <span className="d-block">
                            Meeting link: <i> {meeting.webLink} </i> {/* Internationalization */}
                            <CopyToClipboard text={meeting.webLink} onCopy={() => this.copiedToClipboard('meetingLink_clipboard')}>
                                <i className="ml-1 far fa-copy cursor-pointer" title="Copy"> {/* Internationalization */} </i> 
                            </CopyToClipboard>
                            {
                                meetingLink_clipboard ?
                                    <small className="text-muted"> (Copied)  </small>
                                    : null
                            }
                        </span>
                        <div className="text-right">
                            <Link to={`meetings/${meeting.id}/details`} className="btn btn-sm btn-info"> 
                                <i className="fas fa-info-circle"></i> Details {/* Internationalization */}
                            </Link>
                            <button className="btn btn-sm btn-success ml-2 " onClick={() => this.startMeeting(meeting.sipAddress)}> 
                                <i className="fas fa-sign-in-alt"></i> Start meeting {/* Internationalization */}
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}

export default withRouter(MeetingListItem);