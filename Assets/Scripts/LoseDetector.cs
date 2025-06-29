using UnityEngine;

public class LoseDetector : MonoBehaviour
{
    public CubePart[] parts;

    public void CheckNow()
    {
        if (GameManager.Instance.HasGameEnded()) return;

        foreach (CubePart part in parts)
        {
            if (!part.IsSupported())
            {
                Debug.Log("LOSE: Cube part không còn trên block!");
                GameManager.Instance.LoseGame(transform);
                enabled = false;
                break;
            }
        }
    }
}
