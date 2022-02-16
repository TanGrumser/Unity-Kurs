using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSegment : MonoBehaviour {
    float timer = 20f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        timer -= Time.deltaTime;

        if (timer <= 0f) {
            Destroy(gameObject);
        }
    }
}
