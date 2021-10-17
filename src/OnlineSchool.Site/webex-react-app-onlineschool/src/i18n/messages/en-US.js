import { LOCALES } from './../locales';

const en = {
    [LOCALES.ENGLISH]: {
        'general': {
            'changeLanguage': 'change language',
            'actions': 'actions',
            'close': 'close',
            'enterFullscreen': 'enter fullscreen',
            'exitFullscreen': 'exit fullscreen',
            'empty': 'empty',
            'goBack': 'go back',
            'minutes': 'minutes',
            'remove': 'remove',
            'save': 'save',
            'search': 'search',
            'showMore': 'show more',
            'showLess': 'show less'
        },
        'LiveMeeting': {
            'mute': 'mute',
            'unmute': 'unmute',
            'participants': 'participants',
            'chat': 'chat',
            'raiseHand': 'raise hand',
            'lowerHand': 'lower hand',
            'executionSteps': {
                'localStreamProblem': 'Could not retrieve local audio/video. Make sure you have audio/video devices, and that the sources are not being used by another app.',
                'loading': 'loading...',
                'initializing': 'initializing webex instance...',
                'registering': 'registering device...',
                'joining': 'joining meeting...',
                'meetingReady': 'meeting ready',
                'meetingDestroyed': 'meeting has ended / you are no longer in meeting.',
                'failed': 'something failed ✘',
                'missingInformation': 'internal server error! Some information is missing! ✘',
                'networkDisconnected': 'network disconnected',
                'reconnectionStarting': 'attempting to reconnect...',
                'reconnectionSuccess': 'reconnection successful',
                'reconnectionFailure': 'Reconnection failed. Please try to re-join the meeting once again.',
                'networkConnected': 'network reconnected'
            },
            'myScreen': 'my screen',
            'otherScreen': "{name}'s screen",
            'unmuteRequest': 'you are requested to unmute yourself. Unmute?',
            'leaveMeetingPrompt': 'are you sure you want to leave the meeting?',
            'leaveMeetingPrompt_host': 'you are about to end the meeting for all. Continue?',
            'localVideo': 'local video',
            'remoteVideo': 'remote video',
            'recording': 'recording',
            'recordingPaused': 'recording paused',
            'maxNumberOfInterventions': 'maximum number of interventions is 10',
            'MeetingControls': {
                'controls': 'controls',
                'toggleAudio': 'toggle audio',
                'muteAudio': 'mute audio',
                'unmuteAudio': 'unmute audio',
                'unmuteDisallowed': 'your audio is blocked',
                'toggleVideo': 'toggle video',
                'intervention': 'intervention',
                'muteVideo': 'video on',
                'unmuteVideo': 'video off',
                'toggleScreenShare': 'toggle screen-share',
                'shareScreen': 'share screen',
                'stopShare': 'Stop share',
                'leaveMeeting': 'leave meeting',
                'endMeeting': 'end meeting'
            },
            'OtherOptionsButton': {
                'moreOptions': 'more options',
                'layout': 'layout',
                'displayMyVideo': 'show my video',
                'hideMyVideo': 'hide my video',
                'startRecording': 'start recording',
                'pauseRecording': 'pause recording',
                'resumeRecording': 'resume recording',
                'stopRecording': 'stop recording',
                'videoQuality': 'video quality',
                'remoteVideoQuality': "incoming video quality",
                'low': 'low',
                'medium': 'medium',
                'high': 'high',
                'videoLayout': 'video layout',
                'single': 'single',
                'equal': 'equal',
                'activePresence': 'active presence',
                'prominent': 'prominent'
            },
            'Participants': {
                'muteAll': 'mute all',
                'unmuteAll': 'unmute all',
                'raisedHands': 'raised hands',
                'hosts': 'hosts',
                'moderators': 'moderators',
                'guests': 'guests'
            },
            'Participant': {
                'host': 'host',
                'me': 'me',
                'expel': 'expel'
            },
            'Chat': {
                'chatNotAvailable': 'chat is not available right now.',
                'downloadChat': 'download chat',
                'writeMessage': 'write a message'
            },
            'notifications': {
                'newMessage': 'new message(s)',
                'noHost': 'meeting has ended due to having no host online (reason: host might have disconnected)'
            }
        },
        'MeetingForm': {
            'missingAuthToken': 'Authorization token is missing',
            'createMeeting': 'new scheduled meeting',
            'updateMeeting': 'update meeting information',
            'advancedOptions': 'advanced options',
            'noInvitees': 'no invitees',
            'title': 'title',
            'agenda': 'agenda',
            'password': 'password',
            'start': 'start',
            'end': 'end',
            'timezone': 'timezone',
            'sendEmail': 'send email',
            'allowAnyUserToBeCoHost': 'allow any user to be co-host',
            'enabledAutoRecordMeeting': 'auto-record meeting',
            'enabledJoinBeforeHost': 'enable join before host'
        },
        'Invitees': {
            'newInvitation': 'new invitation',
            'noInvitees': 'no invitees',
            'invitees': 'invitees',
            'invitee': 'invitee',
            'email': 'email',
            'name': 'nome',
            'coHost': 'co-host',
            'invite': 'invitare'
        },
        'confirmations': {
            'removeInvitee': "Are you sure you want to remove '{email}' from the invitees?"
        },
        'notifications': {
            'meetingCreated': 'meeting created successfully',
            'meetingUpdated': 'meeting updated successfully',
            'newMessage': "new message(s)",
            'mutedByOthers': "you have been muted",
            'meetingDestroyed': "meeting has ended / you are no longer in meeting.",
            'noHost': "meeting has ended due to having no host online (reason: host might have disconnected)",
            'maxHandRaiseReached': "maximum number of interventions is 10."
        },
        'validations': {
            'invalidEmail': 'email is invalid',
            'meeting': {
                'titleEmpty': 'title is required',
                'startEmpty': 'start time is required',
                'endEmpty': 'end time is required',
                'startTimeBeforeCurrentTime': 'start time cannot be before current time',
                'endTimeBeforeCurrentTime': 'end time cannot be before current time',
                'endTimeBeforeStartTime': 'end time cannot be before start time',
                'invalidDuration': 'meeting duration cannot be shorter than 10 minutes / longer than 24 hours',
                'joinBeforeHostMinutes': 'invalid "join before host" minutes'
            }
        }
    }
}

export default en;