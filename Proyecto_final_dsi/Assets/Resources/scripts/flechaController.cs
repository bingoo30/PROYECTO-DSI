using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class flechaController : MonoBehaviour
{
    List<VisualElement> bichosContainers;
    //indice para acceder a bichito concreto
    int act_indice = 0;
    Button antes;
    Button despues;
    private void esconderTodo()
    {
        foreach (VisualElement v in bichosContainers)
        {
            v.style.display = DisplayStyle.None;
        }
    }
    private void actualizar(bool antes)
    {
        int ind =0;
        if (antes)
        {
            Debug.Log($"actual indice (antes): {act_indice}");
            //no podemos tener indices negativos
            ind = Mathf.Max(act_indice-1, 0);
        }
        else
        {
            //no podemos tener superiores al tamaño de la lista
            ind = Mathf.Min(act_indice + 1, bichosContainers.Count - 1);
        }

        //solo hacemos cosas cuando es valido
        if (ind >= 0 && ind < bichosContainers.Count)
        {
            //esconder todos los bichos
            esconderTodo();
            //actualizar el indicador y ponerlo como visible
            act_indice = ind;
            bichosContainers[act_indice].style.display = DisplayStyle.Flex;
        }
    }
    private void OnEnable()
    {
        VisualElement document = GetComponent<UIDocument>().rootVisualElement;
        //referenciar la lista de elementos
        bichosContainers = document.Q<VisualElement>("bichos").Children().ToList();
        antes = document.Q<Button>("antes");
        despues = document.Q<Button>("despues");

        despues.RegisterCallback<ClickEvent>(e =>
        {
            Debug.Log("click despues");
            actualizar(false);
        });
        antes.RegisterCallback<ClickEvent>(e => {
            Debug.Log("click antes");
            actualizar(true);
        });
    }
}
