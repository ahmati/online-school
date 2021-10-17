import webex from 'webex';
import { token, guestIssuerSecret, guestIssuerId } from './secrets';
import jwt from 'jsonwebtoken';
import { JoinIntent, JoinIntents, QualityLevel, QualityLevels } from './constants';

export const initWebexAsync = async (jwtToken) => {
    console.info("-> Executing webexInit");
    try {
        let wb = await webex.init({
            config: {
            meetings: {
                reconnection: {
                    enabled: true
                }
            }
            // Any other sdk config we need
            },
            credentials: {
                access_token: jwtToken
            }
        });
        return wb;
    }
    catch(err) {
        console.error("~ Error in webexInitAsync(): ", err);
    }
}

// ---------------------------------------------- Authentication ------------------------------------------------------------

export const getToken = () => {
    return `Bearer ${token}`;
}

/**
 * Guest Issuer: issues a guest token that expires in 24 hours
 */
export const issueGuestToken = (subject, name) => {
    console.log('-> Executing issueGuestToken');
    let payload = {
        sub: subject,
        name,
        iss: guestIssuerId
    };

    var token = jwt.sign(
        payload,
        Buffer.from(guestIssuerSecret, 'base64'),
        { expiresIn: '48h' }
    );

    return token;
}

/**
 * Registers/authenticates the guest from the guest token
 * @return {number}  Guest's access token (expires in 24 hours)
 */
