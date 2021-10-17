import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { withRouter } from 'react-router';
import isEqual from 'react-fast-compare';

import { connect } from 'react-redux';
import { clearErrors, setError, setErrors } from '../../redux/actions/errorActions';
import { addInvitee, deleteInvitee, getInvitees } from '../../redux/actions/inviteesActions';
import { addMeeting, getMeetingById, updateMeeting } from '../../redux/actions/meetingsActions';
import configureHeaders from '../../redux/securityUtils/configureHeaders';

import classnames from 'classnames';
import { translate, translateCap } from '../../i18n/translate';
import { convertToInputDateTimeFormat, getMinutesDiff, scrollToTop } from '../../utils/helpers';
import { join_before_host_minutes } from '../../utils/constants';

import GoBack from '../Layout/Shared/GoBack';
import Invitees from '../Invitees/Invitees';
import { Collapse } from 'react-bootstrap';
import { BeatLoader } from 'react-spinners';
import { toast } from 'react-toastify';
import axios from 'axios';

class MeetingForm extends Component {

    constructor(props) {
        super(props)
        this.minutesDropdown = React.createRef()
        this.state = {
            id: null,
            title: '',
            agenda: '',
            password: '',
            start: '',
            end: '',
            timezone: '',
            recurrence: '',
            sendEmail: true,
            enabledAutoRecordMeeting: false,
            allowAnyUserToBeCoHost: false,
            enabledJoinBeforeHost: false,
            joinBeforeHostMinutes: 0,
            invitees: [],

            isAdvancedOptionsOpen: false,

            isUpdate: false
        }
    }

    componentDidMount() {
        this.props.clearErrors();

        let token = localStorage.getItem('ciscoAuthToken');
        if(!token) {
            this.props.setError(translateCap('MeetingForm.missingAuthToken'));
        }
        else {
            configureHeaders(`Bearer ${token}`);

            let meetingId = localStorage.getItem('webexMeetingId');
            if(meetingId) {
                this.props.getMeetingById(meetingId);
                this.props.getInvitees(meetingId);
    
                let { current_meeting: meeting, current_invitees: invitees } = this.props;
                let start = convertToInputDateTimeFormat(new Date(meeting.start));
                let end = convertToInputDateTimeFormat(new Date(meeting.end));
    
                this.setState({ id: meetingId, ...meeting, start, end, invitees, isUpdate: true });
            }
        }

        localStorage.removeItem('ciscoAuthToken');
        localStorage.removeItem('webexMeetingId');
    }

    componentDidUpdate(prevProps) {
        if(!isEqual(prevProps.current_meeting, this.props.current_meeting)) {
            let { current_meeting: meeting } = this.props;
            let start = convertToInputDateTimeFormat(new Date(meeting.start));
            let end = convertToInputDateTimeFormat(new Date(meeting.end));
            this.setState({ ...meeting, start, end });
        }

        if(!isEqual(prevProps.current_invitees, this.props.current_invitees)) {
            let { current_invitees: invitees } = this.props;
            this.setState({ invitees });
        }
    }

    componentWillUnmount() {
        // Cleanup code here
    }

    addMeetingInvitee = async (meetingId, invitee) => {
        let newInvitee = { meetingId, ...invitee };
        this.props.addInvitee(newInvitee);
    }

    onChange = e => {
        this.setState({ [e.target.name]: e.target.value });
    }

    onCheckboxChange = e => {
        this.setState({ [e.target.name]: e.target.checked });
        if(e.target.name === 'enabledJoinBeforeHost')
            this.minutesDropdown.current.disabled = !e.target.checked;
    }

    toggleAdvancedOptions = () => {
        let { isAdvancedOptionsOpen } = this.state;
        this.setState({ isAdvancedOptionsOpen: !isAdvancedOptionsOpen });
    }

    onAddInvitee = (newInvitee) => {
        let { id, invitees, isUpdate } = this.state;
        
        let exists = invitees.find(i => i.email === newInvitee.email);
        if(exists)
            return;

        if(isUpdate)
            this.addMeetingInvitee(id, newInvitee);
        else {
            invitees.push(newInvitee);
            this.setState({ invitees });     
        }
    }
    
    onRemoveInvitee = (email) => {
        let { invitees, isUpdate } = this.state;

        let exists = invitees.find(i => i.email === email);
        if(!exists)
            return;

        if(isUpdate)
            this.props.deleteInvitee(exists.id);
        else {
            this.setState({ invitees: [...invitees.filter(i => i.email !== email)] });
        }
    }

