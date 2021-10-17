import React, { Component } from 'react';
import PropTypes from 'prop-types';
import './SlideDrawer.css';

const SlideDrawer = ({ id, isOpen, onClose, children }) => {
    return (
        <>
            <div className={`slide-drawer-mask ${isOpen ? '' : 'd-none'}`} onClick={onClose}></div>
            <div id={id} className={`slide-drawer ${isOpen ? 'open' : ''}`}>
                <div className="slide-drawer-container">
                    {children}
                </div>
            </div>
        </>
    );
}

SlideDrawer.propTypes = {
    id: PropTypes.string,
    isOpen: PropTypes.bool,
    onClose: PropTypes.func.isRequired
}

SlideDrawer.defaultProps = {
    id: 'slidedrawer',
    isOpen: false,
    onClose: () => console.log('Slidedrawer onClose')
}

export default SlideDrawer;