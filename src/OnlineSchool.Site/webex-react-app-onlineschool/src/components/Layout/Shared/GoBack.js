import React from 'react';
import PropTypes from 'prop-types';

const BackButton = ({ label, history }) => {
    return (
        <>
            <button className="btn my-btn-secondary btn-sm shadow rounded my-2" onClick={() => history.goBack()}> 
                <i className="fa fa-arrow-left" aria-hidden="true"/> {label}
            </button>
        </>
    );
}

BackButton.propTypes = {
    label: PropTypes.string,
    history: PropTypes.object
}

BackButton.defaultProps = {
    label: 'Go back'
}

export default BackButton;