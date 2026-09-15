import ApiService from './api';

class PrizesService extends ApiService {
    constructor() {
        super('https://api.nobelprize.org/2.0');
    }

    async getAllPrizes() {
        const data = await this.get('/nobelPrizes');
        console.log("Сырые данные премий:", data);
        
        const prizes = data.nobelPrizes || data.prizes || [];
        
        return prizes.map(p => ({
            category: typeof p.category === 'object' 
                ? (p.category.ru || p.category.en || 'Неизвестно')
                : (p.category || 'Неизвестно'),
            date: p.awardYear?.toString() || p.year?.toString() || 'Неизвестно',
            grant: p.prizeAmount ? `${p.prizeAmount} SEK` : 'Неизвестно'
        }));
    }
}

export default new PrizesService();