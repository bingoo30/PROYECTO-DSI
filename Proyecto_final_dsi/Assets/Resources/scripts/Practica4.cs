
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Practica4 : MonoBehaviour
{
    private int[] valores = new int[12];
    private void OnEnable()
    {
        VisualElement rootve = GetComponent<UIDocument>().rootVisualElement;

        VisualElement contenedor = rootve.Q("Contenedor");

        Sprite bg = Resources.Load<Sprite>("Images/bg2");

        contenedor.style.backgroundImage = new StyleBackground(bg);
        VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>("Templates/Practica4Template");

        VisualElement _template = visualTreeAsset.Instantiate();

        _template.style.minHeight = 600;


        VisualElement _flor1 = _template.Q("Flower1");
        VisualElement _flor2 = _template.Q("Flower2");
        VisualElement _flor3 = _template.Q("Flower3");



        contenedor.Add(_template);


        Sprite img_flor1 = Resources.Load<Sprite>("Images/flor");
        Sprite img_flor2 = Resources.Load<Sprite>("Images/flor2");
        Sprite img_flor3 = Resources.Load<Sprite>("Images/narciso");
        Sprite img_sun = Resources.Load<Sprite>("Images/sun");
        Sprite img_water = Resources.Load<Sprite>("Images/water");

        _flor1.style.backgroundImage = new StyleBackground(img_flor1);
        _flor2.style.backgroundImage = new StyleBackground(img_flor2);
        _flor3.style.backgroundImage = new StyleBackground(img_flor3);

        List<VisualElement> suns = contenedor.Query<VisualElement>("Sun").ToList();

        List<Label> texts = contenedor.Query<Label>("Label").ToList();
        
        texts.ForEach(elem =>
        {
            elem.style.maxHeight = 10;
            elem.style.minWidth = 10;
        });


        suns.ForEach(elem =>
        {
            elem.style.backgroundImage = new StyleBackground(img_sun);
            elem.style.maxHeight = 20;
            elem.style.minWidth = 20;
        });

        List<VisualElement> waters = contenedor.Query<VisualElement>("water").ToList();

        waters.ForEach(elem =>
        {
            elem.style.backgroundImage = new StyleBackground(img_water);
        });

        for (int i = 0; i < 12; i++)
        {
            valores[i] = UnityEngine.Random.Range(0,5);
            Debug.Log(valores[i]);

        }


        List<Label> suns_t = contenedor.Query<Label>("sun_t").ToList();

        List<Label> waters_t = contenedor.Query<Label>("water_t").ToList();
        
        int j = 0;
        suns_t.ForEach(elem =>
        {
            elem.text = "Sun " + valores[j] + ":" ;
            Debug.Log(elem.text);
            j++;
        });

        waters_t.ForEach(elem =>
        {
            elem.text = "Water " + valores[j] + ":";
            j++;
        });

        //VisualElement pa = target.parent;

        //List<VisualElement> list = pa.Children().ToList();
        //list.ForEach(v => {
        //    v.style.borderBottomColor = Color.black;
        //    v.style.borderLeftColor = Color.black;
        //    v.style.borderRightColor = Color.black;
        //    v.style.borderTopColor = Color.black;
        //});

        //Debug.Log(target.name + ": Click en Elemento");
        //if (CanStartManipulation(mev))
        //{
        //    target.style.borderBottomColor = Color.white;
        //    target.style.borderLeftColor = Color.white;
        //    target.style.borderRightColor = Color.white;
        //    target.style.borderTopColor = Color.white;
        //    mev.StopPropagation();
        //}
    }
}
