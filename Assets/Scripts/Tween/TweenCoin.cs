using UnityEngine;
using DG.Tweening;

public class TweenCoin : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 randomPosition = transform.position
            + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

        transform.DOJump(randomPosition, 1.2f, 1, 0.4f).SetLink(gameObject);
        transform.DORotate(new Vector3(0f, 360f, 0f), 0.4f, RotateMode.FastBeyond360).SetLink(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
