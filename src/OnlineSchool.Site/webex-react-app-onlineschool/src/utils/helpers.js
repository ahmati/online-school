import { confirmAlert } from 'react-confirm-alert';

/**
 * Checks if the input is a valid email.
 * @param  {string} email
 */
export const isValidEmail = (email) => {
    const re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

/**
 * Returns the difference between two DateTimes in minutes.
 * @param  {Date} d1 Date start.
 * @param  {Date} d2 Date end.
 * @return {number}  Difference in minutes.
 */
 export const getMinutesDiff = (d1, d2) => {
    return (d2-d1) / (1000 * 60);
}

/**
 * Wrapper of 'react-confirm-alert' for easier use.
 */
export const confirmation = (message, confirmCallback, denyCallback) => {
    confirmAlert({
        title: 'Conferma',
        message,
        buttons: [
            {
                label: 'Si',
                className: "confirm-yes",
                onClick: async () => {
                    await confirmCallback();
                }
            },
            {
                label: 'No',
                className: "confirm-no",
                onClick: async () => {
                    if(denyCallback)
                        await denyCallback();
                }
              }
        ]
    })
}

export const scrollToTop = () => {
    window.scrollTo(0, 0);
}

/**
 * Converts date to format: yyyy-MM-ddTHH:mm:ss
 */
export const convertToInputDateTimeFormat = (date) => {
    let day = date.getDate(),
        month = date.getMonth() + 1,
        year = date.getFullYear(),
        hour = date.getHours(),
        min  = date.getMinutes();

    month = (month < 10 ? "0" : "") + month;
    day = (day < 10 ? "0" : "") + day;
    hour = (hour < 10 ? "0" : "") + hour;
    min = (min < 10 ? "0" : "") + min;

    return `${year}-${month}-${day}T${hour}:${min}:00`;
}

/*** An awaitable sleep */
export const sleepAsync = ms => new Promise(r => setTimeout(r, ms))

export const capitalize = (s = '') => {
    if(typeof s !== 'string')
        return '';
    if(s.charAt(0))
        return s.charAt(0).toUpperCase() + s.slice(1)
}

/*** Returns local `MediaStream`, or `undefined` if any problem is encountered: for `e.x`: another app is using your camera etc. */
export const getLocalStream = async (constraints) => {
    let stream
    try {
        stream = await navigator.mediaDevices.getUserMedia(constraints);
        return stream;
    } 
    catch(e) {
        return null;
    }
}

export const isLocalStreamAvailable = async () => {
    let localStream = await getLocalStream({ audio: true, video: true })
    
    if(!localStream)
        return false;

    localStream.getTracks().forEach(track => track.stop())
    return true;
}

export const withExecutionTime = async (funcRef, ...args) => {
    if(funcRef && typeof funcRef === 'function') {
        console.time(funcRef.name);
        await funcRef(...args);
        console.timeEnd(funcRef.name);
    }
}

/*** Sorts an array of objects by a date property, given the property name */
export const sortObjArrayByDateProp_ASC = (arrayOfObjects=[], propertyName) => {
    arrayOfObjects.sort((a, b) => new Date(a[propertyName]) - new Date(b[propertyName]));
}