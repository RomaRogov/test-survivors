using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementView : MonoBehaviour
{
    [SerializeField] private float movingRadius;
    [SerializeField] private Image circle;
    [SerializeField] private Image handle;

    private RectTransform rectTransform;
    private float size;
    
    public void InitWithSize(float size)
    {
        this.size = size;
        rectTransform = GetComponent<RectTransform>();
        Hide();
    }
    
    private IEnumerator Start()
    {
        while (size == 0)
            yield return null;
        
        if (rectTransform.parent is not RectTransform parent)
        {
            Debug.LogException(new Exception("Parent must be RectTransform"));
            yield break;
        }
        
        float minDimensity = Mathf.Min(parent.rect.width, parent.rect.height);
        float targetWidth = minDimensity * size;
        float scale = targetWidth / circle.rectTransform.rect.width;
        transform.localScale = Vector3.one * scale * 2f;
    }

    public void Show()
    {
        circle.enabled = handle.enabled = true;
        SetValue(Vector2.zero);
        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (rectTransform.parent as RectTransform, Input.mousePosition, null, out Vector2 pos);
        rectTransform.anchoredPosition = pos;
    }
    
    public void SetValue(Vector2 value)
    {
        handle.rectTransform.anchoredPosition = value * movingRadius;
    }
    
    public void Hide()
    {
        circle.enabled = handle.enabled = false;
    }
}
