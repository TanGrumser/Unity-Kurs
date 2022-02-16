using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookshotSegment : ScrollingObject {
    
    protected HookshotSegment parent;
    private LineRenderer line;
    private new Rigidbody2D rigidbody2D;
    private float distance;

    public Rigidbody2D Rigidbody2D {
        get {
            if (rigidbody2D == null) {
                rigidbody2D = GetComponent<Rigidbody2D>();
            }

            return rigidbody2D;
        }
    }
    
    // Start is called before the first frame update
    private void Awake() {
        line = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    protected virtual void Update() {
        if (line != null) {
            if (parent != null) {
                line.SetPosition(1, transform.InverseTransformDirection(parent.transform.position - transform.position));
            } else {
                line.enabled = false;
            }
        } 
    }

    public void SetParentHookshotSegment(HookshotSegment parentSegment) {
        parent = parentSegment;
        parentSegment.SetConnectedBody(this);
    }

    public void RemoveParent() {
        parent = null;
    }

    private void SetConnectedBody(HookshotSegment connectedBody) {
        AnchoredJoint2D distanceJoint = GetComponent<AnchoredJoint2D>();
        distanceJoint.enabled = true;
        distanceJoint.connectedBody = connectedBody.Rigidbody2D;
    }
    
    public virtual void DestroyRecursive() {
        if (parent != null) {
            parent.DestroyRecursive();
            StartCoroutine(DestroyingRoutine());
        }
    }

    protected virtual IEnumerator DestroyingRoutine() {
        const float TIME_TO_DESTROY = 0.5f;
        float timer = TIME_TO_DESTROY;

        while (timer > 0f) {
            float alpha = timer / TIME_TO_DESTROY;
            Gradient gradient = new Gradient();
            
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) },
                new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
            
            if (line != null) {
                line.colorGradient = gradient;
            }
            
            timer -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
