import React, { Component } from 'react';
import { withRouter } from 'react-router';
import { Prompt } from 'react-router-dom';
import './index.css';

import { waitForMediaReady, initWebexAsync, registerAsync, unregisterAsync, createMeetingAsync, joinMeetingWithMediaAsync, createRoomAsync, getRoomByTitleAsync, createMembershipAsync, removeMembershipAsync, getPersonByIdAsync, createMessageAsync, getRoomByIdAsync, removeMessageAsync, updateMembershipAsync, getAllMembershipsAsync, getAllMessagesAsync } from '../../../../utils/webexConfig';
import { CustomMessages, ExecutionStep, FileSelectMode, JoinIntent, LayoutTypes, LocalStorageKeys, MaxNumberOfInterventions, MeetingState, ParticipantStatus, QualityLevel, QualityLevels, RecordingState } from '../../../../utils/constants';
import { confirmation, isLocalStreamAvailable, sleepAsync, sortObjArrayByDateProp_ASC } from '../../../../utils/helpers';
import { translate, translateCap } from '../../../../i18n/translate';
import isEqual from 'react-fast-compare';

import { Beforeunload } from 'react-beforeunload';
import Draggable from 'react-draggable';
import Participants from '../Participants/Participants';
import Chat from '../Chat/Chat';
import { notifyHandRaise, notifyMaxNrOfInterventionsExceeded, notifyMutedByOthers, notifyNewMessage } from './notifications';
import MeetingControls from '../MeetingControls/MeetingControls/MeetingControls';
import VideoPlayer from '../../../Layout/Shared/VideoPlayer/VideoPlayer';
import RecordingIndicator from '../RecordingIndicator/RecordingIndicator';

class LiveMeeting extends Component {

    constructor() {
        super()
        this.webex = null
        this.sdkMeeting = null
        this.localVideo = React.createRef()
        this.remoteAudio = React.createRef()
        this.remoteVideo = React.createRef()
        this.localShareStream = null
        this.remoteShareStream = null
        this.screenShare = React.createRef()

        this.state = {
            isBlocking: true,
            isMeetingReady: false,
            executionStep: ExecutionStep.Loading,

            self: null,
            selfDetails: null,
            participants: [],
            participantDetails: [],
            handRaises: [],  // Array of: { participantId: '...', timestamp: '...' }

            isLocalSharing: false,
            isRemoteSharing: false,

            isLocalVideoHidden: false,
            isParticipantsMenuOpen: false,

            isChatOpen: false,
            isRoomLoading: false,
            isRoomReady: false,
            isChatDownloading: false,
            roomId: -1,
            memberships: [],
            messages: [],
            senderDetails: []
        }
    }
    
    async componentDidMount() {
        try {
            let sipAddress = localStorage.getItem(LocalStorageKeys.MeetingSipAddress);
            let meetingPassword = localStorage.getItem(LocalStorageKeys.MeetingPassword);
            let token = localStorage.getItem(LocalStorageKeys.CiscoAuthToken);

            if(!sipAddress || !token) {
                await this.setProgress(0, false, ExecutionStep.MissingInformation);
            }
            else {
                if(!(await isLocalStreamAvailable()))
                    this.setProgress(0, false, ExecutionStep.LocalStreamProblem);
                else
                {
                    await this.setProgress(500, false, ExecutionStep.Initializing);
                    this.webex = await initWebexAsync(token);
        
                    this.webex.once('ready', async () => {
                        if(this.webex.canAuthorize) {
                            try {
                                await unregisterAsync(this.webex);
                                await this.setProgress(500, false, ExecutionStep.Registering);
                                await registerAsync(this.webex);
                                let sdkMeeting = await createMeetingAsync(this.webex, sipAddress);
            
                                if(sdkMeeting) {
                                    await this.setProgress(500, false, ExecutionStep.Joining);
            
                                    this.sdkMeeting = sdkMeeting;
                                    await this.bindMembershipEvents();
                                    await this.bindMeetingEvents(sdkMeeting);
                                    await joinMeetingWithMediaAsync(sdkMeeting, meetingPassword);
                                    this.initChatRoom();
                                    await this.bindMessageEvents();
                                    
                                    await this.setProgress(500, false, ExecutionStep.MeetingReady);
                                    await this.setProgress(0, true);
                                }
                                else {
                                    await this.setProgress(false, ExecutionStep.Failed);
                                }
                            }
                            catch(e) {
                                await this.setProgress(false, ExecutionStep.Failed);
                            }
                        }
                    });
                }
            }
        }
        catch(err) {
        }
    }

    componentWillUnmount() {
        this.leaveMeeting();
    }

    onWindowClose = (e) => {
        // this.leaveMeeting();
        e.preventDefault();
        return e.returnValue;
    }

    // componentDidUpdate(prevProps, prevState) {
    //     let { self, handRaises } = this.state;
    //     let activeHandRaises = this.getActiveHandRaisesCount();

    //     // A piece of code for host, to lower extra hand-raises
    //     if(self && self.isHost && activeHandRaises > MaxNumberOfInterventions) {

    //         // First, sort handRaises by timestamp
    //         sortObjArrayByDateProp_ASC(handRaises, 'timestamp');
    //         // Lower extra hand-raises based by timestamp ordering
    //         let extraHandRaises = handRaises.slice(10, handRaises.length);
    //         extraHandRaises.forEach(x => {
    //             this.lowerParticipantHand(x.participantId);
    //         })
    //     }
    // }

    setProgress = async (delay=0, isMeetingReady, executionStep='') => {
        this.setState({ isMeetingReady, executionStep })
        await sleepAsync(delay)
    }
    
    //#region ---------------------------------  Room --------------------------------------------

