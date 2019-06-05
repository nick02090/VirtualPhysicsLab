import axios from 'axios';
axios.defaults.headers.post['Content-Type'] = 'application/json';
axios.defaults.headers.put['Content-Type'] = 'application/json';

export default {
    async createUser(user) {

        var response = await axios.post('/api/user', {
            password: user.password,
            email: user.email,
            firstName: user.firstName,
            lastName: user.lastName,
            occupation: user.occupation
        })

        return response.data;
    },
    async checkAvailability(user) {

        var response = await axios.post('/api/user/check-availability', {
            password: user.password,
            email: user.email,
            firstName: user.firstName,
            lastName: user.lastName,
            occupation: user.occupation
        })

        return response.data;
    },
    async authenticate(user) {

        var response = await axios.post('/api/user/authenticate', {
            password: user.password,
            email: user.email
        })

        return response.data;
    },
    async retrieveUser(token) {

        var response = await axios.get('/api/user/token', {
            params: {
                token: token
            }
        })

        return response.data;
    },
    async logout(token) {

        var response = await axios.get('/api/user/logout', {
            params: {
                token: token
            }
        });

        return response.data;
    }
}