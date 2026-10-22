<script setup lang="ts">
defineProps({
  columns: Array,
  data: Array,
  title: String,
})
</script>

<template>
  <div class="table-container">
    <h2>{{ title }}</h2>
    <table>
      <thead>
        <tr>
          <th v-for="(col, index) in columns" :key="index">
            {{ col.header }}
          </th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(row, rowIndex) in data" :key="rowIndex">
          <td v-for="(col, colIndex) in columns" :key="colIndex">
            {{ row[col.key] }}
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<style scoped>
.table-container {
  padding: 20px;
  max-width: 1000px;
  margin: 0 auto;
}

.table-container h2 {
  text-align: center;
  margin-bottom: 20px;
}

table {
  width: 100%;
  border-collapse: collapse;
  background-color: white;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
}

th, td {
  border: 1px solid #bdc3c7;
  padding: 15px;
  text-align: left;
}

th {
  background-color: black;
  color: white;
}

tbody tr:nth-child(even) {
  background-color: black;
  color: white;
}

tbody tr:nth-child(odd) {
  background-color: white;
  color: black;
}
</style>