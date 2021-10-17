import React from 'react';
import { Route, Switch } from 'react-router-dom';
import { TransitionGroup, CSSTransition } from 'react-transition-group';

import CustomRoute from './CustomRoute';
import LiveMeeting from './../components/Meetings/LiveMeeting/LiveMeeting/LiveMeeting';
import MeetingsPage from './Pages/MeetingsPage';
import CreateMeetingPage from './Pages/CreateMeetingPage';
import EditMeetingPage from './Pages/EditMeetingPage';
import TestPage from './Pages/TestPage';

function Routes({ locale }) {
    return (
      <Switch>
            <CustomRoute exact
                path={[
                    "/LiveSession/:sessionId(\\d+)/Host",
                    "/LiveSession/:sessionId(\\d+)/Guest",
                    "/SpotMeeting/:sessionId(\\d+)/Host",
                    "/SpotMeeting/:sessionId(\\d+)/Guest",
                    "/riunioneWebex/meetings/live"
                ]}
                component={LiveMeeting}
            />
            <CustomRoute exact path={["/", "/default", "/meetings", "/default/meetings"]} component={MeetingsPage} />
            <CustomRoute exact path={["/riunioneWebex/schedule"]} component={CreateMeetingPage} />
            <CustomRoute exact path={["/riunioneWebex/riunione/modifica"]} component={EditMeetingPage} />
            <CustomRoute exact path={["/test"]} component={TestPage} />
      </Switch>
    )
}

export default Routes;
