import React from 'react';
import ScrollToTop from 'react-scroll-up';

const ScrollTopButton = () => {

    const style = {
        zIndex: 50,
        bottom: 38,
        right: 15,
        color: '#164da3'
    }

    return (
        <ScrollToTop showUnder={150} style={style}>
            <i className="fa fa-arrow-circle-up fa-2x" aria-hidden="true"/>
        </ScrollToTop>
    )
};

export default ScrollTopButton;
