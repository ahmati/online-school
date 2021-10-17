import React, { Component } from 'react'
import PersonalInfo from '../../components/Personal/PersonalInfo';
import MeetingList from '../../components/Meetings/MeetingList';

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