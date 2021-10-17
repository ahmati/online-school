import React from 'react';
import PropTypes from 'prop-types';

function Refresh(props) {
    let { size, color, className, tooltip } = props;
    return (
        <button className={`btn btn-${size} btn-${color} ${className}`} title={tooltip} onClick={props.onClick}>
            <i className="fas fa-sync-alt"></i>
        </button>
    );
}

Refresh.propTypes = {
    size: PropTypes.string,
    color: PropTypes.string,
    className: PropTypes.string,
    tooltip: PropTypes.string
}

Refresh.defaultProps = {
    size: 'md',
    color: 'primary',
    className: '',
    tooltip: 'Refresh'
}

export default Refresh;