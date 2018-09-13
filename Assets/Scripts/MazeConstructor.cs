using UnityEngine;
using UnityEngine.AI;

public class MazeConstructor : MonoBehaviour
{
    [SerializeField] private Material floorMaterial;
    [SerializeField] private Material wallMaterial;
    private MazeDataGenerator dataGenerator;
    private MazeMeshGenerator meshGenerator;

    private GameObject rootGo;
    private NavMeshSurface nmC;

    private float _hallWidth = 3f;
    private float _hallHeight = 2f;

    public float hallWidth
    {
        get { return _hallWidth; }
    }
    public float hallHeight
    {
        get { return _hallHeight; }
    }

    public int startRow
    {
        get; private set;
    }
    public int startCol
    {
        get; private set;
    }

    public int goalRow
    {
        get; private set;
    }
    public int goalCol
    {
        get; private set;
    }

    public int[,] data
    {
        get; private set;
    }

    void Awake()
    {
        data = new int[,]
        {
            {1, 1, 1},
            {1, 0, 1},
            {1, 1, 1}
        };

        dataGenerator = new MazeDataGenerator();
        meshGenerator = new MazeMeshGenerator();
    }


    public void GenerateNewMaze(int sizeRows, int sizeCols)
    {
        if (sizeRows % 2 == 0 && sizeCols % 2 == 0)
        {
            Debug.LogError("Используйте нечетные числа для задания размеров лабиринта!");
        }

        DisposeOldMaze();

        data = dataGenerator.FromDimensions(sizeRows, sizeCols);

        FindStartPosition();
        FindGoalPosition();

        DisplayMaze();
    }

    private void DisplayMaze()
    {
        rootGo = new GameObject();
        rootGo.transform.position = Vector3.zero;
        rootGo.name = "Procedural Maze Root";
        rootGo.tag = "Generated";

        GameObject floorGO = new GameObject();
        floorGO.transform.position = Vector3.zero;
        floorGO.name = "Floor";
        floorGO.tag = "Generated";

        floorGO.transform.SetParent(rootGo.transform);

        MeshFilter mfF = floorGO.AddComponent<MeshFilter>();
        mfF.mesh = meshGenerator.FloorFromData(data, hallWidth, hallHeight);

        MeshCollider mcF = floorGO.AddComponent<MeshCollider>();
        mcF.sharedMesh = mfF.mesh;

        MeshRenderer mrF = floorGO.AddComponent<MeshRenderer>();
        mrF.materials = new Material[1] { floorMaterial };

        GameObject wallsGO = new GameObject();
        wallsGO.transform.position = Vector3.zero;
        wallsGO.name = "Walls";
        wallsGO.tag = "Generated";

        wallsGO.transform.SetParent(rootGo.transform);

        MeshFilter mfW = wallsGO.AddComponent<MeshFilter>();
        mfW.mesh = meshGenerator.WallsFromData(data, hallWidth, hallHeight);

        MeshCollider mcW = wallsGO.AddComponent<MeshCollider>();
        mcW.sharedMesh = mfW.mesh;

        MeshRenderer mrW = wallsGO.AddComponent<MeshRenderer>();
        mrW.materials = new Material[1] { wallMaterial };


        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        float x = hallWidth * data.GetLength(0) / 2;
        float y = hallHeight * data.GetLength(1) / 2;
        cube.transform.position = new Vector3(-1, -1, -1);
        cube.transform.SetParent(rootGo.transform);
        cube.AddComponent<BoxCollider>();

        
        //nmC.BuildNavMesh();

    }

    public void BackeMesh()
    {

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        float x = hallWidth * data.GetLength(0) / 2;
        float y = hallHeight * data.GetLength(1) / 2;
        cube.transform.position = new Vector3(-1, -1, -1);
        cube.transform.SetParent(rootGo.transform);
        cube.AddComponent<BoxCollider>();
        NavMeshSurface nmC = cube.AddComponent<NavMeshSurface>();
        nmC.BuildNavMesh();
        //NavMeshData nmd = new NavMeshData();
        //nmC.UpdateNavMesh(nmd);
        //nmC.nav
    }

    public void DisposeOldMaze()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Generated");
        foreach (GameObject go in objects)
        {
            Destroy(go);
        }
    }

    private void FindStartPosition()
    {
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        for (int i = 0; i <= rMax; i++)
        {
            for (int j = 0; j <= cMax; j++)
            {
                if (maze[i, j] == 0)
                {
                    startRow = i;
                    startCol = j;
                    return;
                }
            }
        }
    }

    private void FindGoalPosition()
    {
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        for (int i = rMax; i>0; i--)
        {
            for (int j = cMax; j>0 ; j--)
            {
                if (maze[i, j] == 0)
                {
                    goalRow = i;
                    goalCol = j;
                    return;
                }
            }
        }
    }
}