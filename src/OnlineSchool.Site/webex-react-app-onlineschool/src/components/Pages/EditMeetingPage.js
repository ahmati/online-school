import React, { Component } from 'react';
import MeetingForm from './../Meetings/MeetingForm';

class EditMeetingPage extends Component {

    componentDidMount() {
        // Do something here
    }

    render() {
        return (
            <div className="transition-page">
                <MeetingForm />
            </div>
        );
    }
}

export default EditMeetingPage;