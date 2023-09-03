using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallObjectAction : MonoBehaviour
{
    [SerializeField] private GameObject GO;
    [SerializeField] private Types gameObjectType;

    private bool inRange;
    private bool interacted;
    private Animator anim;

    enum Types
    {
        Door
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!interacted && inRange && Input.GetKeyUp(KeyCode.E))
        {
            CallAction();
        }
    }

    private void CallAction()
    {
        if (gameObjectType == Types.Door)
        {
            UpDownMovement upDownMovement = GO.GetComponent<UpDownMovement>();

            upDownMovement.Open();
        }

        anim.SetBool("interacted", true);
        interacted = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = false;
        }
    }

}