    resetForm = () => {
        this.setState({
            id: null,
            title: '',
            agenda: '',
            password: '',
            start: '',
            end: '',
            timezone: '',
            recurrence: '',
            sendEmail: true,
            enabledAutoRecordMeeting: false,
            allowAnyUserToBeCoHost: false,
            enabledJoinBeforeHost: false,
            joinBeforeHostMinutes: 0,
            invitees: [],

            isAdvancedOptionsOpen: false,

            isUpdate: false
        })
    }

    createMeeting = () => {
        let meeting = this.getMeetingObject();
        console.log('New meeting: ', meeting);
        
        let errors = this.validateMeeting(meeting);
        if(errors.length > 0) {
            this.props.setErrors(errors)
            scrollToTop()
        }
        else {
            axios.post('https://webexapis.com/v1/meetings', meeting)
                .then(res => {
                    let statusCode = res.status;
                    // Response: Ok
                    if(statusCode === 200 || statusCode === 204) {
                        toast.success(translateCap('notifications.meetingCreated'))
                        this.props.clearErrors()
                        this.resetForm()
                    }
                    // Response: Not ok
                    else if(statusCode >= 400) {
                        let error = res.data;
                        scrollToTop();
                        this.props.setError(error)
                    }
                })
                .catch(err => {
                    console.error(err)
                    scrollToTop();
                    this.props.setError(err.response.data.message)
                });
        }
    }

    updateMeeting = () => {
        let { id } = this.state;
        let meeting = this.getMeetingObject();
        console.log('Updated meeting: ', meeting);

        let errors = this.validateMeeting(meeting);
        if(errors.length > 0) {
            this.props.setErrors(errors)
            scrollToTop()
        }
        else
            axios.put(`https://webexapis.com/v1/meetings/${id}`, meeting)
                .then(res => {
                    let statusCode = res.status;
                    // Response: Ok
                    if(statusCode === 200 || statusCode === 204) {
                        toast.success(translateCap('notifications.meetingUpdated'))
                        this.props.clearErrors()
                        this.resetForm()
                    }
                    // Response: Not ok
                    else if(statusCode >= 400) {
                        let error = res.data;
                        scrollToTop();
                        this.props.setError(error)
                    }
                })
                .catch(err => {
                    console.error(err)
                    scrollToTop();
                    this.props.setError(err.response.data.message)
                });
    }

    getMeetingObject = () => {
        let { title, agenda, password, start, end, timezone, recurrence, sendEmail, enabledAutoRecordMeeting, allowAnyUserToBeCoHost, enabledJoinBeforeHost, joinBeforeHostMinutes, invitees } = this.state;
        let meeting = { title, agenda, password, start, end, timezone, recurrence, sendEmail, enabledAutoRecordMeeting, allowAnyUserToBeCoHost, enabledJoinBeforeHost, joinBeforeHostMinutes: parseInt(joinBeforeHostMinutes), invitees };

        if(this.state.isUpdate)
            delete meeting['invitees'];

        if(meeting.start !== '')
            meeting.start = new Date(start).toISOString();
        if(meeting.end !== '')
            meeting.end = new Date(end).toISOString();
        if(meeting.enabledJoinBeforeHost === false)
            delete meeting['joinBeforeHostMinutes'];

        // Remove empty keys
        Object.keys(meeting).forEach(key => {
            if(meeting[key] === undefined || (typeof(meeting[key]) === "string" && meeting[key].trim() === "") || (Array.isArray(meeting[key]) && meeting[key].length === 0))
                delete meeting[key];
        });

        return meeting;
    }

    validateMeeting = (meeting) => {
        let errors = [];
    
        if(!meeting.title)
            errors.push(translateCap('validations.meeting.titleEmpty'));
        if(!meeting.start)
            errors.push(translateCap('validations.meeting.startEmpty'));
        if(!meeting.end)
            errors.push(translateCap('validations.meeting.endEmpty'));
    
        if(meeting.start && meeting.end) 
        {
            let now = new Date();
            let start = new Date(meeting.start);
            let end = new Date(meeting.end);
    
            if(start < now)
                errors.push(translateCap('validations.meeting.startTimeBeforeCurrentTime'));
            if(end < now)
                errors.push(translateCap('validations.meeting.endTimeBeforeCurrentTime'));
            if(start > end)
                errors.push(translateCap('validations.meeting.endTimeBeforeStartTime'));
    
            let minutesDiff = getMinutesDiff(start, end);
            if(minutesDiff < 10 || minutesDiff > 24 * 60)
                errors.push(translateCap('validations.meeting.invalidDuration'));
        }
    
        if(meeting.enabledJoinBeforeHost === true)
            if(!join_before_host_minutes.includes(meeting.joinBeforeHostMinutes))
                errors.push(translateCap('validations.meeting.joinBeforeHostMinutes'));
    
        return errors;
    }

