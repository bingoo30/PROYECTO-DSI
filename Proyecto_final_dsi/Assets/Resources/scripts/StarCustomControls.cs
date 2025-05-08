using UnityEngine;
using UnityEngine.UIElements;

public class StarCustomControls : VisualElement
{
    public event System.Action<int> OnValueChanged;

    VisualElement container = new VisualElement();
    Texture2D star;
    private int currentValue;
    private bool isInteractive;
    public int Valor
    {
        get => currentValue;
        set
        {
            int newValue = Mathf.Clamp(value, 0, 3);
            if (newValue != currentValue)
            {
                currentValue = newValue;
                mostrarValor();
            }
        }
    }
    public bool Interactivo
    {
        get => isInteractive;
        set
        {
            isInteractive = value;
            esInteractivo();
        }
    }
    void mostrarValor()
    {
        container.Clear();

        for (int i = 0; i < 3; i++)
        {
            var elemento = new VisualElement();

            elemento.AddToClassList("star");

            elemento.style.backgroundImage = star;

            if (i < currentValue)
            {
                elemento.style.unityBackgroundImageTintColor = Color.white;
            }
            else
            {
                elemento.style.unityBackgroundImageTintColor = new Color(0.5f, 0.5f, 0.5f, 1); // gris
            }

            container.Add(elemento);
        }
    }

    private void esInteractivo()
    {
        foreach (var star in container.Children())
        {
            star.UnregisterCallback<ClickEvent>(OnStarClicked);

            if (isInteractive)
            {
                star.RegisterCallback<ClickEvent>(OnStarClicked);
            }
        }
    }
    private void OnStarClicked(ClickEvent evt)
    {
        var start = evt.currentTarget as VisualElement;
        int clickedStar = container.IndexOf(start) + 1;

        Valor = (Valor == clickedStar) ? clickedStar - 1 : clickedStar;
        OnValueChanged?.Invoke(Valor);

        evt.StopPropagation();
    }

    public new class UxmlFactory : UxmlFactory<StarCustomControls, UxmlTraits> { };
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIntAttributeDescription myValor = new UxmlIntAttributeDescription { name = "valor", defaultValue = 0 };
        UxmlBoolAttributeDescription myInteractive= new UxmlBoolAttributeDescription { name = "interactivo", defaultValue = false };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var star = ve as StarCustomControls;
            star.Valor = myValor.GetValueFromBag(bag, cc);
            star.Interactivo=myInteractive.GetValueFromBag(bag,cc);
        }
    }

    public StarCustomControls()
    {
        container.style.flexDirection = FlexDirection.Row;
        container.style.alignItems = Align.Center;

        hierarchy.Add(container);

        styleSheets.Add(Resources.Load<StyleSheet>("styles/MenuNiveles"));
        star = Resources.Load<Texture2D>("sprites/star");

        mostrarValor();
    }
}