import api from '@/axios';

export default {
    login: (name, password) => {
        return api.post('/User/login', { name, password });
    },
    current: () => {
        return api.get('/User/current');
    },
    getBackup: () => {
        return api.get('/User/backup');
    },
    setBackup: (payload) => {
        return api.post('/User/backup', payload);
    },
    deleteBackup: () => {
        return api.delete('/User/backup');
    },
}
