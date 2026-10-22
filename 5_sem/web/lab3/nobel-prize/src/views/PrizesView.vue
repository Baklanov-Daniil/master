<script setup lang="ts">
import { ref, onMounted } from 'vue'
import DataTable from '../components/DataTable.vue'
import { PrizesService } from '../services/PrizesService'
import type { PrizeRow } from '../services/PrizesService'

const service = new PrizesService()
const rows = ref<PrizeRow[]>([])
const loading = ref(true)
const error = ref('')

const columns = [
  { key: 'category', header: 'Категория' },
  { key: 'date', header: 'Дата вручения' },
  { key: 'grant', header: 'Стоимость гранта' },
]

onMounted(async () => {
  try {
    rows.value = await service.fetchAllPrizes()
  } catch (e: any) {
    error.value = 'Не удалось загрузить премии: ' + (e?.message ?? e)
  } finally {
    loading.value = false
  }
})
</script>

<template>
  <p v-if="loading">Загрузка…</p>
  <p v-else-if="error" style="color: red">{{ error }}</p>
  <DataTable v-else :columns="columns" :data="rows" title="Нобелевские премии" />
</template>