<template>
  <header>
    <h1>Нобелевские премии</h1>
    <p>Лабораторная работа №3. Сторонее API</p>
  </header>

  <nav>
    <router-link to="/">Премии</router-link>
    <router-link to="/laureates">Лауреаты</router-link>
  </nav>

  <main>
    <router-view />
  </main>

  <footer>
    <p>Выполнил: Бакланов Даниил МО-241</p>
  </footer>
</template>