    initChatRoom = async () => {
        console.log('-> Executing initChatRoom');
        let sdkMeeting = this.sdkMeeting;
        let title = this.getRoomTitle()

        let existingHosts = [];
        let self = null;
        let participants = [];

        if(sdkMeeting) {
            
            let members = sdkMeeting.members.membersCollection.members;
            Object.keys(members).forEach(k => {
                participants.push(members[k]);
                if(members[k].isSelf)
                    self = members[k];
                if(members[k].status === ParticipantStatus.InMeeting && !members[k].isSelf && members[k].isHost)
                    existingHosts.push(members[k]);
            });
            console.log('-> Moderators already in meeting: ', existingHosts);

            if(self) {
                let room = await getRoomByTitleAsync(this.webex, title)
                if(room) {
                    await this.setRoom(room.id)
                }
                else {
                    if(self.isHost && existingHosts.length === 0) {
                        let room = await createRoomAsync(this.webex, title);
                        let roomMembership = (await getAllMembershipsAsync(this.webex, room.id))[0];
                        roomMembership.isModerator = true;
                        roomMembership = await updateMembershipAsync(this.webex, roomMembership);

                        if(room)
                            await this.setRoom(room.id, participants);
                    }
                    else {
                        // Do nothing. You are subscribed to 'membership' events: you will get the room there
                    }
                }
            }
        }
    }

    /**
     * Initiates a complex workflow that prepares everything about the chat room.
     * - Loads all the messages  
     * - Processes/transforms the messages. E.x: handRaises history/log. Also, user previously raised the hand, left, and rejoined, the hand is automatically lowered.
     * - Retrieves info about message senders.  
     * Only after those steps are completed, sets the chat as 'ready'
     */
    setRoom = async (roomId, participants) => {
        this.setState({ isRoomLoading: true })

        let { senderDetails, handRaises, self } = this.state
        let stateMessages = [];
        let authors = [];

        let messages = await getAllMessagesAsync(this.webex, roomId)
        messages.sort((m1, m2) => new Date(m1.created) - new Date(m2.created))

        let senderIds = messages.map(m => m.personId).filter((pId, index, personIds) => personIds.indexOf(pId) === index)
        for(let i = 0; i < senderIds.length; i++)
        {
            let sender = senderDetails.filter(s => s.id === senderIds[i])[0]
            if(!sender) {
                sender = await getPersonByIdAsync(this.webex, senderIds[i])
            }
            authors.push(sender)
        }

        messages.forEach(m => {
            // If is custom, add/remove from handRaises[]
            // Here, we are automatically syncing handRaises, so that even if someone joins later during meeting, he knows who has raised his hand
            if(this.isCustomMessage(m.text)) {
                let customMsg = JSON.parse(m.text);
                if(customMsg.signal === CustomMessages.RaiseHand) {
                    // Only push if participant for whom the hand-raise is, is in-meeting
                    let i = participants?.findIndex(x => x.id === customMsg.forParticipantId && x.status === ParticipantStatus.InMeeting) ?? -1;
                    if(i > -1) 
                        handRaises.push({ participantId: customMsg.forParticipantId, timestamp: m.created });
                }
                else {
                    let i = handRaises.findIndex(x => x.participantId === customMsg.forParticipantId);
                    if(i > -1)
                        handRaises.splice(i, 1);
                }
            }
            // If is normal message, add to messages[]
            else {
                let senderName = authors.filter(a => a.id === m.personId)[0]?.displayName ?? m.personEmail
                let stateMessage = this.buildMessage(m.id, m.text, m.created, m.updated, { id: m.personId, name: senderName })
                stateMessages.push(stateMessage)
            }
        })

        this.setState({ roomId, isRoomLoading: false, isRoomReady: true, messages: stateMessages, senderDetails: authors, handRaises });

        // Initially, if my hand was raised before (I left with hand raised, and now I'm joining again), lower it.
        // We set state before this piece of code, because information like: 'handRaises', 'roomId' are required by `toggleHand` function to work
        let isHandRaised = handRaises.findIndex(x => x.participantId === self.id) !== -1;
        if(isHandRaised === true)
            this.toggleHand();
    }

    getRoomTitle = () => {
        let { meetingName, meetingNumber } = this.sdkMeeting.meetingInfo
        let title = `${meetingName} (${meetingNumber})`
        return title;
    }

    //#endregion

    //#endregion

    //#region ---------------------------------  Meeting events ---------------------------------

