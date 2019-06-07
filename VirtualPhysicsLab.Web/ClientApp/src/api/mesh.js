import axios from 'axios';
axios.defaults.headers.post['Content-Type'] = 'application/json';
axios.defaults.headers.put['Content-Type'] = 'application/json';

export default {
    async createMesh(mesh) {

        var response = await axios.post('/api/mesh', {
            setting: mesh.settings,
            experiment: mesh.experiment,
            type: mesh.type,
            name: mesh.name
        })

        return response.data;
    },
    async createSettings(settings) {

        var response = await axios.post('/api/mesh/settings', settings);

        return response.data;
    },
    async getByExperiment(id) {

        var response = await axios.get('/api/mesh/experiment', {
            params: {
                id: id
            }
        });

        return response.data;
    }
}