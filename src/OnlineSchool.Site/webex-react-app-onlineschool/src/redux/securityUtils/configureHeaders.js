import axios from 'axios';

const configureHeaders = (token) => {
    axios.defaults.headers.common["Content-Type"] = "application/json";
    if(token)
        axios.defaults.headers.common["Authorization"] = token;
    else
        delete axios.defaults.headers.common["Authorization"];
};

export default configureHeaders;
