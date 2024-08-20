using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    [SerializeField]
    private Text _highScoreText;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public Text ScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int _score;

    private bool m_GameOver = false;

    private int Score
    {
        get => _score;
        set
        {
            _score = value;
            ScoreText.text = $"Score: {_score}";
        }
    }
    private GameManager Main => GameManager.Instance;

    // Start is called before the first frame update
    void Start()
    {
        if (Main.BestScore != null)
        {
            _highScoreText.gameObject.SetActive(true);
            _highScoreText.text = $"Best Score: {Main.BestScore.Name} - {Main.BestScore.Score}";
        }
        BuildBricks();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void BuildBricks()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    void AddPoint(int point) => Score += point;

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        Main.SaveScoreIfIsHigher(Score);
    }
}
