import React, { useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../AuthContext';

const Logout = () => {
    const navigate = useNavigate();
    const { setUser } = useAuth();
    useEffect(() => {
        const doLogout = async () => {
            await axios.post('/api/account/logout');
            setUser(null);
            navigate('/');
        }
        doLogout();
    }, []);

    return (<></>);
}

export default Logout;