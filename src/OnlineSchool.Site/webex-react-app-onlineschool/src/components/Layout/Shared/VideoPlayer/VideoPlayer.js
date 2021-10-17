import React, { Component } from 'react';
import PropTypes from 'prop-types';
import './index.css';
import { translate, translateCap } from '../../../../i18n/translate';
import { RecordingState, RecordingStates } from '../../../../utils/constants';

class VideoPlayer extends Component {

    constructor() {
        super()
        this.mouseTimeout = null
        this.videoContainer = React.createRef()
        this.video = React.createRef()
        this.audio = React.createRef()
        this.state = {
            showOverlay: true,
            isFullscreen: false,
            volume: 100
        }
    }

    componentDidMount = () => {

        this.videoContainer.current.addEventListener('mousemove', e => {
            this.glimpseOverlay()
        })

        this.videoContainer.current.addEventListener('fullscreenchange', e => {
            if(document.fullscreenElement) {
                this.setState({ isFullscreen: true })
            }
            else {
                this.setState({ isFullscreen: false })
            }
        })
    }

    componentWillUnmount = () => {
        this.videoContainer.current.removeEventListener('mousemove', null)
        this.videoContainer.current.removeEventListener('fullscreenchange', null)
    }

    setVideoStream = (stream) => {
        this.video.current.srcObject = stream
    }

    setAudioStream = (stream) => {
        this.audio.current.srcObject = stream
    }

    isFullScreen = () => {
        return this.state.isFullscreen ?? false
    }

    toggleFullscreen = () => {
        let { isFullscreen } = this.state

        if(!isFullscreen)
            this.videoContainer.current.requestFullscreen()
        else
            document.exitFullscreen()
    }

    toggleAudio = () => {
        let { volume } = this.state;
        (volume > 0)
            ? this.setVolume(0)
            : this.setVolume(100);
    }

    toggleVolume = (e) => {
        this.setVolume(e.target.value);
    }

    // Volume between 0-100
    setVolume = (volume) => {
        this.audio.current.volume = volume / 100;
        this.setState({ volume });
    }

    glimpseOverlay = () => {
        this.setState({ showOverlay: true });
        clearTimeout(this.mouseTimeout);
        this.mouseTimeout = setTimeout(() => this.setState({ showOverlay: false }), 2000);
    }

    render() {
        let { title, showControls } = this.props;
        let { showOverlay, isFullscreen, volume } = this.state;

        // let hasVideo = false
        // if(this.video && this.video.current && this.video.current.srcObject)
        //     hasVideo = this.video.current.srcObject.getVideoTracks()[0].enabled

        return (
            <div ref={this.videoContainer} className="video-player" onDoubleClick={() => this.toggleFullscreen()}>
                {
                    !showOverlay ? null :
                        <div className="video-player-overlay">
                            {
                                title ?
                                    <small className="video-title px-1 bg-white text-dark rounded shadow-lg"> 
                                        <i className="fas fa-info fa-sm"></i> {title} 
                                    </small>
                                    : null
                            }
                            <div className="video-controls shadow">
                            {
                                showControls ?
                                    <>
                                        <button className="btn btn-sm border" onClick={this.toggleAudio}>
                                            <i className={`fas ${(volume > 0) ? 'fa-volume-up' : 'fa-volume-mute'}`}></i>
                                        </button>
                                        <input 
                                            id="audioSlider" 
                                            type="range"
                                            className="mr-1"
                                            min="0" 
                                            max="100" 
                                            step="1" 
                                            value={volume} 
                                            onChange={this.toggleVolume}/>
                                    </>
                                    : null
                            }
                                <button 
                                    className="btn btn-default btn-sm video-fullscreen border" 
                                    title={isFullscreen ? translateCap('general.exitFullscreen') : translateCap('general.enterFullscreen')} 
                                    onClick={this.toggleFullscreen}>
                                {
                                    isFullscreen ?
                                        <i className="fas fa-compress-alt"></i> :
                                        <i className="fas fa-expand-alt"></i>
                                }
                                </button>
                            </div>
                        </div>
                }
                {
                    // hasVideo ? null :
                    //     <small className="absolute-center bg-white text-dark px-1 rounded">
                    //         <i className="fas fa-info fa-sm"></i> No video {/* Internationalize */}
                    //     </small>
                }

                <video muted ref={this.video} autoPlay={true} playsInline={true} />
                <audio ref={this.audio} autoPlay={true} playsInline={true} />
                
            </div>
        );
    }
}

VideoPlayer.propTypes = {
    title: PropTypes.string,
    showControls: PropTypes.bool,
}

VideoPlayer.defaultProps = {
    title: null,
    showControls: false
}

export default VideoPlayer;