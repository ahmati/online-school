import React, { Component } from 'react';
import MeetingForm from './../../components/Meetings/MeetingForm';

class CreateMeetingPage extends Component {
    
    componentDidMount() {
        // Do something
    }

    render() {
        return (
            <div className="transition-page">
                <MeetingForm />
            </div>
        );
    }
}

export default CreateMeetingPage;