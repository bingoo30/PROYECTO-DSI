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
        Debug.Log($"bichos detectados: {bichos.Count}");
        bichos.ForEach(b =>
        {
            List<VisualElement> x = b.Children().ToList();
            Debug.Log($"bicho hijo: {b.name}");
            x.ForEach(e =>
            {
                Debug.Log($"animal: {e.name}");
                e.pickingMode = PickingMode.Position;
                e.AddManipulator(new DSIManipulator());
                Debug.Log($"manipulador añadido en: {i++}");
            });
        });
    }
}
