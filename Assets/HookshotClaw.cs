using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookshotClaw : HookshotSegment {
    
    enum State {
        FLYING,
        ATTACHED
    }

    [SerializeField] private float flyingSpeed = 2f;
    [SerializeField] private GameObject hookshotSegmentPrefab;
    [SerializeField] private int segmentCount;

    private SpriteRenderer spriteRenderer;
    private State state = State.ATTACHED;
    float timer = 1f;

    private void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();

        if (state == State.FLYING) {
            transform.position += transform.up * Time.deltaTime * flyingSpeed;

            timer -= Time.deltaTime;

            if (timer <= 0f) {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collider2D collider) {
        state = State.ATTACHED;
    }

    public HookshotSegment AttachToPoint(Vector3 origin, Vector3 destination) {
        transform.position = destination;
        Vector3 direction = origin - destination;
        HookshotSegment segment = this;

        for (int i = 0; i < segmentCount; i++) {
            Vector3 segmentPosition = destination + (direction / segmentCount * (i + 1));
            GameObject hookshotSegmentObject = Instantiate(hookshotSegmentPrefab, segmentPosition, Quaternion.identity);
            HookshotSegment hookshotSegment = hookshotSegmentObject.GetComponent<HookshotSegment>();
            segment.SetParentHookshotSegment(hookshotSegment);
            segment.name = segment.name + i;
            segment = hookshotSegment;
        }

        return segment;
    }

    public override void DestroyRecursive() {
        parent.DestroyRecursive();
        StartCoroutine(base.DestroyingRoutine());
        StartCoroutine(DestroyingRoutine());
    }

    protected override IEnumerator DestroyingRoutine() {
        const float TIME_TO_DESTROY = 0.5f;
        float timer = TIME_TO_DESTROY;

        while (timer > 0f) {
            float alpha = timer / TIME_TO_DESTROY;
            
            spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
            timer -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
