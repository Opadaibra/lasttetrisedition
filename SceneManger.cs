using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI ;
public partial class SceneManger : MonoBehaviour
{
    public GameObject WrongJoin
    {
        get => wrongJoin;
        set => wrongJoin = value;
    }

    private SubSceneManger s;

    public SubSceneManger S
    {
        get => s;
        set => s = value;
    }

    [Header("Game State Attributes")]
    [SerializeField] private Text CurrScore;
    [SerializeField] private Text HeightScore;
    [SerializeField] private Text BlizzerdNumber;
    [SerializeField] private Text EarthquakeNumber;
    [SerializeField] private Text diffLevel;
    [SerializeField] private Text lines;
    [Header("Next Shapes Positions")]
    [SerializeField] private List<Vector3> nextShapesFirstLevelPosition ;
    [SerializeField] private List<Vector3> nextShapesSecondLevelPosition ;
    [SerializeField] private List<Vector3> nextShapesThreidLevelPosition ;
    [Header("VFXs")]
    [SerializeField] private GameObject twoDBreakLineVFX;
    [SerializeField] private GameObject threeDBreakLineVFX;
    [Header("Windows")]
    [SerializeField] private GameObject joinErrorConnectionWindow;
    [SerializeField] private GameObject createErrorConnectionWindow;
    [SerializeField] private GameObject waitingCircle;
    [SerializeField] private GameObject invalidInputWindow;
    [SerializeField] private GameObject connectionWindow;
    [SerializeField] private GameObject successedCreatWindow;
    [SerializeField] private GameObject successedJoinWindow;
    [SerializeField] private GameObject coinsView;
    [SerializeField] private GameObject createNewPlayerScene;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject wrongJoin;
    public GameObject CreateNewPlayerScene
    {
        get => createNewPlayerScene;
        set => createNewPlayerScene = value;
    }
    public GameObject MainMenu
    {
        get => mainMenu;
        set => mainMenu = value;
    }
    public GameObject TwoDBreakLineVfx
    {
        get => twoDBreakLineVFX;
        set => twoDBreakLineVFX = value;
    }

    public GameObject ThreeDBreakLineVfx
    {
        get => threeDBreakLineVFX;
        set => threeDBreakLineVFX = value;
    }

    public GameObject JoinErrorConnectionWindow
    {
        get => joinErrorConnectionWindow;
        set => joinErrorConnectionWindow = value;
    }

    public GameObject CreateErrorConnectionWindow
    {
        get => createErrorConnectionWindow;
        set => createErrorConnectionWindow = value;
    }

    public GameObject WaitingCircle
    {
        get => waitingCircle;
        set => waitingCircle = value;
    }

    public GameObject InvalidInputWindow
    {
        get => invalidInputWindow;
        set => invalidInputWindow = value;
    }

    public GameObject ConnectionWindow
    {
        get => connectionWindow;
        set => connectionWindow = value;
    }

    public GameObject SuccessedCreatWindow
    {
        get => successedCreatWindow;
        set => successedCreatWindow = value;
    }

    public GameObject SuccessedJoinWindow
    {
        get => successedJoinWindow;
        set => successedJoinWindow = value;
    }
    private void Start()
    {
        UpdateView();
    }

    public void SetPlayersBoardVisible(int number)
    {
        
    }
    public void UpdateView()
    {
       /* if (CurrScore && HeightScore && diffLevel)
        {
            CurrScore.text = FindObjectOfType<Game>().CurrScore.ToString();
//            HeightScore.text = player.BestScore.ToString();
            diffLevel.text = (FindObjectOfType<Game>().DifficultyLevel+1).ToString();
            lines.text = (FindObjectOfType<GameBoard>()).Lines.ToString();
        }*/
    }

    public void UpdateNextList()
    {
       /* Queue<Shape> nextShapes = FindObjectOfType<GameManger>().NextShapes;
        List<Vector3> nextShapesPositionOnScreen;
        switch (FindObjectOfType<Game>().DifficultyLevel)
        {
            case 0:
                nextShapesPositionOnScreen = nextShapesFirstLevelPosition;
                break;
            case 1:
                nextShapesPositionOnScreen = nextShapesSecondLevelPosition;
                break;
            case 2:
                nextShapesPositionOnScreen = nextShapesThreidLevelPosition;
                break;
            default:
                return;
        }
        
        for (int i = 0; i < FindObjectOfType<GameManger>().NumberOfNextShapePerLevel[FindObjectOfType<Game>().DifficultyLevel]; i++)
        {
            nextShapes.ToArray()[i].transform.position = nextShapesPositionOnScreen[i];
        }*/
    }

    public void BreackLine2dVFX(float yPos)
    {
      //  GameObject vfx = Instantiate(BreakLineVFX, new Vector3(4.5f,yPos,-1)+BreakLineVFX.transform.position, Quaternion.identity)as GameObject;
       // Destroy(vfx, 1000);
    }

    public void LoadGameScene(string view)
    {
        if (view == "TwoD")
            SceneManager.LoadScene("TwoD");
        else if (view == "ThreeD")
            SceneManager.LoadScene("ThreeD");
    }
}
