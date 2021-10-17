import React, { Component } from 'react';
import isEqual from 'react-fast-compare';
import { getRandomData, getRandomInt } from '../../../test/data';

class Test2 extends Component {

    componentDidMount = () => {
        let obj1 = getRandomData()
        let obj2 = getRandomData()
        
        console.time('deep-compare')
        obj1.forEach(i => {
            let test = obj2.filter(o => o.id === i.id)[0]
            isEqual(i, obj2[getRandomInt(0, obj2.length)])
        })
        console.timeEnd('deep-compare')
    }

    render() {
        return (
            <>
            </>
        );
    }
}

export default Test2;