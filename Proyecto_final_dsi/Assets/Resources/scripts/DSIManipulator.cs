using UnityEngine;
using UnityEngine.UIElements;

public class DSIManipulator : MouseManipulator
{
    public DSIManipulator()
    {
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.RightMouse });
    }
    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
    }
    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
    }
    private void OnMouseDown(MouseDownEvent mev)
    {
        if (CanStartManipulation(mev))
        {
            Debug.Log(target.name + ": Click en Elemento");

            // Obtener el contenedor padre
            VisualElement parent = target.parent;
            if (parent != null)
            {
                foreach (VisualElement child in parent.Children())
                {
                    ResetSelection(child);
                }
            }

            // Aplicar estilo de selección al elemento actual
            SelectElement(target);
            mev.StopPropagation();
        }
    }
    private void SelectElement(VisualElement element)
    {
        element.style.borderLeftColor = Color.white;
        element.style.borderRightColor = Color.white;
        element.style.borderTopColor = Color.white;
        element.style.borderBottomColor = Color.white;
    }

    private void ResetSelection(VisualElement element)
    {
        element.style.borderLeftColor = Color.black;
        element.style.borderRightColor = Color.black;
        element.style.borderTopColor = Color.black;
        element.style.borderBottomColor = Color.black;
    }
}
