using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatformBehaviour : MonoBehaviour {
    [SerializeField] Vector2 upPosition;
    [SerializeField] Vector2 downPosition;
    [SerializeField] public float upSpeed;
    [SerializeField] public float downSpeed;
    [SerializeField] public float platformSpeed;
    public Vector3 direction;


    // Use this for initialization
    void Start()
    {
        StartCoroutine(Move(upPosition));
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Move(Vector3 target)
    {
        while (Mathf.Abs((target - transform.localPosition).y) > 0.2f)
        {
            direction = target.y == downPosition.y ? downSpeed * Vector3.down : upSpeed * Vector3.up;
            transform.localPosition += platformSpeed * direction * Time.deltaTime;

            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        Vector3 newTarget = target.y == upPosition.y ? downPosition : upPosition;
        StartCoroutine(Move(newTarget));

    }
}
