using System;
using System.IO;
using UnityEngine;

namespace Lab6_namespace {
    public static class JsonHelperEstrellas {
        private static string filePath = Application.persistentDataPath + "/estrellas.json";

        public static void SaveStars(int stars) {
            EstrellasData data = new EstrellasData();
            data.estrellas = stars.ToString();  // Guardamos como string

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, json);
            Debug.Log("Estrellas guardadas en: " + filePath);
        }

        public static int LoadStars() {
            if (File.Exists(filePath)) {
                string json = File.ReadAllText(filePath);
                EstrellasData data = JsonUtility.FromJson<EstrellasData>(json);

                if (int.TryParse(data.estrellas, out int result)) {
                    return result;
                } else {
                    Debug.LogWarning("No se pudo convertir el string de estrellas a int. Valor por defecto: 0.");
                    return 0;
                }
            } else {
                Debug.Log("Archivo de estrellas no encontrado. Se devuelve 0 por defecto.");
                return 0;
            }
        }
    }

    [Serializable]
    public class EstrellasData {
        public string estrellas;
    }
}
