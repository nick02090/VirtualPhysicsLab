import axios from 'axios';
axios.defaults.headers.post['Content-Type'] = 'application/json';
axios.defaults.headers.put['Content-Type'] = 'application/json';

export default {
    async postUser() {

        // var create = await axios.post('/api/user', {
        //     logicalName: "user",
        //     password: "12345678",
        //     name: "nick",
        //     nickName: "nick",
        //     email: "nidjolas.u.b@gmail.com",
        //     firstName: "Nikola",
        //     lastName: "Calis"
        // });

        var response = await axios.post('/api/user/authenticate', {
            logicalName: "user",
            password: "12345678",
            name: "nick",
            nickName: "nick",
            email: "nidjolas.u.b@gmail.com",
            firstName: "Nikola",
            lastName: "Calis",
            occupation: "student"
        })

        console.log(response)

        return response.data;


        // return response.data;
    }
}