import DataTable from '../components/DataTable';
import '../components/DataTable.css';
import { laureatesData } from '../data/laureates';

const columns = [
  { key: 'name', header: 'Имя / Название' },
  { key: 'birthDate', header: 'Дата рождения / основания' },
  { key: 'prizesCount', header: 'Число премий' },
];

function LaureatesPage() {
  return <DataTable columns={columns} data={laureatesData} title="Нобелевские лауреаты" />;
}

export default LaureatesPage;