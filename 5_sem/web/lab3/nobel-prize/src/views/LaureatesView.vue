<script setup lang="ts">
import { ref, onMounted } from 'vue'
import DataTable from '../components/DataTable.vue'
import { LaureatesService } from '../services/LaureatesService'
import type { LaureateRow } from '../services/LaureatesService'

const service = new LaureatesService()
const rows = ref<LaureateRow[]>([])
const loading = ref(true)
const error = ref('')

const columns = [
  { key: 'name', header: 'Имя / Название' },
  { key: 'birthDate', header: 'Дата рождения / основания' },
  { key: 'prizesCount', header: 'Число премий' },
]

onMounted(async () => {
  try {
    rows.value = await service.fetchAllLaureates()
  } catch (e: any) {
    error.value = 'Не удалось загрузить лауреатов: ' + (e?.message ?? e)
  } finally {
    loading.value = false
  }
})
</script>

<template>
  <p v-if="loading">Загрузка…</p>
  <p v-else-if="error" style="color: red">{{ error }}</p>
  <DataTable v-else :columns="columns" :data="rows" title="Нобелевские лауреаты" />
</template>