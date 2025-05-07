using UnityEngine;
using UnityEngine.UIElements;

public class StarCustomControls : VisualElement
{
    VisualElement container = new VisualElement();
    Texture2D star;
    int v;
    public int Valor
    {
        get => v;
        set
        {
            v = Mathf.Clamp(value, 0, 3);
            mostrarValor();
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

            if (i < v)
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

    public new class UxmlFactory : UxmlFactory<StarCustomControls, UxmlTraits> { };
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIntAttributeDescription myValor = new UxmlIntAttributeDescription { name = "valor", defaultValue = 0 };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var star = ve as StarCustomControls;
            star.Valor = myValor.GetValueFromBag(bag, cc);
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