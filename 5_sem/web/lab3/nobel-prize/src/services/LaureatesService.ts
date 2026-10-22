import { HttpClient } from './HttpClient'

interface RawLaureate {
  id?: string
  knownName?: { en?: string; [k: string]: any }
  fullName?: { en?: string; [k: string]: any }
  birth?: { date?: string; year?: number | string }
  nobelPrizes?: any[]
}
interface RawLaureatesResponse {
  laureates?: RawLaureate[]
}

export interface LaureateRow {
  name: string
  birthDate: string
  prizesCount: number
}

export class LaureatesService {
  private client: HttpClient

  constructor() {
    this.client = new HttpClient('https://api.nobelprize.org/2.0')
  }

  async fetchAllLaureates(): Promise<LaureateRow[]> {
    const data = await this.client.get<RawLaureatesResponse>('/laureates')
    const list = data.laureates ?? []
    return list.map((l) => {
      const name = l.knownName?.en ?? l.fullName?.en ?? l.id ?? 'Неизвестно'

      let birthDate = 'Неизвестно'
      if (l.birth?.date) {
        const d = new Date(l.birth.date)
        birthDate = isNaN(d.getTime())
          ? String(l.birth.date)
          : d.toLocaleDateString('ru-RU')
      } else if (l.birth?.year != null) {
        birthDate = String(l.birth.year)
      }

      return { name, birthDate, prizesCount: l.nobelPrizes?.length ?? 0 }
    })
  }
}