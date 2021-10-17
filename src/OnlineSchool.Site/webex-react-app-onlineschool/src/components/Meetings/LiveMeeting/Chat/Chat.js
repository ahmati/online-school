import React, { Component } from 'react';
import PropTypes from 'prop-types';
import './index.css';
import SlideDrawer from '../../../Layout/Shared/SlideDrawer/SlideDrawer';
import ChatBox from '@ergisgjergji/react-chat-plugin';
import { translate, translateCap } from '../../../../i18n/translate';
import { FileSelectMode, FileSelectModes } from '../../../../utils/constants';

class Chat extends Component {

    constructor() {
        super()
    }
    
    render() {
        // Fields
        let { id, userId, self, messages, fileSelectMode, placeholder, timestampFormat, showTypingIndicator, isOpen, isLoading, isReady, isChatDownloading } = this.props;
        // Callbacks
        let { onClose, onSendMessage, onDownloadChat } = this.props;

        return (
            <SlideDrawer id={id} isOpen={isOpen} onClose={onClose}>
            {
                !isReady ?
                    <small className="absolute-center"> 
                        <i className="fas fa-info-circle"></i> {translateCap('LiveMeeting.Chat.chatNotAvailable')}
                    </small> 
                    :
                    <>
                        <div className="slide-drawer-header">
                            <b> {translateCap('LiveMeeting.chat')} </b>
                            {
                                (self.isModerator && isLoading === false) ?
                                    <div className="d-inline-block float-right">
                                        <button id="downloadChatBtn" className="btn btn-sm my-btn-primary px-1 py-0" title={translateCap('LiveMeeting.Chat.downloadChat')} onClick={onDownloadChat}>
                                        {
                                            isChatDownloading ?
                                                <span className="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> :
                                                <i className="fas fa-download fa-lg"></i>
                                        }
                                        </button>
                                        <button className="btn btn-sm btn-default ml-1" title={translateCap('general.close')}>
                                            <i className="fas fa-times fa-lg text-danger" onClick={onClose}></i>
                                        </button>
                                    </div>
                                    : null
                            }
                        </div>

                        <div className="slide-drawer-body">
                            {
                                isLoading ?
                                    <div className="w-100 text-center absolute-center">
                                        <span className="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Loading...
                                    </div> :
                                    <ChatBox
                                        userId={userId}
                                        messages={messages}
                                        fileSelectMode={fileSelectMode}
                                        placeholder={placeholder}
                                        timestampFormat={timestampFormat}
                                        showTypingIndicator={showTypingIndicator}
                                        onSendMessage={onSendMessage}
                                    />
                            }
                        </div>
                    </>
            }
                </SlideDrawer>
        );
    }
}

Chat.propTypes = {
    id: PropTypes.string,
    userId: PropTypes.string,
    self: PropTypes.object.isRequired,
    messages: PropTypes.array.isRequired,
    fileSelectMode: PropTypes.oneOf(FileSelectModes),
    placeholder: PropTypes.string,
    timestampFormat: PropTypes.string,
    showTypingIndicator: PropTypes.bool,
    isOpen: PropTypes.bool.isRequired,
    isLoading: PropTypes.bool.isRequired,
    isReady: PropTypes.bool.isRequired,
    isChatDownloading: PropTypes.bool.isRequired,
    onClose: PropTypes.func.isRequired,
    onSendMessage: PropTypes.func.isRequired,
    onDownloadChat: PropTypes.func.isRequired
}

Chat.defaultProps  = {
    id: 'chat-default',
    userId: 0,
    self: { isHost: false, isModerator: false },
    messages: [],
    fileSelectMode: FileSelectMode.Disabled,
    placeholder: '',
    timestampFormat: 'calendar',
    showTypingIndicator: false,
    isOpen: false,
    isLoading: false,
    isReady: false,
    isChatDownloading: false,
    onClose: () => console.log('onClose'),
    onSendMessage: () => console.log('onSendMessage'),
    onDownloadChat: () => console.log('onDownloadChat')
}

export default Chat;