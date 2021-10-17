import React, { Component } from 'react';
import { Link, Redirect, withRouter } from 'react-router-dom';
import PropTypes from 'prop-types';

import { connect } from 'react-redux';
import { getUpcomingMeetings, deleteMeeting } from '../../redux/actions/meetingsActions';

import BeatLoader from "react-spinners/BeatLoader";
import MeetingListItem from './MeetingListItem';
import { confirmation } from './../../utils/helpers';
import { token } from './../../utils/secrets';
import Refresh from '../Layout/Shared/Refresh/Refresh';

class MeetingList extends Component {

    componentDidMount() {
        this.props.getUpcomingMeetings();
    }

    goToSchedulePage = () => {
        localStorage.setItem('ciscoAuthToken', token)
        this.props.history.push("/riunioneWebex/schedule")
    }

    onDeleteMeeting = (meetingId) => {
        /* Internationalization */
        confirmation('Are you sure you want to delete this meeting?', () => {
            this.props.deleteMeeting(meetingId);
        }, null);
    }

    render() {
        let { meetings, isMeetingsLoading } = this.props;
        return (
            <div id="meetings">
                <div className="row mt-2 p-2">
                    <div className="col-12">
                        <h4 className="d-inline-block"> Meetings </h4>
                        <Refresh color="primary" size="sm" className="float-right ml-1" onClick={() => this.props.getUpcomingMeetings()} />
                        <button className="btn btn-sm btn-primary float-right" onClick={this.goToSchedulePage}> 
                            <i className="fas fa-plus-square"></i> Schedule
                        </button>
                        <div className="col-12 p-0">
                        {
                            isMeetingsLoading === true ?
                                <div className="d-block text-center">
                                    <BeatLoader color="#007bff" loading={isMeetingsLoading} size={15} />
                                </div>
                                :
                                (meetings && meetings.length > 0) ?
                                    meetings.map((m, i) => <MeetingListItem key={i} meeting={m} onDelete={this.onDeleteMeeting} /> )
                                    :
                                    <div className="col-12 p-0 mt-3">
                                        <div className="alert alert-primary" role="alert">
                                            <i className="fas fa-info-circle"></i> No upcoming meetings.
                                        </div>
                                    </div>        
                        }
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}

MeetingList.propTypes = {
    meetings: PropTypes.array.isRequired,
    isMeetingsLoading: PropTypes.bool.isRequired,
    errors: PropTypes.array
}

const mapStateToProps = state => ({
    meetings: state.meetingsReducer.meetings,
    isMeetingsLoading: state.meetingsReducer.isMeetingsLoading,
    errors: state.errorsReducer.errors
});

export default connect(mapStateToProps, { getUpcomingMeetings, deleteMeeting })(withRouter(MeetingList));
