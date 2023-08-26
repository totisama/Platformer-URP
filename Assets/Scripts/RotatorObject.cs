using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorObject : MonoBehaviour
{
    [SerializeField] private float minimumAngle = -80.0f;
    [SerializeField] private float maximumAngle = 80f;
    [SerializeField] private float velocity = 1f;
    [SerializeField] private int damage = 5;

    private float t = 0.0f;

    void Update()
    {
        Vector3 rotation = new Vector3(0f, 0f, Mathf.Lerp(minimumAngle, maximumAngle, t));
        transform.rotation = Quaternion.Euler(rotation);

        t += velocity * Time.deltaTime;

        if (t > 1.0f)
        {
            float temp = maximumAngle;
            maximumAngle = minimumAngle;
            minimumAngle = temp;
            t = 0.0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable iDamageable = collision.gameObject.GetComponent<IDamageable>();

            if (iDamageable != null)
            {
                iDamageable.TakeDamage(damage, transform.position);
            }
        }
    }
}
