import { useState, useEffect } from 'react';
import DataTable from '../components/DataTable';
import '../components/DataTable.css';
import laureatesService from '../services/laureatesService';

const columns = [
  { key: 'name', header: 'Имя / Название' },
  { key: 'birthDate', header: 'Дата рождения / Создания' },
  { key: 'prizesCount', header: 'Число премий' },
];

function LaureatesPage() {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 10;

  useEffect(() => {
    laureatesService.getAllLaureates()
      .then(fetchedData => {
        setData(fetchedData);
        setLoading(false);
      })
      .catch(err => console.error(err));
  }, []);

  const filteredData = data.filter(item =>
    Object.values(item).some(val =>
      String(val).toLowerCase().includes(searchTerm.toLowerCase())
    )
  );

  const indexOfLast = currentPage * itemsPerPage;
  const indexOfFirst = indexOfLast - itemsPerPage;
  const currentData = filteredData.slice(indexOfFirst, indexOfLast);
  const totalPages = Math.ceil(filteredData.length / itemsPerPage);

  if (loading) return <h2>Загрузка данных...</h2>;

  return (
    <div>
      <input
        type="text"
        placeholder="Поиск лауреата..."
        value={searchTerm}
        onChange={(e) => {
            setSearchTerm(e.target.value);
            setCurrentPage(1);
        }}
        style={{ padding: '10px', marginBottom: '20px', width: '300px' }}
      />

      <DataTable columns={columns} data={currentData} title="Нобелевские лауреаты (API)" />

      <div style={{ marginTop: '20px' }}>
        <button 
            onClick={() => setCurrentPage(p => Math.max(p - 1, 1))} 
            disabled={currentPage === 1}
            style={{ marginRight: '10px', padding: '5px 15px' }}
        >
            Назад
        </button>
        <span>Страница {currentPage} из {totalPages || 1}</span>
        <button 
            onClick={() => setCurrentPage(p => Math.min(p + 1, totalPages))} 
            disabled={currentPage === totalPages || totalPages === 0}
            style={{ marginLeft: '10px', padding: '5px 15px' }}
        >
            Вперед
        </button>
      </div>
    </div>
  );
}

export default LaureatesPage;