using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int fruitType; //과일 이름 정의
    public bool hasMerged = false; //과일이 합치기 확인 플래그 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasMerged) //합쳐진 과일 무시
            return;

        Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();

        if (otherFruit != null && !otherFruit.hasMerged && otherFruit.fruitType == fruitType) //타입 확인
        {
            hasMerged = true; //합쳐짐 표시
            otherFruit.hasMerged = true;

            Vector3 mergePosition = (transform.position + otherFruit.transform.position) / 2f; //두 과일 중간 위치 계산

            // 게임 매니저에서 머지 구현 된 것을 호출 


            FruitGame gameManger = FindAnyObjectByType<FruitGame>();

            if (gameManger != null)
            {
                gameManger.MergeFruites(fruitType, mergePosition);
            }


            //과일 호출
            Destroy(otherFruit.gameObject);
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
