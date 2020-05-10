using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] float gameOverDelay = 1.5f;
    [SerializeField] float loadNextLevelDelay = 1.5f;
    public void LoadStartMenu() => SceneManager.LoadScene(0);

    public void LoadMainGame()
    {
        SceneManager.LoadScene("Level 1");
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void LoadGameOver()
    {
        StartCoroutine(GameOverCoroutine());
    } 
    IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(gameOverDelay);
        SceneManager.LoadScene("Game Over");
    }
    public void QuitGame() => Application.Quit();

    public void LoadNextLevel()
    {
        StartCoroutine(LoadNextLevelCoroutine());
    } 
    IEnumerator LoadNextLevelCoroutine()
    {
        yield return new WaitForSeconds(loadNextLevelDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
