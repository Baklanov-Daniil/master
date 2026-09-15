import ApiService from './api';

class LaureatesService extends ApiService {
    constructor() {
        super('https://api.nobelprize.org/2.0');
    }

    async getAllLaureates() {
        const data = await this.get('/laureates');
        console.log("Пример лауреата:", data.laureates?.[0]);
        
        const laureates = data.laureates || [];
        
        return laureates.map(l => {
            const name = l.knownName?.en || l.fullName?.en || 'Неизвестно';
            
            let birthDate = 'Неизвестно';
            if (l.birth?.date) {
                const date = new Date(l.birth.date);
                if (!isNaN(date)) {
                    birthDate = date.toLocaleDateString('ru-RU');
                }
            } else if (l.birth?.year) {
                birthDate = l.birth.year;
            }
            
            const prizesCount = l.nobelPrizes ? l.nobelPrizes.length : 0;

            return {
                name: name,
                birthDate: birthDate,
                prizesCount: prizesCount
            };
        });
    }
}

export default new LaureatesService();