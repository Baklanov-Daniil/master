import { HttpClient } from './HttpClient'

// Сырая структура ответа API 2.0 /nobelPrizes
interface RawPrize {
  category?: any
  awardYear?: number | string
  year?: number | string
  prizeAmount?: number
}
interface RawPrizesResponse {
  nobelPrizes?: RawPrize[]
  prizes?: RawPrize[]
}

// Готовая строка для таблицы
export interface PrizeRow {
  category: string
  date: string
  grant: string
}

export class PrizesService {
  private client: HttpClient

  constructor() {
    this.client = new HttpClient('https://api.nobelprize.org/2.0')
  }

  async fetchAllPrizes(): Promise<PrizeRow[]> {
    const data = await this.client.get<RawPrizesResponse>('/nobelPrizes')
    const list = data.nobelPrizes ?? data.prizes ?? []
    return list.map((p) => ({
      category:
        typeof p.category === 'object' && p.category !== null
          ? (p.category.ru ?? p.category.en ?? 'Неизвестно')
          : String(p.category ?? 'Неизвестно'),
      date: String(p.awardYear ?? p.year ?? 'Неизвестно'),
      grant:
        p.prizeAmount != null
          ? `${p.prizeAmount.toLocaleString('ru-RU')} SEK`
          : 'Неизвестно',
    }))
  }
}