<script setup lang="ts">
import DataTable from '../components/DataTable.vue'
import { prizesData } from '../data/prizes'

const columns = [
  { key: 'category', header: 'Категория' },
  { key: 'date', header: 'Дата вручения' },
  { key: 'grant', header: 'Стоимость гранта' },
]
</script>

<template>
  <DataTable :columns="columns" :data="prizesData" title="Нобелевские премии" />
</template>