using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public List<GameObject> scrollingObjects;
    public GameObject[] levelSegments;
    public TextMeshProUGUI scoreText;

    private float score = 0;
    private float levelSegmentTimer = 0;

    public static GameManager Instance {get; private set;}
    private float scrollingSpeed = 5f;
    public bool gameOver = false;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
        }
    }

    private void Update () {
        levelSegmentTimer -= Time.deltaTime;
        if (levelSegmentTimer <= 0f) {
            levelSegmentTimer = 10f;
            Instantiate(levelSegments[0], transform.position + new Vector3(60f, 0f, 10f), Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.R) && gameOver) {
            SceneManager.LoadScene(0);
        }
    }

    // Update is called once per frame
    void LateUpdate() {
        if (!gameOver) {
            score += Time.deltaTime;
            scoreText.text = "Score: " + score.ToString("F1");
            
            transform.position += Vector3.right * Time.deltaTime * scrollingSpeed;
        }
    }

    private void ResetObjects() {
        foreach(GameObject scrollingObject in scrollingObjects) {
            scrollingObject.transform.position += Vector3.left * Time.deltaTime * 2f;
        }
    }
}
