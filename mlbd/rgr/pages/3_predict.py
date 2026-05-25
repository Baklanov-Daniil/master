import streamlit as st
import pandas as pd
import pickle
import keras
import catboost

MODEL_PATHS = {
    "ML1: Классическая (kNN)": "models/ML1_LogReg.pkl",
    "ML2: Бустинг": "models/ML2_RandomForest.pkl",
    "ML3: CatBoost": "models/ML3_CatBoost.cbm",
    "ML4: Бэггинг": "models/ML4_Bagging.pkl",
    "ML5: Стэкинг": "models/ML5_Stacking.pkl",
    "ML6: Нейросеть": "models/ML6_NeuralNet.keras"
}

@st.cache_resource
def load_models():
    loaded_models = {}
    
    for name, path in MODEL_PATHS.items():
        try:
            if path.endswith('.keras'):
                loaded_models[name] = keras.models.load_model(path)
            elif path.endswith('.cbm'):
                loaded_models[name] = catboost.CatBoostClassifier()
                loaded_models[name].load_model(path)
            else:
                with open(path, 'rb') as f:
                    loaded_models[name] = pickle.load(f)
                    
        except FileNotFoundError:
            st.error(f"Файл модели не найден: {path}")
            st.stop()
        except Exception as e:
            st.error(f"Ошибка при загрузке модели {name}: {e}")
            st.stop()
    
    return loaded_models

models = load_models()

st.set_page_config(page_title="Предсказание типа вина", layout="wide")
st.title(" Предсказание типа вина")
st.markdown("Введите параметры химического состава вина или загрузите CSV файл.")

def predict_wine(input_data, model):
    prediction = model.predict(input_data)
    try:
        probability = model.predict_proba(input_data)[0][1]
    except AttributeError:
        probability = 0.5 if prediction[0] == 1 else 0.0
        
    return prediction[0], probability

tabs = st.tabs(["📝 Ручной ввод", "📂 Загрузка CSV"])

with tabs[0]:
    st.subheader("Введите характеристики вина")
    features = {
        "fixed acidity": (3.0, 16.0, 0.1, "г/дм³"),
        "volatile acidity": (0.0, 2.0, 0.01, "г/дм³"),
        "citric acid": (0.0, 2.0, 0.01, "г/дм³"),
        "residual sugar": (0.0, 70.0, 0.1, "г/дм³"),
        "chlorides": (0.0, 0.6, 0.001, "г/дм³"),
        "free sulfur dioxide": (1.0, 300.0, 1.0, "мг/дм³"),
        "total sulfur dioxide": (6.0, 450.0, 1.0, "мг/дм³"),
        "density": (0.98, 1.05, 0.0001, "г/см³"),
        "pH": (2.7, 4.0, 0.01, "pH"),
        "sulphates": (0.2, 2.0, 0.01, "г/дм³"),
        "alcohol": (8.0, 15.0, 0.1, "%"),
        "quality": (3, 10, 1, "балл"),
    }
    
    user_inputs = {}
    cols = st.columns(3)
    
    for i, (name, (mn, mx, step, unit)) in enumerate(features.items()):
        with cols[i % 3]:
            val = st.number_input(
                f"{name} ({unit})",
                min_value=float(mn),
                max_value=float(mx),
                value=float(mn),
                step=float(step)
            )
            user_inputs[name] = val

    if st.button("🔮 Предсказать", type="primary"):
        input_df = pd.DataFrame([user_inputs])
        
        results = []
        for name, model in models.items():
            try:
                pred_class, prob = predict_wine(input_df, model)
                
                wine_type = "🍷 Красное вино" if pred_class == 1 else "🥂 Белое вино"
                # Вероятность того класса, который предсказан
                conf = prob if pred_class == 1 else (1 - prob)
                
                results.append({
                    "Модель": name,
                    "Тип": wine_type,
                    "Уверенность": f"{conf:.2%}"
                })
                st.success("Предсказание успешно!")
            except Exception as e:
                results.append({
                    "Мodel": name,
                    "Ошибка": str(e)
                })
                st.warning("Предсказание Провалилось!")
        
        
        st.dataframe(pd.DataFrame(results), use_container_width=True)

with tabs[1]:
    st.subheader("Загрузка файла для пакетного анализа")
    uploaded_file = st.file_uploader("Загрузите CSV файл с данными", type=["csv"])
    
    if uploaded_file is not None:
        try:
            data_to_predict = pd.read_csv(uploaded_file)
            
            required_cols = list(features.keys())
            if not all(col in data_to_predict.columns for col in required_cols):
                st.error(f"Ошибка: в файле не хватает колонок. Ожидаемые: {required_cols}")
            else:
                st.info("Файл загружен. Нажмите кнопку для анализа.")
                
                st.dataframe(data_to_predict.head())
                
                if st.button("🚀 Анализировать все записи"):
                    X = data_to_predict[required_cols]

                    first_model = list(models.values())[0]
                    preds = first_model.predict(X)
                    
                    df_res = data_to_predict.copy()
                    df_res['Prediction'] = preds.map({0: 'White', 1: 'Red'})
                    
                    st.dataframe(df_res)
                    st.bar_chart(df_res['Prediction'].value_counts())
                    
        except Exception as e:
            st.error(f"Ошибка обработки файла: {e}")