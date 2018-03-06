using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalPaltformBehavior : MonoBehaviour {
    [SerializeField] Vector2 leftPosition;
    [SerializeField] Vector2 rightPosition;
    [SerializeField] public float rightSpeed;
    [SerializeField] public float leftSpeed;
    [SerializeField] public float platformSpeed;
    public Vector3 direction;


    // Use this for initialization
    void Start()
    {
        StartCoroutine(Move(leftPosition));
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Move(Vector3 target)
    {
        while (Mathf.Abs((target - transform.localPosition).x) > 0.2f)
        {
            direction = target.x == rightPosition.x ? rightSpeed * Vector3.right : leftSpeed * Vector3.left;
            transform.localPosition += platformSpeed * direction * Time.deltaTime;
            
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        Vector3 newTarget = target.x == leftPosition.x ? rightPosition : leftPosition;
        StartCoroutine(Move(newTarget));
        
    }
}
