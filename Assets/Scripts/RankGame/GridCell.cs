using UnityEngine;

public class GridCell : MonoBehaviour
{

    public int x, y;
    public DraggableRank currentRank; //현재 칸에 있는 계급장
    public SpriteRenderer cellRenderers;

    public void Awake()
    {
        cellRenderers = GetComponent<SpriteRenderer>();
    }

    //좌표 초기화

    public void Initialize(int gridX, int gridY)
    {
        x = gridX;
        y = gridY;
        name = "Cell_" + x + "," + y; //이름 설정
    }



    public bool IsEmpty() //칸이 비어 있는지 확인 
    {
        return currentRank == null;
    }

    public bool ContainsPosition(Vector3 position)
    {
        Bounds bounds = cellRenderers.bounds;
        return bounds.Contains (position);
    }

    public void SetRank(DraggableRank rank)
    {
        currentRank = rank;

        if(rank != null)
        {
            rank.currentCell = this;
        }

        rank.originalPosition = new Vector3(transform.position.x, transform.position.y, 0);
        rank.transform.position = new Vector3(transform.position.x , transform.position.y , 0);
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



