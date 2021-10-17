import React from 'react';
import { Route } from 'react-router-dom';

import AutoScroll from '../components/Layout/Shared/AutoScroll';
import ScrollTopButton from '../components/Layout/Shared/ScrollTopButton';

const CustomRoute = ({ component: Component, authStore, ...otherProps }) => (

    <Route 
        {...otherProps}
        render={props => {
            return (
                <>
                    <AutoScroll />
                    <Component {...props} {...otherProps}/>
                    <ScrollTopButton />
                </>
            )
        }}/>
);

export default CustomRoute;