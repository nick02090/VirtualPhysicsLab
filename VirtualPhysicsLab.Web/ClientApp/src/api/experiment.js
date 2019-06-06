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
    }
}