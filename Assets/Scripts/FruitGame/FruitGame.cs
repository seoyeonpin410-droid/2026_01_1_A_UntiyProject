using UnityEngine;

public class FruitGame : MonoBehaviour
{

    public GameObject[] fruitPerfabs;                   //과일 프리팹 배열 선언
    public float[] fruitSize = { 0.5f, 0.7f, 0.9f, 1.1f, 1.3f, 1.5f, 1.7f, 1.9f }; //과일 크기 선언

    public GameObject currentFruit;                     //현재 들고 있는 과일
    public int currentFruitType;                        //현재 들고 있는 과일 타입

    public float fruitStartHeigt = 6.0f;                //과일 시작시 높이 설정
    public float gameWidth = 6.0f;                      //게임판 너비
    public bool isGameOver = false;                    //게임 상태
    public Camera mainCamera;                           //카메라 참조

    public float fruitTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        SpawnnewFruit();
        fruitTimer = -3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)return;
        
        if (fruitTimer >= 0)
        {
            fruitTimer -= Time.deltaTime;
        }

        if (fruitTimer < 0 && fruitTimer > -2)
        {
            SpawnnewFruit();
            fruitTimer = -3.0f;
        }

        if(currentFruit != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition); 

            Vector3 newPosition = currentFruit.transform.position;

            newPosition.x = worldPosition.x;

            float halfFruitSize = fruitSize[currentFruitType] / 2f;

            if(newPosition.x <-gameWidth / 2 + halfFruitSize)
            {
                newPosition.x = -gameWidth / 2 + halfFruitSize;
            }

            if (newPosition.x > gameWidth / 2 + halfFruitSize)
            {
               newPosition.x = gameWidth / 2 + halfFruitSize;
            }

            currentFruit.transform.position = newPosition;
        }

        if (Input.GetMouseButtonDown(0) && fruitTimer == -3.0f)
        {
            DropFruit();
        }
    }

    void SpawnnewFruit()
    {
        if (!isGameOver)
        {
            currentFruitType = Random.Range(0, 3);

            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            Vector3 spawnPosition = new Vector3(worldPosition.x, fruitStartHeigt, 0);

            float halfFruitSize = fruitSize[currentFruitType] / 2f;

            //x의 위치가 게임 영역을 벗어나지 않도록 제한 

            spawnPosition.x = Mathf.Clamp(spawnPosition.x, -gameWidth / 2 + halfFruitSize, gameWidth / 2 - halfFruitSize);

            currentFruit = Instantiate(fruitPerfabs[currentFruitType], spawnPosition, Quaternion.identity); //과일 생성
            currentFruit.transform.localScale = new Vector3(fruitSize[currentFruitType], fruitSize[currentFruitType] , 1); //과일 크기 설정

            Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();

            if(rb != null)
            {
                rb.gravityScale = 0.0f;
            }
             
        }
    }

    void DropFruit()
    {
        Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
        if ( rb != null)
        {
            rb.gravityScale = 1.0f;
            currentFruit = null;
            fruitTimer = 1.0f;
        }
    }

    public void MergeFruites(int fruitType, Vector3 position)
    {
        if (fruitType < fruitPerfabs.Length - 1)          //마지막 과일 타입이 아니라면
        {
            GameObject newFruite = Instantiate(fruitPerfabs[fruitType + 1], position, Quaternion.identity);   //해당 타입 다음 번호를 생성 시킨다.
            newFruite.transform.localScale = new Vector3(fruitSize[fruitType + 1], fruitSize[fruitType + 1], 1.0f);

            //점수 추가 로직 등등
        }
    }
}
