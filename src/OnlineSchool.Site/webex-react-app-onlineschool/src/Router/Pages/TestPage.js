import React, { Component } from 'react';
import { createMembershipAsync, createRoomAsync, getGuestDetails, initializeGuest, initWebexAsync, issueGuestToken, registerGuestAsync } from '../../utils/webexConfig';
import { token } from '../../utils/secrets';
import isEqual from 'react-fast-compare';

class TestPage extends Component {

    async componentDidMount() {
        let guestToken = issueGuestToken('bot1@test.com', 'Bot 1');
        let guestAccessToken = await registerGuestAsync(guestToken);
        console.log('Guest access token: ', guestAccessToken);
        let guestDetails = await getGuestDetails(guestAccessToken);
        console.log('Guest details: ', guestDetails);
    }

    render() {
        return (
            <div>
                
            </div>
        );
    }
}

export default TestPage;