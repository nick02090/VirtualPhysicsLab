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
    async updateMesh(mesh) {

        var response = await axios.put(`/api/mesh/${mesh.id}`, mesh);

        return response.data;
    },
    async updateSettings(settings) {

        var response = await axios.put('/api/mesh/settings', settings);

        return response.data;

    },
    async getByExperiment(id) {

        var response = await axios.get('/api/mesh/experiment', {
            params: {
                id: id
            }
        });

        return response.data;
    },
    async deleteMesh(meshId) {

        var response = await axios.delete(`/api/mesh/${meshId}`);

        return response.data;
    }
}