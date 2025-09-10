using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI ScoreText;

    public void Setup(int finalScore) {
        ScoreText.gameObject.SetActive(false);
        gameObject.SetActive(true);
        finalScoreText.text = "Final Score: " + finalScore;
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