    onSubmit = () => {
        let { isUpdate } = this.state;
        if(!isUpdate)
            this.createMeeting();
        else {
            this.updateMeeting();
        }
    }

    renderMinutesDropdown = () => {
        let { enabledJoinBeforeHost, joinBeforeHostMinutes } = this.state;
        return enabledJoinBeforeHost ?
            (
                <select id="meeting-joinBeforeHostMinutes" name="joinBeforeHostMinutes" className="mx-1"
                    ref={this.minutesDropdown}
                    onChange={this.onChange}
                    value={joinBeforeHostMinutes}>
                    { join_before_host_minutes.map((m, i) => <option key={i} value={m}> {m} </option>) }
                </select>
            ) :
            (
                <select id="meeting-joinBeforeHostMinutes" name="joinBeforeHostMinutes" className="mx-1" disabled
                    ref={this.minutesDropdown}
                    onChange={this.onChange}
                    value={joinBeforeHostMinutes}>
                    { join_before_host_minutes.map((m, i) => <option key={i} value={m}> {m} </option>) }
                </select>
            )
    }

    render() {
        let { 
            title, agenda, password, start, end, timezone, recurrence, sendEmail, enabledAutoRecordMeeting, allowAnyUserToBeCoHost, enabledJoinBeforeHost, joinBeforeHostMinutes, invitees, 
            isAdvancedOptionsOpen, isUpdate 
        } = this.state;
        let { isMeetingLoading, isInviteesLoading, errors } = this.props;

        return (
            <>
                <div className="row p-2">
                    <div className="col-12 col-md-10 m-auto">
                        
                        <GoBack label={translateCap('general.goBack')} history={this.props.history} />

                        <h4 className="text-right border-bottom"> {isUpdate ? translateCap('MeetingForm.updateMeeting') : translateCap('MeetingForm.createMeeting')} </h4>
                        {
                            isMeetingLoading ?
                                <div className="d-block text-center">
                                    <BeatLoader color="#007bff" loading={isMeetingLoading} size={15} />
                                </div>
                                :
                                <div id="create-meeting-form">

                                    {/* Component: Validation summary */}
                                    <div id="validation-summary" className={classnames("validation-summary my-2 border rounded shadow text-white bg-danger ", {"d-none": !(errors.length > 0)})}>
                                        <div className="col-12 text-right p-0">
                                            <button type="button" className="btn btn-sm btn-default text-white" data-toggle="tooltip" title={translateCap('general.close')} onClick={() => this.props.clearErrors()}> x </button>
                                        </div>
                                        <ul id="validation-summary-errors">
                                            {
                                                errors.map((err, index) => {
                                                    return <li key={index} className="validation-summary-item"> {err} </li>
                                                })
                                            }
                                        </ul>
                                    </div>

                                    {/* Main inputs */}
                                    <div className="form-group">
                                        <label htmlFor="meeting-title"> {translateCap('MeetingForm.title')} <small className="text-danger">*</small> </label>
                                        <input required type="text" id="meeeting-title" name="title" className="form-control form-control-sm" value={title} onChange={this.onChange} />
                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="meeting-agenda"> {translateCap('MeetingForm.agenda')} </label>
                                        <input type="text" id="meeeting-agenda" name="agenda" className="form-control form-control-sm" value={agenda} onChange={this.onChange} />
                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="meeting-password"> {translateCap('MeetingForm.password')} </label>
                                        <input type="password" id="meeeting-password" name="password" className="form-control form-control-sm" value={password} onChange={this.onChange} />
                                    </div>
                                    <div className="form-group row">
                                        <div className="col-12 col-sm-6">
                                            <label htmlFor="meeting-title"> {translateCap('MeetingForm.start')} <small className="text-danger">*</small> </label>
                                            <input required type="datetime-local" id="meeeting-start" name="start" className="form-control form-control-sm" value={start} onChange={this.onChange} />
                                        </div>
                                        <div className="col-12 col-sm-6">
                                            <label htmlFor="meeting-title"> {translateCap('MeetingForm.end')} <small className="text-danger">*</small> </label>
                                            <input required type="datetime-local" id="meeeting-end" name="end" className="form-control form-control-sm" value={end} onChange={this.onChange} />
                                        </div>
                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="meeting-timezone"> {translateCap('MeetingForm.timezone')} </label>
                                        <input type="text" id="meeeting-timezone" name="timezone" className="form-control form-control-sm" value={timezone} onChange={this.onChange} />
                                    </div>
                                    {/* <div className="form-group">
                                        <label htmlFor="meeting-recurrence"> Recurrence </label>
                                        <input type="text" id="meeeting-recurrence" name="recurrence" className="form-control form-control-sm" value={recurrence} onChange={this.onChange} />
                                    </div> */}
                                    <div className="custom-control custom-switch">
                                        <input type="checkbox" id="meeting-sendEmail" name="sendEmail" className="custom-control-input" checked={sendEmail} onChange={this.onCheckboxChange} />
                                        <label className="custom-control-label" htmlFor="meeting-sendEmail"> {translateCap('MeetingForm.sendEmail')} </label>
                                    </div>

                                    {/* Advanced options */}
                                    <div className="form-group my-4 py-1 border-bottom">
                                        <span type="button" aria-controls="advanced-options" aria-expanded={isAdvancedOptionsOpen} onClick={this.toggleAdvancedOptions}>
                                            <i className={`fas fa-chevron-${isAdvancedOptionsOpen ? 'up' : 'down'}`} /> {translateCap('MeetingForm.advancedOptions')}
                                        </span>
                                        <Collapse in={isAdvancedOptionsOpen}>
                                            <div id="advanced-options" className="border-top">
                                                <div className="custom-control custom-switch mt-3">
                                                    <input type="checkbox" id="meeting-allowAnyUserToBeCoHost" name="allowAnyUserToBeCoHost" className="custom-control-input" checked={allowAnyUserToBeCoHost} onChange={this.onCheckboxChange} />
                                                    <label className="custom-control-label" htmlFor="meeting-allowAnyUserToBeCoHost"> {translateCap('MeetingForm.allowAnyUserToBeCoHost')} </label>
                                                </div>
                                                <div className="custom-control custom-switch mt-3">
                                                    <input type="checkbox" id="meeting-enabledAutoRecordMeeting" name="enabledAutoRecordMeeting" className="custom-control-input" checked={enabledAutoRecordMeeting} onChange={this.onCheckboxChange} />
                                                    <label className="custom-control-label" htmlFor="meeting-enabledAutoRecordMeeting"> {translateCap('MeetingForm.enabledAutoRecordMeeting')} </label>
                                                </div>
                                                <div className="custom-control custom-switch mt-3">
                                                    <input type="checkbox" id="meeting-enabledJoinBeforeHost" name="enabledJoinBeforeHost" className="custom-control-input" checked={enabledJoinBeforeHost} onChange={this.onCheckboxChange} />
                                                    <label className="custom-control-label" htmlFor="meeting-enabledJoinBeforeHost"> {translateCap('MeetingForm.enabledJoinBeforeHost')}: </label>
                                                    {this.renderMinutesDropdown()} {translate('general.minutes')}
                                                </div>
                                            </div>
                                        </Collapse>
                                    </div>

                                    <div className="form-group mt-3">
                                        {
                                            isInviteesLoading ?
                                                <div className="d-block text-center">
                                                    <BeatLoader color="#007bff" loading={isInviteesLoading} size={15} />
                                                </div> :
                                                <Invitees invitees={invitees} allowCoHost={allowAnyUserToBeCoHost} onAdd={this.onAddInvitee} onRemove={this.onRemoveInvitee} />
                                        }
                                    </div>

                                    <button type="button" className="btn btn-md btn-primary shadow float-right mr-4" onClick={this.onSubmit}>
                                        <i className="fas fa-save"></i> {translateCap('general.save')}
                                    </button>

                                </div>
                        }
                    </div>
                </div>
            </>
        );
    }
}

MeetingForm.propTypes = {
    current_meeting: PropTypes.object.isRequired,
    isMeetingLoading: PropTypes.bool.isRequired,
    current_invitees: PropTypes.array.isRequired,
    isInviteesLoading: PropTypes.bool.isRequired,
    errors: PropTypes.array.isRequired
}

const mapStateToProps = state => ({
    current_meeting: state.meetingsReducer.current_meeting,
    isMeetingLoading: state.meetingsReducer.isMeetingLoading,
    current_invitees: state.inviteesReducer.current_invitees,
    isInviteesLoading: state.inviteesReducer.isInviteesLoading,
    errors: state.errorsReducer.errors
});

export default connect(mapStateToProps, { addMeeting, addInvitee, deleteInvitee, getMeetingById, getInvitees, updateMeeting, clearErrors, setError, setErrors })(withRouter(MeetingForm));