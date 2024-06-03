import React, { useState, useRef, useEffect } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import axios from 'axios';
import { useAuth } from '../AuthContext';

const Home = () => {

    const connectionRef = useRef(null);
    const { user } = useAuth();
    const [title, setTitle] = useState();
    const [taskItems, setTaskItems] = useState([]);
    
    const { id } = user;

    const loadTaskItems = async () => {
        const { data } = await axios.get('/api/taskitem/getall');
        setTaskItems(data);
    }

    useEffect(() => {
        const connectToHub = async () => {
            const connection = new HubConnectionBuilder().withUrl("/api/task").build();
            await connection.start();
            connectionRef.current = connection;

            connection.on('addTask', value => {
                setTaskItems(allTasks => [...allTasks, value]);
            });

            connection.on('markAsDoing', taskItems => {
                setTaskItems(taskItems);
            });
        }

        connectToHub();
        loadTaskItems();
    }, []);

    const onAddTaskClick = async () => {
        await axios.post('/api/taskitem/add', { title });
        setTitle('');
    }

    const onDoTaskClick = async (id) => {
        await axios.post('/api/taskitem/markasdoing', { id });
    }

    const onDeleteClick = async (id) => {
        await axios.post('/api/taskitem/delete', { id });
        loadTaskItems();
    }

    return (
        <div className='container' style={{ marginTop: 80 }}>
            <div style={{ marginTop: 70 }}>
                <div className='row'>
                    <div className='col-md-10'>
                        <input type="text" className='form-control' placeholder='Task Title'
                            value={title} onChange={e => setTitle(e.target.value)}></input>
                    </div>
                    <div className='col-md-2'>
                        <button className='btn btn-primary w-100' onClick={onAddTaskClick}>Add Task</button>
                    </div>
                </div>
                <table className='table table-hover table-striped table-bordered mt-3'>
                    <thead>
                        <tr>
                            <th>Title</th>
                            <th>Status</th>
                        </tr>
                    </thead>
                    <tbody>
                        {taskItems.map(t => <tr key={t.id}>
                            <td>{t.title}</td>
                            <td>
                                {!t.userId && <button className='btn btn-dark' style={{ marginRight: 10 }} onClick={() => onDoTaskClick(t.id)}>I'm doing this one!</button>}
                                {t.userId !== id && t.userId && <button className='btn btn-warning' disabled>{t.user.firstName} {t.user.lastName} is doing this one</button>}
                                {t.userId === id &&
                                    <button className='btn btn-primary' onClick={() => onDeleteClick(t.id)}>I'm done!</button>}
                            </td>
                        </tr>)}
                    </tbody>
                </table>
            </div>
        </div>
    )
};

export default Home;