    bindMeetingEvents = async (meeting) => {
        console.log("-> Executing bindMeetingEvents");

        //
        meeting.on('all', (event, payload) => {
            console.log('** Meeting:all ** ', event, payload)
        })
        //
        meeting.on('error', (err) => {
            console.error('** Meeting:error ** ', err);
        })
        //
        meeting.on('DESTROY_MEETING', async (e) => {
            try {
                await this.leaveMeeting()
                this.setProgress(0, false, ExecutionStep.MeetingDestroyed)
                if(this.localVideo.current.isFullScreen() || this.remoteVideo.current.isFullScreen() || this.screenShare.current.isFullScreen())
                    document.exitFullscreen()
            }
            catch(e) {
                console.error(e)
            }
        })
        //
        meeting.on('media:ready', (media) => {
            if (!media) {
                return;
            }
            if (media.type === 'local' || media.type === 'Local') {
                this.localVideo.current.setVideoStream(media.stream)
            }
            if (media.type === 'remoteAudio') {
                this.remoteVideo.current.setAudioStream(media.stream)
            }
            if (media.type === 'remoteVideo') {
                this.remoteVideo.current.setVideoStream(media.stream)
            }
            if (media.type === 'localShare') {
                // this.screenShare.current.setVideoStream(media.stream)
                this.localShareStream = media.stream
            }
            if (media.type === 'remoteShare') {
                // this.screenShare.current.setVideoStream(media.stream)
                this.remoteShareStream = media.stream
            }
        })
        //
        meeting.on('media:stopped', (media) => {
            // Remove media streams
            if (media.type === 'local') {
                if(this.localVideo && this.localVideo.current)
                    this.localVideo.current.setVideoStream(null)
            }
            if (media.type === 'remoteAudio') {
                if(this.remoteVideo && this.remoteVideo.current)
                    this.remoteVideo.current.setAudioStream(null)
            }
            if (media.type === 'remoteVideo') {
                if(this.remoteVideo && this.remoteVideo.current)
                    this.remoteVideo.current.setVideoStream(null)
            }
            if (media.type === 'localShare') {
                if(this.screenShare && this.screenShare.current)
                    this.screenShare.current.setVideoStream(null)
            }
            if (media.type === 'remoteShare') {
                if(this.screenShare && this.screenShare.current)
                    this.screenShare.current.setVideoStream(null)
            }
        })
        //
        meeting.members.on('members:update', async (delta) => {
            let participants = [];
            let self = null;

            let { full } = delta;
            Object.keys(full).forEach((memberID) => {
                let participant = this.extractMemberInfo(full[memberID]);
                if(participant.isSelf)
                    self = participant;
                participants.push(participant);
            });
            console.log("Participants: ", participants);
            this.setState({ participants, self });
            await this.getPersonalInfo()

            let inMeeting = participants.filter(p => p.status === ParticipantStatus.InMeeting)
            let hostsInMeeting = inMeeting.filter(p => p.isHost)
            let notInMeeting = participants.filter(p => p.status !== ParticipantStatus.InMeeting)

            this.handleNoHost(inMeeting, self)
            await this.handleInMeeting(inMeeting, hostsInMeeting);
            await this.handleNotInMeeting(notInMeeting, hostsInMeeting);
        })
        //
        meeting.on('meeting:self:mutedByOthers', (e) => {
            notifyMutedByOthers()
        })
        //
        meeting.on('meeting:self:requestedToUnmute', (e) => {
            this.handleUnmuteRequest()
        })
        //
        //#region ScreenShare
        meeting.on('meeting:startedSharingLocal', () => {
            this.screenShare.current.setVideoStream(this.localShareStream)
            this.setState({ isLocalSharing: true })
            
            if(this.localVideo.current.isFullScreen() || this.remoteVideo.current.isFullScreen())
                document.exitFullscreen()
        })

        meeting.on('meeting:stoppedSharingLocal', () => {
            this.screenShare.current.setVideoStream(null)
            this.setState({ isLocalSharing: false })

            if(this.screenShare.current.isFullScreen())
                document.exitFullscreen()
        })

        meeting.on('meeting:startedSharingRemote', () => {
            this.screenShare.current.setVideoStream(this.remoteShareStream)
            this.setState({ isRemoteSharing: true })

            if(this.localVideo.current.isFullScreen() || this.remoteVideo.current.isFullScreen())
                document.exitFullscreen()
        })

        meeting.on('meeting:stoppedSharingRemote', () => {
            this.screenShare.current.setVideoStream(null)
            this.setState({ isRemoteSharing: false })

            if(this.screenShare.current.isFullScreen())
                document.exitFullscreen()
        })
        //#endregion
        //
        //#region Network
        meeting.on('network:disconnected', () => {
            this.setProgress(0, false, ExecutionStep.NetworkDisconnected)

            if(this.localVideo.current.isFullScreen() || this.remoteVideo.current.isFullScreen() || this.screenShare.current.isFullScreen())
                document.exitFullscreen()
        })

        meeting.on('meeting:reconnectionStarting', () => {
            this.setProgress(0, false, ExecutionStep.ReconnectionStarting)
        })

        meeting.on('meeting:reconnectionSuccess', () => {
            this.setProgress(500, false, ExecutionStep.ReconnectionSuccess)
            this.setProgress(0, true)
        })

        meeting.on('meeting:reconnectionFailure', (e) => {
            // this.setProgress(0, false, e.error.sdkMessage)
            this.setProgress(0, false, ExecutionStep.ReconnectionFailure)
        })

        meeting.on('network:connected', () => {
            this.setProgress(500, false, ExecutionStep.NetworkConnected)
            this.setProgress(0, true)
        })
        //#endregion
    }

    unbindMeetingEvents = (meeting) => {
        try {
            meeting.off('all')
            meeting.off('error')
            meeting.off('media:ready')
            meeting.off('media:stopped')
            meeting.off('meeting:startedSharingLocal')
            meeting.off('meeting:stoppedSharingLocal')
            meeting.off('meeting:startedSharingRemote')
            meeting.off('meeting:stoppedSharingRemote')
            meeting.off('meeting:self:mutedByOthers')
            meeting.off('meeting:self:requestedToUnmute')
            meeting.off('members:update')
        }
        catch(e) {
            console.error(e)
        }
    }

    extractMemberInfo = (member) => {
        let { id, name, isSelf, isHost, isModerator, isAudioMuted, isVideoMuted, isContentSharing, isRecording, isMutable, isRemovable, status } = member;
        let { id: personId } = member.participant.person;

        let participant = { id, personId, name, isSelf, isHost, isModerator, isAudioMuted, isVideoMuted, isContentSharing, isRecording, isMutable, isRemovable, status };

        if(participant.status === ParticipantStatus.InMeeting) {
            let { disallowUnmute, requestedToUnmute } = member.participant.controls.audio;
            participant.disallowUnmute = disallowUnmute;
            participant.requestedToUnmute = requestedToUnmute;
        }

        return participant;
    }

    getPersonalInfo = async () => {
        let { self, selfDetails } = this.state;

        if(self && !selfDetails) {
            selfDetails = await getPersonByIdAsync(this.webex, self.personId)
            if(selfDetails)
                this.setState({ selfDetails })
        }
    }

