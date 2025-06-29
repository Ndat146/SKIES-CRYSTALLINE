using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool gameEnded = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void WinGame(Transform boxTransform)
    {
        if (gameEnded) return; 
        gameEnded = true;

        Debug.Log("YOU WIN!");

        var cubeMove = boxTransform.GetComponent<CubeMove>();
        if (cubeMove != null) cubeMove.enabled = false;

        boxTransform.DOMoveY(-5f, 0.8f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                Debug.Log("Fall complete - show win UI here");
                // TODO: Hiện UI thắng tại đây nếu cần
            });
    }

    public void LoseGame(Transform box)
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("LOSE!");

        box.DORotate(new Vector3(0, 0, 360f), 1f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);

        box.DOMoveY(box.position.y - 5f, 1f)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.InBack);
    }

    public bool HasGameEnded()
    {
        return gameEnded;
    }
}
