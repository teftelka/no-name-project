using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameOverScreen gameOverScreen;
    private bool gameHasEnded = false;
    private float delay = 1f;

    public void EndGame()
    {
        if (!gameHasEnded)
        {
            gameHasEnded = true;
            Debug.Log("GAME OVER");
            Invoke(nameof(Setup), delay);
        }
    }

    private void Setup()
    {
        gameOverScreen.Setup();
    }
}