    handleNoHost = async (inMeeting, self) => {
        let hostsInMeeting = inMeeting.filter(p => p.isHost)

        if(hostsInMeeting.length === 0) {
            await this.leaveMeeting()
            // if(!self.isHost)
            //     this.setProgress(0, false, translateCap('LiveMeeting.notifications.noHost'))
        }
    }

    handleInMeeting = async (inMeeting, hostsInMeeting) => {
        let { roomId, memberships, handRaises } = this.state;
        
        let isSelfHost = (hostsInMeeting.filter(p => p.isSelf)[0]) ? true : false

        // Firsly, check if any member with his hand raised left. In such case, remove him from handRaises[]
        let inMeetingIds = inMeeting.map(im => im.id)
        handRaises.forEach((hr, index) => {
            if(!inMeetingIds.includes(hr.participantId)) {
                handRaises.splice(index, 1)
            }
        })

        for(let p of inMeeting)
        {
            let participantId = p.id, personId = p.personId, personName = p.name;

            try {
                // Host(s) responsible for creating memberships
                if(isSelfHost && !p.isSelf) {
                    let membershipExists = memberships.filter(m => m.participantId === participantId)[0]?.membership ? true : false
                    if(!membershipExists) {
                        let membership
                        // 1. Check in the room memberships list if there is
                        membership = (await this.webex.memberships.list({ roomId, personId })).items[0]
                        // 2. If isn't, create it
                        if(!membership) {
                            membership = await createMembershipAsync(this.webex, roomId, personId, false)
                        }
                        let membershipObj = { participantId, membership }
                        memberships.push(membershipObj)
                    }
                }
            } catch {}
        }

        this.setState({ handRaises, memberships })
    }

    handleNotInMeeting = async (notInMeeting, hostsInMeeting) => {
        let { roomId, participants, memberships, handRaises } = this.state;

        let isSelfHost = (hostsInMeeting.filter(p => p.isSelf)[0]) ? true : false
        
        for(let p of notInMeeting)
        {
            let participantId = p.id, personId = p.personId;

            // Remove from handRaises[] if is there
            let i = handRaises.findIndex(x => x.participantId === participantId);
            if(i > -1)
                handRaises.splice(i, 1);

            try {
                // Host(s) responsible for removing memberships
                if(isSelfHost && !p.isSelf ) {
                    let participantMembership = memberships.filter(m => m.participantId === participantId)[0]?.membership
                    if(participantMembership) {
                        try {
                            await removeMembershipAsync(this.webex, participantMembership);
                            // Remove from state too
                            let i = memberships.findIndex(m => m.participantId === participantId)
                            memberships.splice(i, 1)
                        } catch(e) {}
                    }
                }
            } catch(e) { }
        }

        this.setState({ handRaises, memberships })
    }

    //#endregion

    //#region ---------------------------------  Membership events ---------------------------------

    bindMembershipEvents = async () => {
        try {
            await this.webex.memberships.listen();
            console.log('-> Listening for memberships...');

            // Created event
            this.webex.memberships.on('created', async (event) => {
                try {
                    console.log('New membership');

                    if(this.state.roomId === -1) {
                        let room = await getRoomByIdAsync(this.webex, event.data.roomId);
                        if(room && room.title === this.getRoomTitle())
                            await this.setRoom(event.data.roomId)
                    }
                }
                catch(err) {
                    console.log('Error in membership:created ', err);
                }
            });

            // Deleted event
            this.webex.memberships.on('deleted', (event) => {
                try {
                    console.log('-> Deleted membership');

                    let { roomId, self, selfDetails } = this.state;
                    let { personId } = event.data;

                    // Make sure the person removed from the room is YOU, not someone else
                    let isMyMembership = (selfDetails && selfDetails.id === personId);
                    let isChatRoom = (roomId === event.data.roomId) ? true : false

                    if(roomId !== -1 && isMyMembership && isChatRoom) {
                        this.setState({ roomId: -1, isRoomReady: false });
                    }
                }
                catch(err) {
                    console.log('Error in membership:created ', err);
                }
                
            });
        }
        catch(err) {
            console.error('-> Unable to register for membership events: ', err);
        }
    }

    unbindMembershipEvents = async () => {
        try {
            this.webex.memberships.stopListening()
            this.webex.memberships.off('created')
            this.webex.memberships.off('updated')
            this.webex.memberships.off('deleted')
        }
        catch(e) {
            console.error(e)
        }
    }

    //#endregion

    //#region ---------------------------------  Message events ---------------------------------

    bindMessageEvents = async () => {
        try {
            await this.webex.messages.listen();
            console.log('-> Listening for messages...');

            //#region Listener: new/edited message
            this.webex.messages.on('created', async (event) => {
                
                let { roomId, messages, self, selfDetails, senderDetails, isChatOpen } = this.state;

                let isEdit = event.data.updated ? true : false;
                let { id: messageId, text, personId, personEmail, created, updated } = event.data;
                let senderId = event.actorId;
                // Optimization
                let sender = senderDetails.filter(s => s.id === personId)[0];
                if(!sender) {
                    sender = await getPersonByIdAsync(this.webex, personId);
                    senderDetails.push(sender);
                }
                let senderName = sender?.displayName ?? personEmail;
                        
                // Filter the messages related to the chat/room
                if(event.data.roomId === roomId) 
                {
                    let isMyMessage = (selfDetails && senderId === selfDetails.id)

                    if(this.isCustomMessage(text)) {
                        await this.handleCustomMessage(event, sender)
                    }
                    else 
                    {
                        if(text !== CustomMessages.RaiseHandLog)
                        {
                            let currentMessage = this.buildMessage(messageId, text, created, updated, { id: senderId, name: senderName });
        
                            //#region New message
                            if(!isEdit) {
                                // Check for duplicate
                                if(messages.findIndex(m => m.id === messageId) === -1) {
                                    messages.push(currentMessage);
                                    if(!isMyMessage && !isChatOpen)
                                        notifyNewMessage()
                                }
                            }
                            //#endregion
    
                            //#region Edited message
                            else {
                                // Custom message edited
                                if(text === CustomMessages.RaiseHandLog) {
                                    messages.push(currentMessage)
                                }
                                else {
                                    let i = messages.findIndex(m => m.id === messageId);
                                    if(i > -1)
                                        messages[i] = currentMessage;
                                }
                            }
                            //#endregion
                        }
    
                        this.setState({ messages, senderDetails });
                    }
                }
            });
            //#endregion

            //#region Listener: deleted message
            this.webex.messages.on('deleted', (event) => {
                console.log('-> Deleted message');

                let { roomId, messages } = this.state;

                // Filter the messages related to the chat/room
                if(event.data.roomId === roomId) {
                    let { id: messageId } = event.data;
                    let i = messages.findIndex(m => m.id === messageId);
                    if(i > -1) {
                        messages.splice(i, 1);
                        this.setState({ messages });
                    }
                }
            });
            //#endregion
        }
        catch(err) {
            console.error('-> Unable to register for message events: ', err);
        }
    }

