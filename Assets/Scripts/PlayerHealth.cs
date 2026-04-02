using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    public int maxLives = 3; //최대 생명력
    public int currentLives; // 현재 생명력


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLives = maxLives; //생명력 초기화
    }
    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Blobfish"))
        {
            currentLives--;
            Destroy(other.gameObject); //미사일과 충돌시 미사일 오브젝트 삭제

            if(currentLives <= 0)
            {
                GameOver();
            }
        }
    }

    void GameOver()
    {
        gameObject.SetActive(false);
        Invoke("RestarGame", 3.0f);
    }    

    void RestarGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //씬 재시작
    }
}
