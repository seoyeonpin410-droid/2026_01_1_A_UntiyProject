using UnityEngine;

public class ZAxisMover : MonoBehaviour
{

    public float spped = 5.0f;
    public float timer = 5.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, spped * Time.deltaTime);

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            Destroy(gameObject);
        }
    }
}
