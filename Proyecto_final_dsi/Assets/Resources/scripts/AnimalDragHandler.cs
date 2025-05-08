using UnityEngine;
using UnityEngine.UIElements;

public class AnimalDragHandler : PointerManipulator
{
    private Vector3 _mStart;
    protected bool _mActive;
    private int _mIndex;
    private Vector2 _mStartSize;

    public AnimalDragHandler()
    {
        _mIndex = -1;
        _mActive = false;
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
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

        if (CanStartManipulation(e))
        {
            _mStart = e.localPosition;
            _mIndex = e.pointerId;
            _mActive = true;
            target.CapturePointer(_mIndex);
            e.StopPropagation();
        }
    }
    protected void OnPointerMove(PointerMoveEvent e)
    {
        if (!_mActive || !target.HasPointerCapture(_mIndex)) return;

        Vector2 diff = e.localPosition - _mStart;

        target.style.top = target.layout.y + diff.y;
        target.style.left = target.layout.x + diff.x;

        e.StopPropagation();
    }
    protected void OnPointerUp(PointerUpEvent e)
    {
        if (!_mActive || !target.HasPointerCapture(_mIndex) || !CanStopManipulation(e)) return;

        _mActive = false;
        target.ReleasePointer(_mIndex);
        _mIndex = -1;
        e.StopPropagation();

       // colocamos el animal

       //  Obtener posición del mouse en pantalla
       // Vector2 mouseScreenPos = e.position;

       // Convertir a coordenadas de mundo
       //Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, 10f)); // 10f es la distancia z desde la cámara

       // Debug.Log($"Instanciando animal en {worldPos}");

       // Instanciar animal
       // if (animalPrefab != null)
       // {
       //     GameObject.Instantiate(animalPrefab, worldPos, Quaternion.identity);
       // }
    }
}

