import pandas as pd
from sqlalchemy import create_engine

file_path = 'customers.xlsx'
df = pd.read_excel(file_path)


engine = create_engine('postgresql://postgres:12345@localhost:5432/sql_lab01_baklanov')
table_name = 'customers_temp'
df.to_sql(table_name, engine, if_exists='replace', index=False)
print('Данные успешно загружены!')



