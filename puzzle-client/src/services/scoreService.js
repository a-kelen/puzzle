import api from '@/axios';

export default {
    addScore: (payload) => {
        return api.post('/Score', payload);
    },
}
