using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {
    public GameObject[] plattforms;
    public TextMeshProUGUI scoreText;

    private float score = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        score += Time.deltaTime;
        scoreText.text = "Score: " + score.ToString("F1");
        
        foreach(GameObject plattform in plattforms) {
            plattform.transform.position += Vector3.left * Time.deltaTime * 2f;
        }
        
    }
}
