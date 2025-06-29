using DG.Tweening;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelData[] levels; 
    private int currentLevelIndex = 0;
    public GameObject playerPrefab;

    private GameObject currentMap;
    private GameObject currentPlayer;

    void Start()
    {
        LoadLevel(currentLevelIndex); 
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= levels.Length)
        {
            Debug.LogError("Level index out of bounds!");
            return;
        }

        currentLevelIndex = levelIndex;

        GameManager.Instance.ResetGame();

        if (currentMap != null)
        {
            Destroy(currentMap);
        }

        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }


        currentMap = Instantiate(levels[currentLevelIndex].mapPrefab);


        currentPlayer = Instantiate(playerPrefab, levels[currentLevelIndex].playerSpawnPosition, Quaternion.identity);
    }
    public void ContinueGame()
    {
        HomeFalse();  
        LoadLevel(currentLevelIndex);  
    }
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject homePanel;

    public void HomeFalse()
    {
        if (winPanel != null)
        {
            DOTween.Kill(winPanel);
            winPanel.SetActive(false); 
        }
        if (losePanel != null)
        {
            DOTween.Kill(losePanel);
            losePanel.SetActive(false);
        }
        if (homePanel != null)
        {
            homePanel.SetActive(false); 
        }
    }

    public void LoadNextLevel()
    {
        HomeFalse();

        currentLevelIndex++;
        if (currentLevelIndex < levels.Length)
        {
            LoadLevel(currentLevelIndex);
        }
        else
        {
            Debug.Log("Game Completed!");
        }
    }
    public void PlayAgain()
    {
        HomeFalse();

        DOTween.KillAll();

        LoadLevel(currentLevelIndex);  
    }
}
