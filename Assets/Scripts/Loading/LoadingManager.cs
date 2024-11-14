using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI title;
    
    private RectTransform rectTransform;
    private float textTweenAmount;
    private AudioController audioController;
    
    IEnumerator Start()
    {
        rectTransform = GetComponent<RectTransform>();
        textTweenAmount = rectTransform.rect.height / 2f + title.rectTransform.rect.height / 2f;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Gameplay", LoadSceneMode.Additive);
    }

    public void Hide()
    {
        background.DOFade(0, .2f).SetEase(Ease.InSine).SetDelay(.5f);
        title.rectTransform.DOAnchorPosY(-textTweenAmount, .7f)
            .SetEase(Ease.InBack).OnComplete(Disable);
    }

    public void Show()
    {
        background.enabled = true;
        title.enabled = true;
        
        title.rectTransform.anchoredPosition = new Vector2(title.rectTransform.anchoredPosition.x, textTweenAmount);
        background.DOFade(1, .5f).SetEase(Ease.OutSine);
        title.rectTransform.DOAnchorPosY(0, .7f).SetEase(Ease.InExpo).OnComplete(() =>
        {
            SceneManager.LoadScene("Boot");
        });;
    }
    
    private void Disable()
    {
        background.enabled = false;
        title.enabled = false;
    }
}
