import React, {Component} from 'react';
import './index.css';
import english from '../../../images/usa_flag.png';
import italian from '../../../images/italian_flag.png';

import { LOCALES } from '../../../i18n';
import { translate, translateCap } from '../../../i18n/translate';
import { Dropdown } from 'react-bootstrap';

class Header extends Component {

    render() {
        let { locale } = this.props;
        return (
            <header>

                <div className="row m-0 p-0">
                    <div className="col-12 m-0 p-0">

                        <Dropdown id="language-dropdown" className="float-right">
                            <Dropdown.Toggle variant="primary" id="dropdown-basic" className="px-1 my-bg-primary">
                                {translateCap("general.changeLanguage")}
                            </Dropdown.Toggle>

                            <Dropdown.Menu>
                                <Dropdown.Item
                                    className={(locale === LOCALES.ENGLISH) ? 'active' : ''}
                                    onClick={() => this.props.changeLanguage(LOCALES.ENGLISH)}>
                                        <img src={english}  style={{ width: "13px", height: "10px" }} alt="en" />
                                        <small> English </small>
                                </Dropdown.Item>
                                <Dropdown.Item 
                                    className={(locale === LOCALES.ITALIAN) ? 'active' : ''}
                                    onClick={() => this.props.changeLanguage(LOCALES.ITALIAN)}>
                                        <img src={italian} style={{ width: "13px", height: "10px" }} alt="it" />
                                        <small> Italian </small>
                                </Dropdown.Item>
                            </Dropdown.Menu>
                        </Dropdown>

                    </div>
                </div>
            </header>
        )
    }
}

export default Header;