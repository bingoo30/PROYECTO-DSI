using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;


public class MenuJuego : MonoBehaviour
{
    private void OnEnable()
    {
        int i = 0;
        UIDocument document = GetComponent<UIDocument>();

        VisualElement rootve = document.rootVisualElement;

        List<VisualElement> bichos = rootve.Q("bichos").Children().ToList();
        bichos.ForEach(b =>
        {
            List<VisualElement> x = b.Children().ToList();
            x.ForEach(e =>
            {
                e.pickingMode = PickingMode.Position;
                e.AddManipulator(new DSIManipulator());
                e.AddManipulator(new AnimalDragHandler());
            });
        });
    }
}