    unbindMessageEvents = () => {
        try {
            this.webex.messages.stopListening()
            this.webex.messages.off('created')
            this.webex.messages.off('deleted')
        }
        catch(e) {
            console.error(e)
        }
    }

    buildMessage = (messageId, text, createdTime, updatedTime, sender) => {
        let author = {
            id: sender?.id ?? 0,
            // avatar: 'https://image.flaticon.com/icons/svg/2446/2446032.svg',
            username: sender?.name ?? 'Unknown'
        };
        let message = {
            id: messageId,
            author,
            text,
            type: 'text',
            timestamp: (new Date(createdTime)).getTime()
        }

        if(updatedTime)
            message.updated = (new Date(updatedTime)).getTime()

        return message;
    }

    isCustomMessage = (text) => {
        try {
            let message = JSON.parse(text);
            if(message.isCustomMessage)
                return true;
            return false;
        }
        catch(err) {
            return false;
        }
    }

    handleCustomMessage = async (event, sender) => {
        /** Workflow:
         * 1. If is a 'raise hand':
         *      - Only add to handRaises if nr. of current active hand-raises hasn't reach max. nr. of interventions
         *      - If is valid hand-raise and it's not yours, display notification
         * 2. If is a 'lower hand', remove from handRaises[] if is there
         */
        let { handRaises, selfDetails, participants } = this.state;

        let senderId = event.actorId;
        let senderName = sender?.displayName ?? event.data.personEmail

        let customMessage = JSON.parse(event.data.text);
        let { forParticipantId, signal } = customMessage;
        let isMyMessage = (selfDetails && selfDetails.id === senderId);

        let i;
        // 1.
        if(signal === CustomMessages.RaiseHand) {
            // A check for duplicate hand-raise
            i = handRaises.findIndex(x => x.participantId === forParticipantId) 
            if(i === -1)
                handRaises.push({ participantId: forParticipantId, timestamp: event.created })
            // Showing notifications since it's someone else's hand-raise
            if(!isMyMessage)
                notifyHandRaise(senderName)
        }
        else if(signal === CustomMessages.LowerHand) {
            i = handRaises.findIndex(x => x.participantId === forParticipantId)
            if(i > -1)
                handRaises.splice(i, 1)
        }
        // await this.clearCustomMessage(event); // Now we will store custom messages
        this.setState({ handRaises });
    }

    clearCustomMessage = async (event) => {
        let { selfDetails } = this.state;

        let { actorId: senderId } = event;
        let { id: messageId, text } = event.data;
        
        // Means this is MY custom message, so I'm responsible to delete it
        if(selfDetails && senderId === selfDetails.id)
            await removeMessageAsync(this.webex, messageId);
    }

    sendMessage = async (message) => {
        let { roomId, isRoomReady } = this.state;
        if(roomId !== -1 && isRoomReady)
            await createMessageAsync(this.webex, roomId, message);
    };

    //#endregion

    //#region ---------------------------------  My controls ------------------------------------
    
    toggleAudio = async () => {
        try {
            let sdkMeeting = this.sdkMeeting;
            let { self } = this.state;
            if(self) {
                if(self.isAudioMuted) {
                    if(self.disallowUnmute === false) {
                        await sdkMeeting.unmuteAudio();
                    }
                }
                else {
                    await sdkMeeting.muteAudio();
                }
            }
        } catch(e) {
            console.error(e);
        }
    }

    toggleVideo = async () => {
        try {
            let sdkMeeting = this.sdkMeeting;
            let { self } = this.state;
            if(self) {
                if(self.isVideoMuted)
                    await sdkMeeting.unmuteVideo();
                else
                    await sdkMeeting.muteVideo();
            }
        } catch(e) {
            console.error(e);
        }
    }

    toggleShareScreen = () => {
        try {
            let sdkMeeting = this.sdkMeeting;
            let { isLocalSharing } = this.state;
    
            if(isLocalSharing) {
                waitForMediaReady(sdkMeeting).then(() => {
                    sdkMeeting.stopShare();
                    this.toggleLocalSharingState();
                })
            }
            else {
                waitForMediaReady(sdkMeeting).then(() => {
                    sdkMeeting.shareScreen()
                        .then(() => {
                            console.info('LOCAL-SHARE-SCREEN: Screen successfully added to meeting.');
                            this.toggleLocalSharingState();
                        })
                        .catch((e) => {
                            console.error('LOCAL-SHARE-SCREEN: Unable to share screen, error:');
                            console.error(e);
                        });
                })
            }
        } catch(e) {
            console.error(e);
        }
    }

