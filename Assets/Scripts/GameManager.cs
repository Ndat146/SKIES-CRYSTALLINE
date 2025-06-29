using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void WinGame(Transform boxTransform)
    {
        Debug.Log("YOU WIN!");

        boxTransform.GetComponent<CubeMove>().enabled = false;

        boxTransform.DOMoveY(-5f, 0.8f).SetEase(Ease.InBack).OnComplete(() =>
        {
            Debug.Log("Fall complete - show win UI here");
        });
    }
}
