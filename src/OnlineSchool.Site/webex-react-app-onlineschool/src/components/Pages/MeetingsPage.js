import React, { Component } from 'react'
import MeetingList from '../Meetings/MeetingList';
import PersonalInfo from '../Personal/PersonalInfo';

class MeetingsPage extends Component {

    componentDidMount() {
        // Do something
    }

    render() {
        return (
            <div className="transition-page">
                <PersonalInfo />
                <MeetingList />
            </div>
        )
    }
}

export default MeetingsPage;