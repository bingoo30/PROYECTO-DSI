using UnityEngine;
using UnityEngine.UIElements;

public class NumberCustomController : VisualElement
{
    VisualElement container = new VisualElement();

    Label label = new Label();

    int _value = 0;

    public int VALUE
    {
        get { return _value; }
        set
        {
            _value = value;
            switchOn();
        }
    }

    void switchOn()
    {
        // Actualizar etiquetas
        label.text = $"{_value}";
    }

    public new class UxmlFactory : UxmlFactory<NumberCustomController, UxmlTraits> { };

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIntAttributeDescription state = new UxmlIntAttributeDescription { name = "value", defaultValue = 0 };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var ad = ve as NumberCustomController;
            ad.VALUE = state.GetValueFromBag(bag, cc);
        }
    }

    private void create_label()
    {
        // Configurar etiquetas
        label.text = $": {_value}";
        label.AddToClassList("texto");
    }
    public NumberCustomController()
    {
        // Configuración de los contenedores principales
        container.style.flexDirection = FlexDirection.Row;

        create_label();

        // Agregar etiqueta a su contenedores
        container.Add(label);

        // Agregar el contenedor al UI Toolkit
        hierarchy.Add(container);
    }
}
