using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(MazeConstructor))]

public class GameController : MonoBehaviour
{
    [SerializeField] private AICharacterControl AITester;
    [SerializeField] private GameObject Goal;
    [SerializeField] private Camera camera;

    private MazeConstructor generator;

    void Start()
    {
        generator = GetComponent<MazeConstructor>();
        //StartNewMaze();
    }

    string content1 = "x";
    string content2 = "y";

    void OnGUI()
    {
        
        content1 = GUI.TextField(new Rect(0, 0, 40, 40), content1,25);
        content2 = GUI.TextField(new Rect(0, 40, 40, 40), content2,25);

        if (GUI.Button(new Rect(40, 0, 250, 80), "Сгенерировать лабиринт."))
        {
            if (content1 != string.Empty && content2 != string.Empty)
                StartNewMaze(Convert.ToInt32(content1), Convert.ToInt32(content2));

        }
    }


    public float  x;
        public float y;
        public float z;

    private void StartNewMaze(int xdim, int ydim)
    {
        AITester.gameObject.SetActive(false);
        AITester.enabled = false;
        generator.GenerateNewMaze(xdim, ydim);

         x = generator.startCol * generator.hallWidth;
         y = 1;
         z = generator.startRow * generator.hallWidth;

        camera.GetComponent<CameraController>().startPoint = new Vector3(x, y + AITester.transform.localScale.y, z);

        float x1 = generator.goalCol * generator.hallWidth;
        float y1 = 1;
        float z1 = generator.goalRow * generator.hallWidth;
        AITester.transform.position = new Vector3(x, y, z);
        Goal.transform.position = new Vector3(x1, y1, z1);
        AITester.gameObject.SetActive(true);
        AITester.enabled = true;

        generator.BackeMesh();
    }
}