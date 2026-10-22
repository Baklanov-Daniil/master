<script setup lang="ts">
import { ref, computed, watch } from 'vue'

const props = defineProps({
  columns: Array,
  data: Array,
  title: String,
  pageSize: { type: Number, default: 10 },
})

const currentPage = ref(1)
const filter = ref('')

const filtered = computed(() => {
  const rows = (props.data ?? []) as any[]
  const cols = (props.columns ?? []) as any[]
  const q = filter.value.trim().toLowerCase()
  if (!q) return rows
  return rows.filter((row) =>
    cols.some((col) => String(row[col.key] ?? '').toLowerCase().includes(q))
  )
})

const totalPages = computed(() =>
  Math.max(1, Math.ceil(filtered.value.length / (props.pageSize ?? 10)))
)

const pageData = computed(() => {
  const size = props.pageSize ?? 10
  const start = (currentPage.value - 1) * size
  return filtered.value.slice(start, start + size)
})

// Сбрасываем на первую страницу при смене фильтра или данных
watch(filter, () => (currentPage.value = 1))
watch(() => props.data, () => (currentPage.value = 1))

function prev() {
  if (currentPage.value > 1) currentPage.value--
}
function next() {
  if (currentPage.value < totalPages.value) currentPage.value++
}
</script>

<template>
  <div class="table-container">
    <h2>{{ title }}</h2>

    <div class="filter-container">
      <input
        v-model="filter"
        type="text"
        placeholder="Поиск..."
        class="filter-input"
      />
    </div>

    <table>
      <thead>
        <tr>
          <th v-for="(col, i) in columns" :key="i">{{ col.header }}</th>
        </tr>
      </thead>
      <tbody>
        <tr v-if="pageData.length === 0">
          <td :colspan="(columns ?? []).length" class="empty">Нет данных</td>
        </tr>
        <tr v-for="(row, r) in pageData" :key="r">
          <td v-for="(col, c) in columns" :key="c">{{ row[col.key] }}</td>
        </tr>
      </tbody>
    </table>

    <div class="pagination" v-if="totalPages > 1">
      <button class="pagination-btn" :disabled="currentPage <= 1" @click="prev">
        ← Назад
      </button>
      <span class="pagination-info">Страница {{ currentPage }} из {{ totalPages }}</span>
      <button
        class="pagination-btn"
        :disabled="currentPage >= totalPages"
        @click="next"
      >
        Вперёд →
      </button>
    </div>
  </div>
</template>

<style scoped>
.table-container { padding: 20px; max-width: 1000px; margin: 0 auto; }
.table-container h2 { text-align: center; margin-bottom: 20px; }
.filter-container { margin-bottom: 20px; text-align: center; }
.filter-input {
  padding: 10px; width: 300px; border: 1px solid #bdc3c7;
  border-radius: 4px; font-size: 14px;
}
.filter-input:focus { outline: none; border-color: black; }
table {
  width: 100%; border-collapse: collapse; background-color: white;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
}
th, td { border: 1px solid #bdc3c7; padding: 15px; text-align: left; }
th { background-color: black; color: white; }
tbody tr:nth-child(even) { background-color: black; color: white; }
tbody tr:nth-child(odd) { background-color: white; color: black; }
td.empty { text-align: center; background-color: white; color: #888; }
.pagination {
  display: flex; justify-content: center; align-items: center;
  gap: 20px; margin-top: 20px;
}
.pagination-btn {
  padding: 10px 20px; background-color: black; color: white;
  border: none; border-radius: 4px; cursor: pointer; font-size: 14px;
}
.pagination-btn:hover:not(:disabled) { background-color: #333; }
.pagination-btn:disabled { background-color: #ccc; cursor: not-allowed; }
.pagination-info { font-weight: bold; }
</style>