﻿using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool gameEnded = false;
    public GameObject winPanel;
    public GameObject losePanel;
    public Transform[] winPanelButtons;
    private Vector3[] originalButtonScales;
    private Vector3 originalScale = Vector3.one;
    public GameObject homePanel;
    public GameObject howPanel;

    public LevelManager levelManager;

    public AudioManager audioManager;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        homePanel.SetActive(true);
    }

    public void WinGame(Transform boxTransform)
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("YOU WIN!");
        DOTween.Kill(boxTransform);
        boxTransform.GetComponent<CubeMove>().enabled = false;
        audioManager.PlayWinSound();

        boxTransform.DOMoveY(-5f, 0.8f)
            .SetEase(Ease.InBack)
            .SetTarget(boxTransform)
            .OnComplete(() =>
            {
                Debug.Log(" Complete ");
                ShowWinPanel();
            });

    }

    private void ShowWinPanel()
    {
        if (winPanel == null) return;

        winPanel.transform.localScale = Vector3.zero;
        winPanel.SetActive(true);

        if (originalButtonScales == null || originalButtonScales.Length != winPanelButtons.Length)
        {
            originalButtonScales = new Vector3[winPanelButtons.Length];
            for (int i = 0; i < winPanelButtons.Length; i++)
            {
                originalButtonScales[i] = winPanelButtons[i].localScale;
            }
        }

        winPanel.transform.DOScale(Vector3.one, 0.6f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                float delay = 0f;
                for (int i = 0; i < winPanelButtons.Length; i++)
                {
                    var btn = winPanelButtons[i];
                    btn.localScale = Vector3.zero; 

                    btn.DOScale(originalButtonScales[i], 0.4f)
                       .SetEase(Ease.OutBack)
                       .SetDelay(delay);

                    delay += 0.1f;
                }
            });
    }

    public void LoseGame(Transform box)
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("LOSE!");
        DOTween.Kill(box);

        box.DORotate(new Vector3(0, 0, 360f), 1f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear)
            .SetTarget(box);

        box.DOMoveY(box.position.y - 5f, 1f)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.InBack)
            .SetTarget(box);

        Invoke(nameof(ShowLosePanel), 2f);
        audioManager.PlayLoseSound();
    }

    private void ShowLosePanel()
    {
        if (losePanel == null) return;

        CanvasGroup cg = losePanel.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = losePanel.AddComponent<CanvasGroup>();
        }

        cg.alpha = 0f;
        losePanel.transform.localScale = Vector3.one * 0.8f;
        losePanel.SetActive(true);

        Sequence seq = DOTween.Sequence();
        seq.Append(cg.DOFade(1f, 0.8f).SetEase(Ease.OutQuad));
        seq.Join(losePanel.transform.DOScale(1f, 0.8f).SetEase(Ease.OutSine));
    }
    public void HowToPlay()
    {
        DOTween.Kill(howPanel.transform); 
        DOTween.Kill(homePanel.transform);

        howPanel.SetActive(true);
        homePanel.SetActive(false);

        howPanel.transform.localScale = Vector3.zero;

        howPanel.transform.DOScale(originalScale, 0.5f)
            .SetEase(Ease.OutBack)
            .SetTarget(howPanel.transform);
    }

    public void Home()
    {
        DOTween.Kill(howPanel.transform);
        DOTween.Kill(homePanel.transform);

        homePanel.SetActive(true);
        howPanel.SetActive(false);

        homePanel.transform.localScale = Vector3.zero;

        homePanel.transform.DOScale(originalScale, 0.5f)
            .SetEase(Ease.OutBack)
            .SetTarget(homePanel.transform);
    }
    public bool HasGameEnded()
    {
        return gameEnded;
    }
    public void ResetGame()
    {
        gameEnded = false;     
    }

}
