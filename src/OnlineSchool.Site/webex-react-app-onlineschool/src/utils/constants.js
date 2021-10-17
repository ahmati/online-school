export const LocalStorageKeys = {
    Language: 'webex-app-language',
    MeetingSipAddress: 'meetingSipAddress',
    MeetingPassword: 'meetingPassword',
    CiscoAuthToken: 'ciscoAuthToken'
};

export const FileSelectMode = {
    Single: 'SINGLE',
    Multiple: 'MULTIPLE',
    Disabled: 'DISABLED'
};
export const FileSelectModes = [FileSelectMode.Single, FileSelectMode.Multiple, FileSelectMode.Disabled];

export const join_before_host_minutes = [0, 5, 10, 15];

export const ExecutionStep = {
    LocalStreamProblem: 'localStreamProblem',
    Loading: 'loading',
    Initializing: 'initializing',
    Registering: 'registering',
    Joining: 'joining',
    MeetingReady: 'meetingReady',
    MeetingDestroyed: 'meetingDestroyed',
    Failed: 'failed',
    MissingInformation: 'missingInformation',
    NetworkDisconnected: 'networkDisconnected',
    ReconnectionStarting: 'reconnectionStarting',
    ReconnectionSuccess: 'reconnectionSuccess',
    ReconnectionFailure: 'reconnectionFailure',
    NetworkConnected: 'networkConnected'
};

export const ParticipantStatus = {
    NotInMeeting: 'NOT_IN_MEETING',
    InLobby: 'IN_LOBBY',
    InMeeting: 'IN_MEETING'
};
export const ParticipantStatuses = [ParticipantStatus.NotInMeeting, ParticipantStatus.InLobby, ParticipantStatus.InMeeting];

export const MeetingState = {
    Active: 'ACTIVE',
    NotActive: 'INACTIVE'
};
export const MeetingStates = [MeetingState.Active, MeetingState.NotActive];

export const RecordingState = {
    Idle: 'idle',
    Paused: 'paused',
    Recording: 'recording'
};
export const RecordingStates = [RecordingState.Idle, RecordingState.Paused, RecordingState.Recording];

export const CustomMessages = {
    RaiseHand: '50585002-2564-4451-a2bc-4475db4b7fbe',
    LowerHand: '92aae601-1fc5-414e-88cc-3e06f4b343c8',
    RaiseHandLog: '*** alzato la mano ***'
};

export const QualityLevel = {
    Low: 'LOW',
    Medium: 'MEDIUM',
    High: 'HIGH',
};
export const QualityLevels = [QualityLevel.Low, QualityLevel.Medium, QualityLevel.High];

export const LayoutType = {
    Single: 'Single',
    Equal: 'Equal',
    ActivePresence: 'ActivePresence',
    Prominent: 'Prominent',
    OnePlusN: 'OnePlusN'
}
export const LayoutTypes = ['Single', 'Equal', 'ActivePresence', 'Prominent', 'OnePlusN'];

export const MaxNumberOfInterventions = 10;