    toggleLocalSharingState = () => {
        let { isLocalSharing } = this.state;
        this.setState({ isLocalSharing: !isLocalSharing });
    }

    startRecording = async () => {
        try {
            let sdkMeeting = this.sdkMeeting;
            let { state } = sdkMeeting.recording;
    
            if(state === RecordingState.Idle) {
                await sdkMeeting.startRecording();
            }
        } catch(e) {
            console.error(e);
        }
    }

    pauseRecording = async () => {
        try {
            let sdkMeeting = this.sdkMeeting;
            let { state } = sdkMeeting.recording;
    
            if(state === RecordingState.Recording) {
                await sdkMeeting.pauseRecording();
            }
        } catch(e) {
            console.error(e);
        }
    }

    resumeRecording = async () => {
        try {
            let sdkMeeting = this.sdkMeeting;
            let { state } = sdkMeeting.recording;
    
            if(state === RecordingState.Paused) {
                await sdkMeeting.resumeRecording();
            }
        } catch(e) {
            console.error(e);
        }
    }

    stopRecording = async () => {
        try {
            let sdkMeeting = this.sdkMeeting;
            let { state } = sdkMeeting.recording;
    
            if(state === RecordingState.Recording || state === RecordingState.Paused) {
                await sdkMeeting.stopRecording();
            }
        } catch(e) {
            console.error(e);
        }
    }

    toggleHand = () => {
        let { self, handRaises, participants } = this.state;
        let text = '';
        let isHandRaised = handRaises.findIndex(x => x.participantId === self.id) !== -1;

        if(isHandRaised)
            text = JSON.stringify({ isCustomMessage: true, forParticipantId: self.id, signal: CustomMessages.LowerHand });
        else
            text = JSON.stringify({ isCustomMessage: true, forParticipantId: self.id, signal: CustomMessages.RaiseHand });
            
        // If it's a hand-raise, make sure maximum nr. of interventions isn't reached
        let activeHandRaises = 0;
        if(!isHandRaised)
            activeHandRaises = this.getActiveHandRaisesCount();

        try {
            // If it's a hand-raise, and max nr. of interventions is reached, show notification.
            if(!isHandRaised && activeHandRaises >= MaxNumberOfInterventions)
                notifyMaxNrOfInterventionsExceeded();
            else
                this.sendMessage(text);
        } catch(e) {
            console.error(e);
        }
    }

    toggleLocalVideoQuality = async (quality) => {
        let sdkMeeting = this.sdkMeeting;

        if(QualityLevels.includes(quality)) {
            try {
                await sdkMeeting.setLocalVideoQuality(quality);
                this.forceUpdate();
            } catch(e) {
                console.error(e);
            }
        }
    }

    toggleVideoLayout = async (layoutType) => {
        let sdkMeeting = this.sdkMeeting;

        if(LayoutTypes.includes(layoutType)) {
            try {
                await sdkMeeting.changeVideoLayout(layoutType);
            } catch(e) {

            }
        }
    }

    toggleRemoteVideoQuality = async (quality) => {
        let sdkMeeting = this.sdkMeeting;

        if(QualityLevels.includes(quality)) {
            try {
                let res = await sdkMeeting.setRemoteQualityLevel(quality);
            } catch(e) {
                console.error(e);
            }
        }
    }

    toggleLocalVideoVisibility = () => {
        this.setState({ isLocalVideoHidden: !this.state.isLocalVideoHidden })
    }

    downloadChat = async () => {
        this.setState({ isChatDownloading: true })
        
        let { roomId, senderDetails } = this.state
        let authors = []

        try {
            let messages = await getAllMessagesAsync(this.webex, roomId)
            messages.sort((m1, m2) => new Date(m1.created) - new Date(m2.created))
    
            let senderIds = messages.map(m => m.personId).filter((pId, index, personIds) => personIds.indexOf(pId) === index)
            for(let i = 0; i < senderIds.length; i++)
            {
                let sender = senderDetails.filter(s => s.id === senderIds[i])[0]
                if(!sender) {
                    sender = await getPersonByIdAsync(this.webex, senderIds[i])
                }
                authors.push(sender)
            }
    
            let fileName = `Chat - ${this.sdkMeeting.meetingInfo.meetingName}`
            let fileData = '', currentLine = ''
            messages.forEach(m => {
                let message;
                // If is custom message/'raise hand', change text else don't show at all
                if(this.isCustomMessage(m.text)) {
                    let customMsg = JSON.parse(m.text);
                    if(customMsg.signal === CustomMessages.RaiseHand)
                        message = CustomMessages.RaiseHandLog;
                    else
                        return;
                }
                else
                    message = m.text;

                let senderName = authors.filter(a => a.id === m.personId)[0]?.displayName ?? m.personEmail
                let messageTime = new Date(m.created)
                let timestamp = messageTime.toLocaleDateString() + " " + messageTime.toLocaleTimeString()
    
                currentLine = `${senderName} (${timestamp})\n${message}\n\n`
                fileData += currentLine
            });
    
            this.downloadTxtFile(fileName, fileData)
        }
        catch(e) {
            console.error(e);
        }
        finally {
            this.setState({ isChatDownloading: false })
        }
    }

    downloadTxtFile = (fileName, data) => {        
        let element = document.createElement("a");
        let file = new Blob([data], {type: 'text/plain'});
        element.href = URL.createObjectURL(file);
        element.download = `${fileName}.txt`;
        document.body.appendChild(element); // Required for this to work in FireFox
        element.click();
        element.parentNode.removeChild(element);
    }

    //#endregion

    //#region ---------------------------------  Participant controls ---------------------------------

