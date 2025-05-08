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
        _mIndex = 0;
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        _mActive = false;
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
            Debug.Log("pointer down");
            _mStart = e.localPosition;
            _mStartSize = target.layout.size;
            _mIndex = e.pointerId;
            _mActive = true;
            target.CapturePointer(_mIndex);
            e.StopPropagation();

        }
    }
    protected void OnPointerMove(PointerMoveEvent e)
    {
        if (!_mActive || !target.HasPointerCapture(_mIndex)) return;
        Debug.Log("pointer move");
        Vector2 diff = e.localPosition - _mStart;

        target.style.height = _mStartSize.y + diff.y;
        target.style.width = _mStartSize.x + diff.x;

        e.StopPropagation();
    }
    protected void OnPointerUp(PointerUpEvent e)
    {
        if (!_mActive || !target.HasPointerCapture(_mIndex) || !CanStopManipulation(e)) return;
        Debug.Log("pointer up");
        _mActive = false;
        target.ReleasePointer(_mIndex);
        _mIndex = 0;
        _mStartSize = target.layout.size;
        e.StopPropagation();
    }

}