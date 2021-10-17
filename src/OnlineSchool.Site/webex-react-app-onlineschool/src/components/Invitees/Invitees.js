import React, { Component } from 'react';
import classnames from 'classnames';
import { Modal } from 'react-bootstrap';
import { translate, translateCap } from '../../i18n/translate';
import { confirmation, isValidEmail } from '../../utils/helpers';

class Invitees extends Component {

    constructor(props) {
        super(props);        
        this.state = {
            email: '',
            displayName: '',
            coHost: false,
            show: false,
            errors: []
        }
    }

    onOpen = () => {
        this.setState({ show: true });
    }

    onClose = () => {
        this.setState({ 
            email: '',
            displayName: '',
            coHost: false,
            show: false,

            showErrors: false,
            errors: []
        });
    }

    showErrors = (errors) => {
        this.setState({ errors, showErrors: true });
    }

    clearErrors = () => {
        this.setState({ errors: [], showErrors: false });
    }

    onChange = e => {
        this.setState({ [e.target.name]: e.target.value });
    }

    onCheckboxChange = e => {
        this.setState({ [e.target.name]: e.target.checked });
    }

    onRemove = (email) => {
        /* Internationalization */
        confirmation(translateCap('confirmations.removeInvitee', { email }), () => {
            this.props.onRemove(email);
        }, 
        null);
    }

    onSubmit = () => {
        let { email, displayName, coHost } = this.state;
        let invitee = { email, displayName, coHost };

        let errors = this.validate(invitee);
        if(errors.length > 0)
            this.showErrors(errors);
        else {
            this.props.onAdd(invitee);
            this.onClose();
        }
    }

    validate = (invitee) => {
        let validationErrors = [];

        if(!isValidEmail(invitee.email))
            validationErrors.push(translateCap('validations.invalidEmail'));
        // Other validations if required
        return validationErrors;
    }

    render() {
        let { email, displayName, coHost, show, showErrors, errors } = this.state;
        let { invitees, allowCoHost } = this.props;
        return (
            <div id="invitees">

                <h5 className="p-0 m-0"> {translateCap('Invitees.invitees')} </h5>
                                
                <div className="collapse show col-12 p-0 mt-3" id="invitees">
                    <div id="invitees-table" className="table-responsive">
                        <table className="table table-sm table-bordered">
                            <thead className="thead-light">
                                <tr>
                                    <th scope="col" className="text-center"> {translateCap('Invitees.email')} </th>
                                    <th scope="col" className="text-center"> {translateCap('Invitees.name')} </th>
                                    <th scope="col" className="text-center"> {translateCap('Invitees.coHost')} </th>
                                    <th scope="col" className="text-center"> {translateCap('general.actions')} </th>
                                </tr>
                            </thead>
                            <tbody>
                                {
                                    (invitees && invitees.length > 0) ?
                                        invitees.map((invitee, index) => {
                                            return (
                                                <tr key={index}>
                                                    <td className='text-center'> <small> {invitee.email} </small> </td>
                                                    <td className='text-center'> <small> {invitee.displayName} </small> </td>
                                                    <td className='text-center'> 
                                                        <small> 
                                                        {
                                                            invitee.coHost ?
                                                                <i className="fas fa-check text-muted"></i> :
                                                                <i className="fas fa-times text-muted"></i>
                                                        } 
                                                        </small> 
                                                    </td>
                                                    <td className="text-center"> 
                                                        <i type='button' className='fas fa-trash-alt cursor-pointer text-danger' data-toggle='tooltip' title={translateCap('general.remove')} onClick={() => this.onRemove(invitee.email)}>
                                                        </i>
                                                    </td>
                                                </tr>
                                            )
                                        })
                                        :
                                        <tr>
                                            <td colSpan="4"> <small className="text-muted ml-2"> {translateCap('Invitees.noInvitees')}. </small> </td>
                                        </tr>
                                }
                            </tbody>
                        </table>
                    </div>
                    <div className="row">
                        <div className="col-3 mx-auto text-center">
                            <button id="addInviteeBtn" type="button" className="btn btn-sm btn-outline-secondary my-2" onClick={this.onOpen} data-placement="top" title={translateCap('Invitees.newInvitation')}> + </button>
                        </div>
                    </div>        
                </div>

                {/* Modal: add invitee */}
                <Modal show={show} onHide={this.onClose} backdrop="static" centered keyboard={false}>
                    
                    <Modal.Header closeButton>
                        <Modal.Title> {translateCap('Invitees.newInvitation')} </Modal.Title>
                    </Modal.Header>

                    <Modal.Body>
                        <div id="add-invitee-form">

                            {/* Component: Validation summary */}
                            <div id="validation-summary" className={classnames("validation-summary my-2 border rounded shadow text-white bg-danger ", {"d-none": !showErrors })}>
                                <div className="col-12 text-right p-0">
                                    <button type="button" className="btn btn-sm btn-default text-white" data-toggle="tooltip" title={translateCap('general.close')} onClick={() => this.clearErrors()}> x </button>
                                </div>
                                <ul id="validation-summary-errors">
                                    {
                                        errors.map((err, index) => {
                                            return <li key={index} className="validation-summary-item"> {err} </li>
                                        })
                                    }
                                </ul>
                            </div>

                            <div className="form-group">
                                <label htmlFor="invitee-email"> {translateCap('Invitees.email')} <small className="text-danger">*</small> </label>
                                <input required type="text" id="invitee-email" name="email" className="form-control form-control-sm" value={email} onChange={this.onChange} />
                            </div>
                            <div className="form-group">
                                <label htmlFor="invitee-displayName"> {translateCap('Invitees.name')} </label>
                                <input type="text" id="invitee-displayName" name="displayName" className="form-control form-control-sm" value={displayName} onChange={this.onChange} />
                            </div>
                            {/* {
                                !allowCoHost ? 
                                    null :
                                    <div className="custom-control custom-switch mt-3">
                                        <input type="checkbox" id="coHost" name="coHost" className="custom-control-input" checked={coHost} onChange={this.onCheckboxChange} />
                                        <label className="custom-control-label" htmlFor="coHost"> Co-Host </label>
                                    </div>
                            } */}
                            <div className="col-12 mt-4 px-0 pt-3 border-top text-right">
                                <button type="button" className="btn btn-sm btn-secondary" onClick={this.onClose}> {translateCap('general.close')} </button>
                                <button type="button" className="btn btn-sm btn-success ml-2" onClick={this.onSubmit}> {translateCap('general.save')} </button>
                            </div>
                        </div>
                    </Modal.Body>
                </Modal>

            </div>
        );
    }
}

export default Invitees;