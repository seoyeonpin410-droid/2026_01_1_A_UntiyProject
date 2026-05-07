using UnityEngine;
using System.Collections.Generic;  

public class RankGameManager : MonoBehaviour
{

    public int gridWidth = 7;
    public int gridHeight = 7;
    public float CellSize = 1.3f;
    public GameObject cellPrefabs;
    public Transform gridContainer;

    public GameObject rankPrefabs;
    public Sprite[] rankSprites;
    public int maxRankLevel = 7;

    public GridCell[,] grid;


    void InitalizeGrid()
    {
        grid = new GridCell[gridWidth, gridHeight];

        for (int x =0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 position = new Vector3
                (
                   x * CellSize -(gridWidth * CellSize /2) + CellSize /2,
                   y * CellSize - (gridWidth * CellSize / 2) + CellSize / 2,
                   1f
                );

                GameObject cellObj = Instantiate(cellPrefabs, position, Quaternion.identity, gridContainer);
                GridCell cell = cellObj.AddComponent<GridCell>();
                cell.Initialize(x, y);

                grid[x, y] = cell;
                    
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitalizeGrid();

        for (int i = 0; i < 4; i++)
        {
            SpawnNewRank();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            SpawnNewRank();
        }
    }

    public DraggableRank CreateRankInCell(GridCell cell, int level)
    {
        if (cell == null || !cell.IsEmpty()) return null;        //비어있는 칸이 아니면 생성 실패

        level = Mathf.Clamp(level, 1, maxRankLevel);             //레벨 범위 확인

        Vector3 rankPosition = new Vector3(cell.transform.position.x, cell.transform.position.y, 0f);    //계급장 위치 설정

        //드래그 가능한 계급장 컴포넌트를 추가

        GameObject rankObj = Instantiate(rankPrefabs, rankPosition, Quaternion.identity, gridContainer);
        rankObj.name = "Rank_Level_" + level;

        DraggableRank rank = rankObj.AddComponent<DraggableRank>();

        rank.SetRankLevel(level);

        cell.SetRank(rank);

        return rank;
    }

    private GridCell FindEmptyCell()            //비어 있는 칸 찾기
    {
        List<GridCell> emptyCells = new List<GridCell>();        //빈 칸들을 저장할 리스트

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y].IsEmpty())       //칸이 비어 있다면 리스트에 추가
                {
                    emptyCells.Add(grid[x, y]);
                }
            }
        }

        if (emptyCells.Count == 0)              //빈 칸이 없으면 null 값 반환
        {
            return null;
        }

        return emptyCells[Random.Range(0, emptyCells.Count)];    //랜덤하게 빈칸 하나 선택
    }

    public bool SpawnNewRank()
    {
        GridCell emptyCell = FindEmptyCell();
        if (emptyCell == null) return false;

        int rankLevel = Random.Range(0, 100) < 80 ? 1 : 2;

        CreateRankInCell(emptyCell, rankLevel);

        return true;
    }

    public GridCell FindClosestCell(Vector3 position)
    {
        for(int x = 0; x < gridWidth;x++)
        {
            for (int y = 0; y < gridHeight;y++)
            {
                if(grid[x, y].ContainsPosition(position))
                {
                    return grid[x, y];
                }
            }
            
        }

        GridCell closestCell = null;
        float closestDistance = float.MaxValue;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                float distance = Vector3.Distance(position, grid[x, y].transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCell = grid[x, y];
                }
            }

        }

        if(closestDistance > CellSize * 2) //너무 멀면 null 반환
        {
            return null;
        }

        return closestCell;

    }

    public void RemoveRank(DraggableRank rank)
    {
        if (rank == null) return;

        if (rank.currentCell == null)
        {
            rank.currentCell.currentRank = null;
        }

        Destroy(rank.gameObject);
    }

    public void MergeRanks(DraggableRank draggedRank, DraggableRank targetRank)
    {
        if (draggedRank == null || targetRank == null || draggedRank.rankLevel != targetRank.rankLevel) //같은 레벨이 아니면 합치기 실패
        {
            if (draggedRank != null) draggedRank.ReturnToOriginalPosition();
            return;
        }

        int newLevel = targetRank.rankLevel + 1;
        if (newLevel > maxRankLevel)
        {
            RemoveRank(draggedRank);
            return;
        }

        targetRank.SetRankLevel(newLevel);
        RemoveRank(draggedRank);
    }
}
