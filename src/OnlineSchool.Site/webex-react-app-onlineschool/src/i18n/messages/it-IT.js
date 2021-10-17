import { LOCALES } from '../locales';

const it = {
    [LOCALES.ITALIAN]: {
        'general': {
            'changeLanguage': 'cambia lingua',
            'actions': 'azioni',
            'close': 'chiudi',
            'enterFullscreen': 'vai fullscreen',
            'exitFullscreen': 'esci fullscreen',
            'empty': 'vuoto',
            'goBack': 'ritorno',
            'minutes': 'minuti',
            'remove': 'rimuovi',
            'save': 'salva',
            'search': 'ricerca',
            'showMore': 'mostra di più',
            'showLess': 'mostra meno'
        },
        'LiveMeeting': {
            'mute': 'mute',
            'unmute': 'unmute',
            'participants': 'participanti',
            'chat': 'chat',
            'raiseHand': 'alza la mano',
            'lowerHand': 'abbassa la mano',
            'executionSteps': {
                'localStreamProblem': "Impossibile verificare la disponibilità di audio e video del tuo computer. Assicurati del funzionamento audio/video sul tuo computer e che questi non siano attualmente utilizzati da un'altra app.",
                'loading': 'caricamento in corso...',
                'initializing': 'inizializzazione istanza webex...',
                'registering': 'registrazione dispositivo...',
                'joining': 'ingresso meeting...',
                'meetingReady': 'meeting pronto',
                'meetingDestroyed': 'il meeting è concluso / non sei più nel meeting.',
                'failed': 'qualcosa è andata male ✘',
                'missingInformation': 'internal server error! Mancano alcune informazioni! ✘',
                'networkDisconnected': 'Rete internet disconnessa',
                'reconnectionStarting': 'Tentativo di riconnessione...',
                'reconnectionSuccess': 'Riconnessione con successo',
                'reconnectionFailure': 'Riconnessione fallita. Prova a rientrare nel meeting di nuovo.',
                'networkConnected': 'Rete riconnessa'
            },
            'myScreen': 'il mio video',
            'otherScreen': "video di {name}",
            'unmuteRequest': "ti viene richiesto di riattivare l'audio. Riattivare?",
            'leaveMeetingPrompt': 'sei sicuro di voler lasciare il meeting?',
            'leaveMeetingPrompt_host': 'stai per terminare il meeting per tutti. Continuare?',
            'localVideo': 'video locale',
            'remoteVideo': 'video remoto',
            'recording': 'registrazione',
            'recordingPaused': 'registrazione in pausa',
            'maxNumberOfInterventions': 'il numero massimo di interventi è 10',
            'MeetingControls': {
                'controls': 'controls',
                'toggleAudio': 'attiva/disattiva audio',
                'muteAudio': 'mute audio',
                'unmuteAudio': 'unmute audio',
                'unmuteDisallowed': 'il tuo audio è disattivato',
                'toggleVideo': 'attiva/disattiva video',
                'intervention': 'intervento',
                'muteVideo': 'disattiva video',
                'unmuteVideo': 'attiva video',
                'toggleScreenShare': 'attiva/disattiva condivisione video',
                'shareScreen': 'condividi video',
                'stopShare': 'Stop condivisione',
                'leaveMeeting': 'abbandona il meeting',
                'endMeeting': 'fine meeting'
            },
            'OtherOptionsButton': {
                'moreOptions': 'altre opzioni',
                'layout': 'lo schema',
                'displayMyVideo': 'mostra il mio video',
                'hideMyVideo': 'nascondi il mio video',
                'startRecording': 'avvia registrazione',
                'pauseRecording': 'pause registrazione',
                'resumeRecording': 'riprendi registrazione',
                'stopRecording': 'stop registrazione',
                'videoQuality': 'video quality',
                'remoteVideoQuality': "incoming video quality",
                'low': 'low',
                'medium': 'medium',
                'high': 'high',
                'videoLayout': 'layout del video',
                'single': 'singolo',
                'equal': 'uguale',
                'activePresence': 'presenza attiva',
                'prominent': 'prominente'
            },
            'Participants': {
                'muteAll': 'mute all',
                'unmuteAll': 'unmute all',
                'raisedHands': 'mani alzate',
                'hosts': 'hosts',
                'moderators': 'moderatori',
                'guests': 'ospiti'
            },
            'Participant': {
                'host': 'host',
                'me': 'me',
                'expel': 'espelli'
            },
            'Chat': {
                'chatNotAvailable': 'la chat non è disponibile adesso.',
                'downloadChat': 'scarica chat',
                'writeMessage': 'scrivi un messaggio'
            }
        },
        'MeetingForm': {
            'missingAuthToken': "manca il token di autorizzazione",
            'createMeeting': "nuovo riunione programmato",
            'updateMeeting': "aggiorna le informazioni sulla riunione",
            'advancedOptions': "opzioni avanzate",
            'noInvitees': "nessun invitato",
            'title': 'titolo',
            'agenda': 'agenda',
            'password': 'password',
            'start': 'ora di inizio',
            'end': 'tempo di fine',
            'timezone': 'timezone',
            'sendEmail': 'invia una email',
            'allowAnyUserToBeCoHost': "consentire a qualsiasi utente di essere co-host",
            'enabledAutoRecordMeeting': "avvio automatico della registrazione",
            'enabledJoinBeforeHost': "abilita l'accesso prima dell'host"
        },
        'Invitees': {
            'newInvitation': 'nuovo invito',
            'noInvitees': 'nessun invitato',
            'invitees': 'invitati',
            'invitee': 'invitato',
            'email': 'e-mail',
            'name': 'nome',
            'coHost': 'co-host',
            'invite': 'invitare'
        },
        'confirmations': {
            'removeInvitee': "sei sicuro di voler rimuovere '{email}' dagli invitati?"
        },
        'notifications': {
            'meetingCreated': 'la riunione è stata creata con successo',
            'meetingUpdated': 'la riunione è stata aggiornata con successo',
            'newMessage': "nuovi messaggi",
            'mutedByOthers': "il tuo audio è stato disattivato",
            'meetingDestroyed': "il meeting è concluso / non sei più nel meeting.",
            'noHost': "meeting è terminato a causa della mancanza di host online (motivo: l'host potrebbe essersi disconnesso)",
            'maxHandRaiseReached': "il numero massimo di interventi è 10"
        },
        'validations': {
            'invalidEmail': "l'email non è valida",
            'meeting': {
                'titleEmpty': 'il titolo è obbligatorio',
                'startEmpty': "l'ora di inizio è obbligatoria",
                'endEmpty': "l'ora di fine è obbligatoria",
                'startTimeBeforeCurrentTime': "l'ora di inizio non può essere precedente all'ora corrente",
                'endTimeBeforeCurrentTime': "l'ora di fine non può essere precedente all'ora corrente",
                'endTimeBeforeStartTime': "l'ora di fine non può essere precedente all'ora di inizio",
                'invalidDuration': "la durata della riunione non può essere inferiore a 10 minuti / superiore a 24 ore",
                'joinBeforeHostMinutes': "minuti \"partecipa prima dell'host\" non validi"
            }
        }
    }
}

export default it;