    toggleParticipantAudio = async (participantId, muted) => {
        let participant = this.state.participants.filter(p => p.id === participantId)[0];
        if(participant) {
            try {
                participant.isSelf ?
                    await this.toggleAudio() :
                    await this.sdkMeeting.mute(participantId, muted);
            } catch(e) {
                console.error(e);
            }
        }
    }

    lowerParticipantHand = async (participantId) => {
        let text = JSON.stringify({ isCustomMessage: true, forParticipantId: participantId, signal: CustomMessages.LowerHand });
        try {
            await this.sendMessage(text);
        } catch(e) {
            console.error(e);
        }
    }

    muteAll = async () => {
        let { participants } = this.state;
        let guests = participants.filter(p => p.status === ParticipantStatus.InMeeting && !p.isModerator && !p.isHost && !p.isSelf);

        await Promise.all(
            guests.map(async p => {
                try {
                    await this.sdkMeeting.mute(p.id, true);
                } catch(e) {
                    console.error(e);
                }
            })
        );
    }

    unmuteAll = async () => {
        let { participants } = this.state;
        let guests = participants.filter(p => p.status === ParticipantStatus.InMeeting && !p.isModerator && !p.isHost && !p.isSelf);

        await Promise.all(
            guests.map(async p => {
                try {
                    await this.sdkMeeting.mute(p.id, false);
                } catch(e) {
                    console.error(e);
                }
            })
        );
    }

    removeParticipant = async (participantId) => {
        try {
            await this.sdkMeeting.remove(participantId);
        } catch(e) {
            console.err(e);
        }
    }

    makeHost = (participantId) => {
        try {
            this.sdkMeeting.transfer(participantId);
        } catch(e) {
            console.error(e);
        }
    }

    //#endregion

    //#region ---------------------------------  Leave/quit ---------------------------------

    confirmLeave = async () => {
        let { self } = this.state

        let confirmMsg = translateCap('LiveMeeting.leaveMeetingPrompt')
        if(self && self.isHost)
            confirmMsg = translateCap('LiveMeeting.leaveMeetingPrompt_host')

        confirmation(confirmMsg, async () => {
            await this.leaveMeeting()
        }, null)
    }

    leaveMeeting = async () => {
        /* 
            In meeting end, if you are the host/moderator:
            1. Stop registration (if registration still active)
            2. Kick everyone out
        */
        try {
            let { self, participants } = this.state
            let inMeeting = participants.filter(p => !p.isSelf && p.status === ParticipantStatus.InMeeting)
            
            if(self.isHost) {
                // 1.
                if(this.sdkMeeting.recording && this.sdkMeeting.recording.state !== RecordingState.Idle)
                    await this.stopRecording()

                // 2.
                await Promise.all(
                    inMeeting.map(async p => {
                        await this.removeParticipant(p.id);
                    })
                );
            }
        }
        catch(err) {
            console.error(err);
        }
        finally {
            await this.unSubscribeAndLeave()
        }
    }

    unSubscribeAndLeave = async () => {
        let { self } = this.state

        this.unbindMembershipEvents();
        this.unbindMessageEvents();

        let sdkMeeting = this.sdkMeeting;
        if(sdkMeeting && sdkMeeting.meetingState === MeetingState.Active ) {
            try {
                if(self.status === ParticipantStatus.InMeeting) {
                    await sdkMeeting.leave()
                }
            }
            catch(e) {
                console.error(e)
            }
            finally {
                this.unbindMeetingEvents(sdkMeeting)
                await unregisterAsync(this.webex);

                this.sdkMeeting = null;
                // Clear localStorage
                localStorage.removeItem(LocalStorageKeys.MeetingSipAddress);
                localStorage.removeItem(LocalStorageKeys.CiscoAuthToken);
                localStorage.removeItem(LocalStorageKeys.Language);
                localStorage.removeItem(LocalStorageKeys.JoinIntent);

                this.setState({ isBlocking: false })
                this.setProgress(0, false, ExecutionStep.MeetingDestroyed);
            }
        }
    }

    //#endregion

    //#region ---------------------------------  Utils ---------------------------------

    getActiveHandRaisesCount = () => {
        let { handRaises, participants } = this.state;

        let activeHandRaises = 0;
        let inMeeting = participants.filter(x => x.status === ParticipantStatus.InMeeting);
            
        handRaises.forEach(hr => {
            let i = inMeeting.findIndex(x => x.id === hr.participantId);
            if(i > -1)
                activeHandRaises++;
        });

        return activeHandRaises;
    }

    //#endregion

    handleLocalVideoDrag = () => {
        if(this.localVideo && this.localVideo.current)
        {
            this.localVideo.current.glimpseOverlay()
            
            if(this.localVideo.current.isFullScreen())
                return false;
            return true;
        }
        return false;
    }

    handleUnmuteRequest = () => {
        let { self } = this.state
        if(this.sdkMeeting && self.isAudioMuted)
            confirmation(translateCap('LiveMeeting.unmuteRequest'), () => {
                this.sdkMeeting.unmuteAudio();
            }, null);
    }

    getScreenshareTitle = () => {
        let { isLocalSharing, isRemoteSharing, participants } = this.state
        if(isLocalSharing || isRemoteSharing) {
            let title;
            let sharer = participants.filter(p => p.isContentSharing)[0]
            if(sharer) {
                return sharer.isSelf ?
                    translateCap('LiveMeeting.myScreen') :
                    translateCap('LiveMeeting.otherScreen', { name: sharer.name ?? 'guest' })
            }
        }
        return;
    }

