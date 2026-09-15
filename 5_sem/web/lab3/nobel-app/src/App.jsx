import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import PrizesPage from './pages/PrizesPage';
import LaureatesPage from './pages/LaureatesPage';
import './App.css';

function App() {
  return (
    <BrowserRouter>
      <header>
        <h1>Нобелевские премии</h1>
        <p>Лабораторная работа №3. API</p>
      </header>

      <nav>
        <Link to="/">Премии</Link>
        <Link to="/laureates">Лауреаты</Link>
      </nav>

      <main>
        <Routes>
          <Route path="/" element={<PrizesPage />} />
          <Route path="/laureates" element={<LaureatesPage />} />
        </Routes>
      </main>

      <footer>
        <p>Выполнил: Бакланов Даниил МО-241</p>
      </footer>
    </BrowserRouter>
  );
}

export default App;