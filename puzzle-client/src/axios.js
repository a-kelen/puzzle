import Axios from 'axios';

const api = Axios.create({
	baseURL: import.meta.env.VITE_API_URL,
});
const token = localStorage.getItem('token');
if (token) {
	api.defaults.headers.common.Authorization = `Bearer ${token}`;
}
api.defaults.withCredentials = false;
export default api;