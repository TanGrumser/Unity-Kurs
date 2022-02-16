using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour {

    [SerializeField] private float speed = 1f;
    
    // Update is called once per frame
    void Update() {
        transform.localPosition -= Vector3.right * Time.deltaTime * speed;

        if (transform.localPosition.x < -256) {
            transform.localPosition += Vector3.right * 256f;
        } 
    }
}
