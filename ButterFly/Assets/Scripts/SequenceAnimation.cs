using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceAnimation : MonoBehaviour
{

    public float WaitBetween = 0.15f;
    public float WaitEnd = 0.15f;
    List<Animator> _animators;

    // Start is called before the first frame update
    void Start()
    {
        _animators = new List<Animator>(GetComponentsInChildren<Animator>());
        StartCoroutine(DoAnimation());
    }

    // Update is called once per frame
    IEnumerator DoAnimation()
    {
        while (true)
        {
            foreach(var animator in _animators)
            {
                Debug.Log("ok1");
                animator.SetTrigger("OnAnimation");
                yield return new WaitForSeconds(WaitBetween);
            }

            yield return new WaitForSeconds(WaitEnd);
        }
    }
}
