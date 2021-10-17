import { v4 as uuidv4 } from 'uuid';

export const samples = [
    {
        disallowUnmute: false,
        id: uuidv4(),
        isAudioMuted: false,
        isContentSharing: false,
        isHost: true,
        isModerator: true,
        isMutable: true,
        isRecording: false,
        isRemovable: false,
        isSelf: true,
        isVideoMuted: false,
        name: Math.random().toString(36).replace(/[^a-z]+/g, ''),
        personId: uuidv4(),
        requestedToUnmute: false,
        status: "IN_MEETING"
    },
    {
        disallowUnmute: false,
        id: uuidv4(),
        isAudioMuted: false,
        isContentSharing: false,
        isHost: false,
        isModerator: false,
        isMutable: true,
        isRecording: false,
        isRemovable: false,
        isSelf: false,
        isVideoMuted: false,
        name: Math.random().toString(36).replace(/[^a-z]+/g, ''),
        personId: uuidv4(),
        requestedToUnmute: false,
        status: "IN_MEETING"
    },
    {
        disallowUnmute: false,
        id: uuidv4(),
        isAudioMuted: true,
        isContentSharing: false,
        isHost: false,
        isModerator: false,
        isMutable: true,
        isRecording: false,
        isRemovable: false,
        isSelf: true,
        isVideoMuted: false,
        name: Math.random().toString(36).replace(/[^a-z]+/g, ''),
        personId: uuidv4(),
        requestedToUnmute: false,
        status: "IN_MEETING"
    },
    {
        disallowUnmute: false,
        id: uuidv4(),
        isAudioMuted: true,
        isContentSharing: false,
        isHost: false,
        isModerator: false,
        isMutable: true,
        isRecording: false,
        isRemovable: false,
        isSelf: false,
        isVideoMuted: true,
        name: Math.random().toString(36).replace(/[^a-z]+/g, ''),
        personId: uuidv4(),
        requestedToUnmute: false,
        status: "IN_MEETING"
    },
    {
        disallowUnmute: false,
        id: uuidv4(),
        isAudioMuted: false,
        isContentSharing: false,
        isHost: false,
        isModerator: false,
        isMutable: true,
        isRecording: true,
        isRemovable: false,
        isSelf: false,
        isVideoMuted: true,
        name: Math.random().toString(36).replace(/[^a-z]+/g, ''),
        personId: uuidv4(),
        requestedToUnmute: false,
        status: "IN_MEETING"
    }
]

/**
 * Get a random integer between 2 values.
 * @param {Number} min Minimum (inclusive)
 * @param {Number} max Maximum (exclusive)
*/
export const getRandomInt = (min, max) => {
    min = Math.ceil(min);
    max = Math.floor(max);
    return Math.floor(Math.random() * (max - min) + min); //The maximum is exclusive and the minimum is inclusive
}

export const getRandomData = (length=2000) => {
    let data = []
    for(let i = 0; i < length; i++)
    {
        let current = samples[getRandomInt(0, samples.length)]
        data.push(current)
    }
    return data
}