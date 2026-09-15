import DataTable from '../components/DataTable';
import '../components/DataTable.css';
import { prizesData } from '../data/prizes';

const columns = [
  { key: 'category', header: 'Категория' },
  { key: 'date', header: 'Дата вручения' },
  { key: 'grant', header: 'Стоимость гранта' },
];

function PrizesPage() {
  return <DataTable columns={columns} data={prizesData} title="Нобелевские премии" />;
}

export default PrizesPage;