using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OseroCellView : MonoBehaviour
{
    [SerializeField] private EventTrigger _btn;
    [SerializeField] private Image _dotImage;
    [SerializeField] private Image _diskImage;
    Color Clear = new Color(1, 1, 1, 0);

    public (int, int) YX { get; private set; }

    public Action<(int, int)> OnClick;
    private float _lastClickTime;
    private void Awake()
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) =>
        {
            if (Time.time - _lastClickTime < 0.2f) return;
            _lastClickTime = Time.time;
            OnClick?.Invoke(YX);
        });
        _btn.triggers.Add(entry);
    }

    public void Init((int, int) yx, Action<(int, int)> onClick)
    {
        YX = yx;
        OnClick = onClick;
        Reset();
    }

    private void Reset()
    {
        SetDot(Clear);
        SetDisk(Clear);
    }
    public void SetDot(Color color)
    {
        _dotImage.color = color;
    }
    public void SetDisk(Color color)
    {
        _diskImage.color = color;
    }
}
