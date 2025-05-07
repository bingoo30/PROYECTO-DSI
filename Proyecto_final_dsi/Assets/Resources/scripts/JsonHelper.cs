using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace stars_namespace {
    public static class JsonHelperEstrellas {
        private static string filePath = Application.persistentDataPath + "/estrellas.json";
        
        [Serializable]
        public class EstrellaNivel
        {
            public int nivel;
            public int estrellas;
        }
        public class EstrellaDataList
        {
            public List<EstrellaNivel> niveles = new List<EstrellaNivel>();
        }
        public static void SaveStars(int level, int stars) {
            EstrellaDataList dataList = LoadAllStars();

            // Buscar si el nivel ya existe
            EstrellaNivel existing = dataList.niveles.Find(n => n.nivel == level);
            if (existing != null)
            {
                existing.estrellas = stars; // actualizar
            }
            else
            {
                dataList.niveles.Add(new EstrellaNivel { nivel = level, estrellas = stars }); // agregar
            }

            string json = JsonUtility.ToJson(dataList, true);
            File.WriteAllText(filePath, json);
            Debug.Log("Estrellas actualizadas en: " + filePath);
        }


        public static int LoadStars(int level) {
            EstrellaDataList dataList = LoadAllStars();

            EstrellaNivel result = dataList.niveles.Find(n => n.nivel == level);
            if (result != null)
            {
                return result.estrellas;
            }
            else
            {
                Debug.Log($"Nivel {level} no encontrado. Se devuelve 0 por defecto.");
                return 0;
            }
        }

        public static EstrellaDataList LoadAllStars()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonUtility.FromJson<EstrellaDataList>(json);
            }
            else
            {
                return new EstrellaDataList();
            }
        }
    }
}