    render() {
        let meeting = this.sdkMeeting;
        let { isBlocking, isMeetingReady, executionStep, participants, handRaises, self, selfDetails, isLocalSharing, isRemoteSharing, isParticipantsMenuOpen, isLocalVideoHidden, isChatOpen, isRoomLoading, isRoomReady, isChatDownloading, messages } = this.state;

        let selfId = selfDetails?.id ?? '0';
        let isHandRaised = handRaises.findIndex(x => x.participantId === (self?.id ?? '0')) !== -1;
        let activeHandRaises = this.getActiveHandRaisesCount();

        let recordingState = (meeting && meeting.recording) ? meeting.recording.state : RecordingState.Idle;

        let localVideoQuality = meeting?.mediaProperties.localQualityLevel ?? QualityLevel.Low;
        let remoteVideoQuality = meeting?.mediaProperties.remoteQualityLevel ?? QualityLevel.Low;

        let isContentSharing = (isLocalSharing || isRemoteSharing);
        let screenShareTitle = this.getScreenshareTitle();

        let layoutType = this.sdkMeeting?.lastVideoLayoutInfo?.layoutType;

        return (
            <>
                {
                    //#region Meeting overlay
                    isMeetingReady ? null :
                        <div id="meeting-initialization-overlay">
                            <div className="w-100 text-center absolute-center">
                                <i className="fas fa-info-circle"></i> {translateCap(`LiveMeeting.executionSteps.${executionStep}`)}
                                {
                                    (executionStep === ExecutionStep.MeetingDestroyed) ?
                                        <span className="d-block mt-2">
                                            <a href="/" className="btn btn-sm my-btn-primary shadow"> 
                                                <i class="fas fa-home"></i> {translateCap('general.goBack')} 
                                            </a>
                                        </span>
                                        : null
                                }
                            </div>
                        </div>
                    //#endregion
                }

                <Beforeunload onBeforeunload={this.onWindowClose} />

                <Prompt
                    when={isBlocking} 
                    message={location => {
                        let msg = translateCap('LiveMeeting.leaveMeetingPrompt')
                        if(self && self.isHost)
                            msg = translateCap('LiveMeeting.leaveMeetingPrompt_host')
                        return window.confirm(msg);
                    }}/>
                    
                <div id="meeting-streams" className="d-flex justify-content-center flex-wrap">

                    <Draggable bounds='parent' onStart={this.handleLocalVideoDrag}>
                        <div id="local-video" className={`shadow-lg ${isLocalVideoHidden ? 'd-none' : ''}`}>
                            <VideoPlayer
                                key='Local video'
                                ref={this.localVideo}
                                title={translateCap('LiveMeeting.localVideo')}
                            />
                        </div>
                    </Draggable>

                    <div id="remote-video" className={`${isContentSharing ? 'h-50' : 'h-100'}`}>
                        <VideoPlayer
                            key='Remote video'
                            ref={this.remoteVideo}
                            title={translateCap('LiveMeeting.remoteVideo')}
                            showControls={true}
                        />
                    </div>

                    <div id="screenshare-video" className={`${isContentSharing ? 'd-block' : 'd-none'}`}>
                        <VideoPlayer
                            key='Screenshare'
                            ref={this.screenShare}
                            title={screenShareTitle}
                        />
                    </div>
                        
                </div>

                {
                    //#region Bottom controls
                    !self ? null :
                        <>
                            <MeetingControls
                                self={self}
                                isRoomReady={isRoomReady}
                                recordingState={recordingState}
                                isLocalVideoHidden={isLocalVideoHidden}
                                onToggleLocalVideoVisibility={this.toggleLocalVideoVisibility}
                                isLocalSharing={isLocalSharing}
                                isHandRaised={isHandRaised}
                                handRaisesCount={activeHandRaises}
                                localVideoQuality={localVideoQuality}
                                remoteVideoQuality={remoteVideoQuality}
                                onChangeLocalVideoQuality={this.toggleLocalVideoQuality}
                                onChangeRemoteVideoQuality={this.toggleRemoteVideoQuality}
                                onChangeVideoLayout={this.toggleVideoLayout}
                                layoutType={layoutType}
                                onToggleAudio={this.toggleAudio}
                                onToggleVideo={this.toggleVideo}
                                onToggleShare={this.toggleShareScreen}
                                onStartRecording={this.startRecording}
                                onPauseRecording={this.pauseRecording}
                                onResumeRecording={this.resumeRecording}
                                onStopRecording={this.stopRecording}
                                onToggleHand={this.toggleHand}
                                onQuitMeeting={async () => { await this.confirmLeave() }}
                                onToggleParticipantsDrawer={() => this.setState({ isParticipantsMenuOpen: !isParticipantsMenuOpen })}
                                onToggleChatDrawer={() => this.setState({ isChatOpen: !isChatOpen })}
                            />

                            <Participants
                                id="participants-drawer"
                                isModerator={self.isModerator}
                                participants={participants}
                                handRaises={handRaises}
                                isOpen={isParticipantsMenuOpen}
                                onClose={() => this.setState({ isParticipantsMenuOpen: false })}
                                onMuteAll={this.muteAll}
                                onUnmuteAll={this.unmuteAll}
                                onLowerHand={this.lowerParticipantHand}
                                onToggleAudio={this.toggleParticipantAudio}
                                onRemove={this.removeParticipant}
                                onMakeHost={this.makeHost}
                            />

                            <Chat
                                id="chat-drawer"
                                userId={selfId}
                                self={self}
                                messages={messages}
                                fileSelectMode={FileSelectMode.Disabled}
                                placeholder={translateCap('LiveMeeting.Chat.writeMessage')}
                                timestampFormat="h:mm:ss a"
                                isOpen={isChatOpen}
                                isLoading={isRoomLoading}
                                isReady={isRoomReady}
                                isChatDownloading={isChatDownloading}
                                onClose={() => this.setState({ isChatOpen: false })}
                                onSendMessage={async (message) => await this.sendMessage(message)}
                                onDownloadChat={this.downloadChat}
                            />
                        </>
                    //#endregion
                }

                <RecordingIndicator recordingState={recordingState} />
            </>
        )
    }
}

export default withRouter(LiveMeeting);
