import { Flip, toast } from "react-toastify"
import { translateCap } from "../../../../i18n/translate";

export const notifyHandRaise = (handRaiser) => {
    toast(`✋ ${handRaiser}`, {
        transition: Flip,
        position: 'top-right',
        autoClose: 3000
    });
}

export const notifyNewMessage = () => {
    toast(`✉️ ${translateCap('notifications.newMessage')}`, {
        transition: Flip,
        position: 'top-right',
        autoClose: 3000,
        toastId: 'fcd8f4b2-0c05-4c2b-b87d-5ed7d29e5da0'
    });
}

export const notifyMutedByOthers = () => {
    toast(`🔇 ${translateCap('notifications.mutedByOthers')}`, {
        transition: Flip,
        position: 'top-right',
        autoClose: 3000,
        toastId: 'fcd8f4b2-0c05-4c2b-b87d-5ed7d29e5da0'
    });
}

export const notifyMeetingDestroyed = () => {
    toast.warning(translateCap('notifications.meetingDestroyed'), {
        transition: Flip,
        position: 'top-right',
        autoClose: 5000,
        toastId: '0edd9dea-62d4-479f-beda-5b8e8b2c7077'
    });
}

export const notifyNoHost = () => {
    toast.warning(translateCap('notifications.noHost'), {
        transition: Flip,
        position: 'top-right',
        autoClose: 5000,
        toastId: 'a73045c0-731e-4620-bf8d-5010e25763f2'
    });
}

export const notifyMaxNrOfInterventionsExceeded = () => {
    toast.warning(translateCap('notifications.maxHandRaiseReached'), {
        transition: Flip,
        position: 'top-right',
        autoClose: 3000,
        toastId: '4bc3761a-396c-4490-9258-b4999d8afe6c'
    });
}