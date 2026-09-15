import axios from 'axios';

class ApiService {
    constructor(baseURL) {
        this.client = axios.create({
            baseURL: baseURL,
        });
    }

    async get(url, params = {}) {
        try {
            const response = await this.client.get(url, { params });
            return response.data;
        } catch (error) {
            console.error("Ошибка API:", error);
            throw error;
        }
    }
}

export default ApiService;