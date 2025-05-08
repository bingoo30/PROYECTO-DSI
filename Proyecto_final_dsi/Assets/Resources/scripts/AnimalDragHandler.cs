using UnityEngine;
using UnityEngine.UIElements;

public class AnimalDragHandler : PointerManipulator
{
    private Vector3 _mStart;
    protected bool _mActive;
    private int _mIndex;
    private Vector2 _mStartSize;


    // Referencia al prefab que vamos a instanciar
    public GameObject animalPrefab;

    // Instancia del prefab
    private GameObject _animalInstance;

    public AnimalDragHandler()
    {
        _mIndex = 0;
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        _mActive = false;
        animalPrefab = Resources.Load<GameObject>("prefabs/pepe");
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(OnPointerDown);
        target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        target.RegisterCallback<PointerUpEvent>(OnPointerUp);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
        target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
        target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
    }

    protected void OnPointerDown(PointerDownEvent e)
    {
        if (_mActive)
        {
            e.StopImmediatePropagation();
            return;
        }
        _mStart = e.localPosition;
        _mStartSize = target.layout.size;
        _mIndex = e.pointerId;
        _mActive = true;

        if (animalPrefab != null)
        {
            // Convertir posición local a pantalla
            Vector3 screenPos = (Vector3)target.worldBound.position + e.localPosition;

            // Convertir a coordenadas de mundo (z = 0 para 2D)
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, Screen.height - screenPos.y, Camera.main.nearClipPlane));
            worldPos.z = 0; // aseguramos que esté en el plano 2D
            _animalInstance.layer = 6;
            _animalInstance = GameObject.Instantiate(animalPrefab, worldPos, Quaternion.identity);
            _animalInstance.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }

        target.CapturePointer(_mIndex);
        e.StopPropagation();
    }
    protected void OnPointerMove(PointerMoveEvent e)
    {
        if (!_mActive || !target.HasPointerCapture(_mIndex) || _animalInstance == null) return;

        // Convertir posición local a pantalla
        Vector2 screenPos = target.worldBound.position + (Vector2)e.localPosition;

        // Convertir a coordenadas de mundo
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, Screen.height - screenPos.y, Camera.main.nearClipPlane));
        worldPos.z = 0; // plano 2D

        _animalInstance.transform.position = worldPos;

        e.StopPropagation();
    }
    protected void OnPointerUp(PointerUpEvent e)
    {
        if (!_mActive || !target.HasPointerCapture(_mIndex) || !CanStopManipulation(e)) return;
        Debug.Log("up");
        _mActive = false;
        target.ReleasePointer(_mIndex);
        _mIndex = 0;
        _mStartSize = target.layout.size;
        e.StopPropagation();
    }

}