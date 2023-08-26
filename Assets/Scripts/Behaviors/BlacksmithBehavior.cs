using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithBehavior : MonoBehaviour
{
    [SerializeField] private float idleTime = 2f;
    [SerializeField] private float actionTime = 5f;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {  
        StartCoroutine(DoAction());
    }

    private IEnumerator DoAction()
    {
        anim.SetBool("doAction", false);
        yield return new WaitForSeconds(idleTime);
        anim.SetBool("doAction", true);
        yield return new WaitForSeconds(actionTime);
        StartCoroutine(DoAction());
    }
}
