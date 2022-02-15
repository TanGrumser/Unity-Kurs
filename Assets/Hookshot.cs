using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookshot : MonoBehaviour {
    
    [SerializeField] LineRenderer line;
    public float speed = 2f;
    float timer = 1f;
    private bool locked = false;
    private GameObject parent;

    public void SetParent(GameObject parent) {
        this.parent = parent;
    }

    void Start() {
        line = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update() {
        timer -= Time.deltaTime;

        if (timer <= 0f) {
            Destroy(gameObject);
        }
        
        if (locked) {
            transform.position += transform.up * Time.deltaTime * speed;
        }

        line.SetPosition(1, transform.InverseTransformDirection(parent.transform.position - transform.position));
    }

    void OnCollisionEnter2D(Collider2D collider) {
        locked = true;
    }
}