export const registerGuestAsync = async (guestToken) => {
    console.log('-> Executing registerGuestAsync');

    if(guestToken) {
        try {
            let res = await fetch('https://webexapis.com/v1/jwt/login', {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${guestToken}`
                }
            });
            let data = await res.json();
            return data.token;
        }
        catch(err) {
            return;
        }
    }
    return;
}

export const initializeGuest = async (subject, name) => {
    let guestToken = issueGuestToken(subject, name);
    let guestAccessToken = await registerGuestAsync(guestToken);
    let guestDetails = await getGuestDetails(guestAccessToken);
    return guestDetails;
}

// ---------------------------------------------- Meetings ------------------------------------------------------------


export const registerAsync = async (wb) => {
    console.info("-> Executing registerAsync");
    if (!wb.meetings.registered) {
        try {
            await wb.meetings.register();
            // Sync our meetings with existing meetings on the server
            await wb.meetings.syncMeetings();
        }
        catch(err) {
            console.error("~ Error in register(): ", err);
        }
    }
}

export const unregisterAsync = async (wb) => {
    console.info("-> Executing unregisterAsync");
    try {
        if (wb.meetings.registered)
            await wb.meetings.unregister();
    }
    catch(err) {
        console.error("~ Error in unregisterAsync(): ", err);
    }
}

export const createMeetingAsync = async (wb, destination) => {
    console.info("-> Executing createMeetingAsync");
    try {
        let meeting = await wb.meetings.create(destination);
        return meeting;
    }
    catch(err) {
        console.error("~ Error in unregisterAsync(): ", err);
    }
}

/** Join the meeting with a specific video quality (in this case 'LOW') and the video initially muted. This method's workflow is:
 * @param {Object} meeting The meeting object that is returned when you call: *webex.meetings.create(...)*
 * @param {string} pin Meeting's *password*
 * @param {'LOW'|'MEDIUM'|'HIGH'} localVideoQuality Defines local video quality. Default: **LOW**
 * 1. Join the meeting
 * 2. Add local media streams to the meeting
 * 3. Set the local video quality to 'LOW'
 * 4. Mute the video
 */
export const joinMeetingWithMediaAsync = async (meeting, pin, localVideoQuality = QualityLevel.Low) => {
    console.info("-> Executing joinMeeting");
    
    try {
        // If `pin` is present, means the account trying to join is a `guest` account
        if(pin)
            await meeting.join({ moderator: false, pin });
        else
            await meeting.join();

        return addMediaToMeeting(meeting, localVideoQuality);
    }
    catch(err) {
        console.error(err);
    }
}

export const addMediaToMeeting = (meeting, localVideoQuality = QualityLevel.Low) => {
    return meeting.getSupportedDevices({sendAudio: true, sendVideo: true}).then(({sendAudio, sendVideo}) => {
        let mediaSettings = {
            receiveVideo: true,
            receiveAudio: true,
            receiveShare: true,
            sendShare: false,
            sendVideo,
            sendAudio
        };

        meeting.getMediaStreams(mediaSettings).then((mediaStreams) => {
            let [localStream, localShare] = mediaStreams;
            meeting.addMedia({ localShare, localStream, mediaSettings }).then(() => {
                meeting.setLocalVideoQuality(localVideoQuality).then(() => {
                    meeting.muteVideo()
                })
            })
        });
    });
}

// Waits for the meeting to be media update ready
export const waitForMediaReady = (meeting) => {
    return new Promise((resolve, reject) => {
      if (meeting.canUpdateMedia()) {
        resolve();
      }
      else {
        console.info('SHARE-SCREEN: Unable to update media, pausing to retry...');
        let retryAttempts = 0;
  
        const retryInterval = setInterval(() => {
            retryAttempts += 1;
            console.info('SHARE-SCREEN: Retry update media check');
    
            if (meeting.canUpdateMedia()) {
                console.info('SHARE-SCREEN: Able to update media, continuing');
                clearInterval(retryInterval);
                resolve();
            }
            // If we can't update our media after 15 seconds, something went wrong
            else if (retryAttempts > 15) {
                console.error('SHARE-SCREEN: Unable to share screen, media was not able to update.');
                clearInterval(retryInterval);
                reject();
            }
        }, 7000);
      }
    });
}

// ---------------------------------------------- Rooms ------------------------------------------------------------

export const createRoomAsync = async (wb, title) => {
    console.info("-> Executing createRoomAsync");
    try {
        let room = await wb.rooms.create({ title });
        return room;
    }
    catch(err) {
        console.log(err);
        return;
    }
}

export const getRoomsAsync = async (wb) => {
    try {
        let rooms = await wb.rooms.list();
        return rooms.items;
    }
    catch(err) {
        console.log('Unable to get rooms: ', err);
        return;
    }
}

export const getRoomByTitleAsync = async (wb, title) => {
    let rooms = await getRoomsAsync(wb);
    if(rooms)
        return rooms.filter(r => r.title === title)[0];
    return;
}

export const getRoomByIdAsync = async (wb, roomId) => {
    try {
        let room = await wb.rooms.get(roomId);
        if(room)
            return room;
    }
    catch(err) {
        return;
    }
}

// ---------------------------------------------- Memberships ------------------------------------------------------

export const getAllMembershipsAsync = async (wb, roomId) => {
    let roomMemberships = await wb.memberships.list({ roomId });
    return roomMemberships.items;
}

export const getMembershipAsync = async (wb, roomId, personId) => {
    let memberships = await wb.memberships.list({ roomId });
    let membership = memberships.items.filter(m => m.personId === personId)[0];
    return membership;
}

export const membershipExists = async (wb, roomId, personId) => {
    let membership = await getMembershipAsync(wb, roomId, personId);
    if(membership)
        return true;
    return false;
}

export const createMembershipAsync = async (wb, roomId, personId, isRoomModerator=false) => {
    try {
        let exists = await membershipExists(wb, roomId, personId);
        if(exists === false) {
            return await wb.memberships.create({ roomId, personId, isModerator: isRoomModerator });
        }
        return;
    }
    catch(err) {
        return;
    }
}

export const updateMembershipAsync = async (wb, updatedMembership) => {
    return await wb.memberships.update(updatedMembership);
}

// export const removeMembershipAsync = async (wb, roomId, personId) => {
//     let membership = await getMembershipAsync(wb, roomId, personId);
//     if(membership)
//         await wb.memberships.remove(membership);
// }

export const removeMembershipAsync = async (wb, membership) => {
    return wb.memberships.remove(membership);
}

// ---------------------------------------------- Messages ---------------------------------------------------------

export const getAllMessagesAsync = async (wb, roomId) => {
    let messages = await getMessagesRecursively(wb, roomId, 1000)
    return messages;
}

export const getMessagesRecursively = async (wb, roomId, max = 1000) => {
    let messages = (await wb.messages.list({ roomId, max })).items
    if(messages.length === max)
        return getMessagesRecursively(wb, roomId, max + 1000);
    else
        return messages;
}

export const createMessageAsync = async (wb, roomId, text) => {
    let message = await wb.messages.create({ roomId, text });
    return message;
}

export const updateMessageAsync = async (wb, id, text) => {
    try {
        let message = await wb.messages.create({ id, text });
        return message;
    } catch(e) {
        return;
    }
}

export const removeMessageAsync = async (wb, messageId) => {
    await wb.messages.remove(messageId);
}

// ---------------------------------------------- People ---------------------------------------------------------

export const getPersonByIdAsync = async (wb, personId) => {
    try {
        let person = await wb.people.get( personId );
        return person;
    }
    catch(err) {
        return;
    }
}

export const personIdExistsAsync = async (wb, personId) => {
    try {
        let person = await getPersonByIdAsync(wb, personId);
        if(person)
            return true;
        return false;
    }
    catch(err) {
        console.log('~ personIdExistsAsync: ', err);
    }
}

// ---------------------------------------------- Guest operations ---------------------------------------------------------

export const getGuestDetails = async (guestAccessToken) => {
    console.log('-> Executing getGuestDetails');
    
    if(guestAccessToken) {
        try {
        let res = await fetch('https://webexapis.com/v1/people/me', {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${guestAccessToken}`
                }
            });
            let data = await res.json();
            return data;
        }
        catch(err) {
            return;
        }
    }
    return;
}