using stars_namespace;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static stars_namespace.JsonHelperEstrellas;

public class NivelData
{
    public int nivel;
    public int estrellas;
}

public class MenuNiveles : MonoBehaviour
{
    private List<NivelData> niveles = new List<NivelData>();
    private VisualElement root;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        var datosJSON = JsonHelperEstrellas.LoadAllStars();
        foreach (var estrellaNivel in datosJSON.niveles)
        {
            niveles.Add(new NivelData
            {
                nivel = estrellaNivel.nivel,
                estrellas = estrellaNivel.estrellas
            });
        }

        if (niveles.Count == 0)
        {
            InicializarNivelesPorDefecto();
        }

        ConfigurarTemplatesExistentes();
    }

    private void InicializarNivelesPorDefecto()
    {
        for (int i = 1; i <= 6; i++)
        {
            niveles.Add(new NivelData
            {
                nivel = i,
                estrellas = 0
            });
        }

        foreach (var nivel in niveles)
        {
            JsonHelperEstrellas.SaveStars(nivel.nivel, nivel.estrellas);
        }
    }

    private void ConfigurarTemplatesExistentes()
    {
        for (int i = 0; i < niveles.Count; i++)
        {
            var nivel = niveles[i];
            string nombreTemplate = $"botonNivel{nivel.nivel}";

            VisualElement template = root.Q<VisualElement>(nombreTemplate);

            if (template != null)
            {
                var contenedor = template.Q<VisualElement>("botonNivel");
                // Estrellas
                StarCustomControls estrellas = contenedor.Q<StarCustomControls>();
                if (estrellas != null)
                {
                    estrellas.Valor = nivel.estrellas >= 0 ? nivel.estrellas : 0;
                }
            }
        }
    }

    public void ActualizarEstrellas(int nivelNumero, int estrellas)
    {
        int index = nivelNumero - 1;
        if (index >= 0 && index < niveles.Count)
        {
            niveles[index].estrellas = estrellas;
            JsonHelperEstrellas.SaveStars(nivelNumero, estrellas);

            // Actualizar UI actual
            string templateActual = $"botonNivel{nivelNumero}";
            var estrellasControl = root.Q<VisualElement>(templateActual)?.Q<StarCustomControls>("estrellas");
            if (estrellasControl != null)
            {
                estrellasControl.Valor = estrellas;
            }
        }
    }
}
