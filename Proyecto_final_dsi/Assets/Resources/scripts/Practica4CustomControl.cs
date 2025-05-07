using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Practica4CustomControl : VisualElement {
    private string nombre;
    private int estado;
    private bool custom;

    private VisualElement container = new VisualElement();
    private Label texto = new Label();
    private List<VisualElement> elementos = new List<VisualElement>();

    public int Estado {
        get => estado;
        set {
            estado = Mathf.Clamp(value, 0, 5);
            UpdateUI();
        }
    }

    public string Nombre {
        get => nombre;
        set {
            nombre = value;
            UpdateUI();
        }
    }

    public bool Custom {
        get => custom;
        set {
            custom = value;
            UpdateUI();
        }
    }

    public new class UxmlFactory : UxmlFactory<Practica4CustomControl, UxmlTraits> { }

    public new class UxmlTraits : VisualElement.UxmlTraits {
        UxmlIntAttributeDescription myEstado = new UxmlIntAttributeDescription { name = "estado", defaultValue = 0 };
        UxmlStringAttributeDescription myNombre = new UxmlStringAttributeDescription { name = "nombre", defaultValue = "sun" };
        UxmlBoolAttributeDescription myCustom = new UxmlBoolAttributeDescription { name = "custom", defaultValue = false };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
            base.Init(ve, bag, cc);
            var control = ve as Practica4CustomControl;

            control.Estado = myEstado.GetValueFromBag(bag, cc);
            control.Nombre = myNombre.GetValueFromBag(bag, cc);
            control.Custom = myCustom.GetValueFromBag(bag, cc);
        }
    }

    public Practica4CustomControl() {
        style.flexGrow = 1; // Ocupa todo el espacio disponible
        style.flexDirection = FlexDirection.Column;
        style.alignItems = Align.Stretch;
        style.justifyContent = Justify.Center;

        texto.style.unityFontStyleAndWeight = FontStyle.Bold;
        texto.style.fontSize = 20;
        texto.style.alignSelf = Align.Center;

        container.style.flexDirection = FlexDirection.Row;
        container.style.alignItems = Align.Stretch;
        container.style.flexGrow = 1; // Hace que los elementos dentro ocupen todo el espacio

        Add(texto);
        Add(container);

        CrearElementos();
        UpdateUI();
    }

    private void CrearElementos() {
        for (int i = 0; i < 5; i++) {
            VisualElement elem = new VisualElement();
            elem.style.flexGrow = 1; // Se expande para llenar el espacio disponible
            elem.style.height = Length.Percent(100); // Ocupa toda la altura disponible

            // Asignar imagen (cargar desde Resources)
            elem.style.backgroundImage = new StyleBackground(Resources.Load<Sprite>("Images/sun"));

            // Ajuste de la imagen (escala completa sin repetir)
            elem.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;

            container.Add(elem);
            elementos.Add(elem);
        }
    }



    private void UpdateUI() {
        if (!custom) {
            estado = Random.Range(0, 6);
        }

        UpdateLabel();
        UpdateSprites();
        UpdateElements();
    }

    private void UpdateSprites() {
        Sprite sprite = Resources.Load<Sprite>($"Images/{nombre}");
        if (sprite != null) {
            foreach (var elem in elementos) {
                elem.style.backgroundImage = new StyleBackground(sprite.texture);
            }
        }
    }

    private void UpdateElements() {
        for (int i = 0; i < 5; i++) {
            elementos[i].style.opacity = (i < estado) ? 1f : 0.3f;
        }
    }

    private void UpdateLabel() {
        texto.text = $"{nombre} {estado}:";
    }
}
