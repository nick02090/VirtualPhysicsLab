import axios from 'axios';
axios.defaults.headers.post['Content-Type'] = 'application/json';
axios.defaults.headers.put['Content-Type'] = 'application/json';

export default {
    async createExperiment(experiment) {

        var response = await axios.post('/api/experiment', {
            setting: experiment.settings,
            meshes: experiment.meshes,
            title: experiment.title,
            description: experiment.description,
            createdBy: experiment.createdBy
        })

        return response.data;
    },
    async createSettings(settings) {

        var response = await axios.post('/api/experiment/settings', settings);

        return response.data;
    },
    async getByUser(id) {

        var response = await axios.get('/api/experiment/user', {
            params: {
                id: id
            }
        });

        return response.data;
    },
    async getById(id) {

        var response = await axios.get(`/api/experiment/${id}`)

        return response.data;
    },
    async updateExperiment(experiment) {

        var response = await axios.put(`/api/experiment/${experiment.id}`, experiment)

        return response.data;
    },
    async updateSettings(settings) {

        var response = await axios.put(`/api/experiment/settings`, settings);

        return response.data;
    },
    async checkAvailability(data) {

        var response = await axios.post('/api/experiment/check-availability', {
            params: {
                title: data.title,
                userId: data.userId
            }
        });

        return response.data;
    },
    async deleteExperiment(experimentId) {

        var response = await axios.delete(`/api/experiment/${experimentId}`);

        return response.data;
    },
    async getExperiments() {

        var repsonse = await axios.get('/api/experiment');

        return repsonse.data;
    }
}