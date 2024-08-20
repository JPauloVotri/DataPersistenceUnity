using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _bestScoreText;

    [SerializeField]
    private TMP_InputField _nameInput;

    private GameManager Main => GameManager.Instance;

    private void Start()
    {
        if (Main.BestScore == null)
            return;
        _bestScoreText.gameObject.SetActive(true);
        _bestScoreText.text = $"Best Score: {Main.BestScore.Name} - {Main.BestScore.Score}";
    }

    public void StartGame()
    {
        Main.PlayerName = _nameInput.text;
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
