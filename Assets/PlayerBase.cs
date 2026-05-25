using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerBase : MonoBehaviour
{
    public int hp = 100;

    public GameObject gameOverScreen;
    public TextMeshProUGUI hpText;

    void Start()
    {
        Time.timeScale = 1f;
        UpdateHPText();
    }

    public void ReceiveDamage(int damage)
    {
        hp -= damage;
        UpdateHPText();

        if (hp <= 0)
        {
            GameOver();
        }
    }

    void UpdateHPText()
    {
        if (hpText != null)
        {
            hpText.text = "BASE HP: " + hp.ToString();
        }
    }

    void GameOver()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}