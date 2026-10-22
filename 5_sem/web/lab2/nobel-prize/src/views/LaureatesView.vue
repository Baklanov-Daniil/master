<script setup lang="ts">
import DataTable from '../components/DataTable.vue'
import { laureatesData } from '../data/laureates'

const columns = [
  { key: 'name', header: 'Имя / Название' },
  { key: 'birthDate', header: 'Дата рождения / основания' },
  { key: 'prizesCount', header: 'Число премий' },
]
</script>

<template>
  <DataTable :columns="columns" :data="laureatesData" title="Нобелевские лауреаты" />
</template>