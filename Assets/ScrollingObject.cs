using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        GameManager.Instance.scrollingObjects.Add(gameObject);
    }

    private void OnDestroy() {
        GameManager.Instance.scrollingObjects.Remove(gameObject);
    }
}
