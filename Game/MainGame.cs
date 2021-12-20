using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Scriptes.Messages;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
    //config parms

    [SerializeField] private Game _singleGame;
    [SerializeField] private Game _multiGame;
    [SerializeField] private GameStore gameStore;
    [SerializeField] private SceneManger sceneManager;
    private string _playerInfoPath;
    private string _gameSettingInfoPath;
    private string _gameInfoPath;

    //actual parms
    private Game _game;
    private Player _player;
    [SerializeField] private string _gameView;
    [SerializeField] private string _gameStyle;
    private GameSettings _gameSetting;
    private GameStore _gameStore;
    private SceneManger _sceneManager;
    private NetworkManger _networkManger;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        _playerInfoPath = Application.persistentDataPath + Path.DirectorySeparatorChar + "player.tetris";
        _gameSettingInfoPath = Application.persistentDataPath + Path.DirectorySeparatorChar + "gameSettings.tetris";
        _gameInfoPath = Application.persistentDataPath + Path.DirectorySeparatorChar + "game.tetris";
        _gameSetting = FindObjectOfType<GameSettings>();
        _sceneManager = FindObjectOfType<SceneManger>();
        _gameView = "";
        _gameStyle = "";
        try
        {
            CheckExistencePlayer();
        }
        catch (PlayerNotFound playerNotFound)
        {
            _sceneManager.MainMenu.SetActive(false);
            _sceneManager.CreateNewPlayerScene.SetActive(true);
        }

        try
        {
            CheckExistenceGameSetting();
        }
        catch (GameSettingNotFound gameSettingNotFound)
        {
            _gameSetting.Default();
            WriteGameSetting();
        }
    }

    public void CreatSingleGame()
    {
        try
        {
            CheckCreationState();
        }
        catch (IllegalGameCreation illegalGameCreation)
        {
            _gameStyle = "Survival";
            _gameView = "TwoD";
        }

        _sceneManager.LoadGameScene(_gameView);
        SceneManager.sceneLoaded += SingleGameAttributes;
    }

    public void SingleGameAttributes(Scene s, LoadSceneMode r)
    {
        _game = Instantiate(_singleGame);
        _game.Player = _player;
        _game.GameSetting = _gameSetting;
        _game.Style = _gameStyle;
        _game.View = _gameView;
        _game.SceneManger = _sceneManager;
        _game.SceneManger.S = FindObjectOfType<SubSceneManger>();
        FindObjectOfType<SubSceneManger>().RepairTwoScene(_game.Style);
        Debug.Log("hi i'm done ");
    }

    public void CreatMultiGame(Text maxPlayerTextField)
    {
        _sceneManager.ConnectionWindow.SetActive(true);
        _sceneManager.WaitingCircle.SetActive(true);
        _sceneManager.CreateErrorConnectionWindow.SetActive(false);
        _sceneManager.InvalidInputWindow.SetActive(false);
        _networkManger = new NetworkManger();
        try
        {
            CheckCreationState();
        }
        catch (IllegalGameCreation illegalGameCreation)
        {
            _gameStyle = "Survival";
            _gameView = "TwoD";
        }

        if (maxPlayerTextField.text == "")
        {
            _sceneManager.InvalidInputWindow.SetActive(true);
            return;
        }

        var maxPlayer = int.Parse(maxPlayerTextField.text);
        //for mobile 
        if (maxPlayer > 3) maxPlayer = 3;
        //   if (maxPlayer == 1) maxPlayer = 2;

        _networkManger.OrderMessage = new Create(
            _player.GetNameWithSubId(),
            _gameStyle, _gameView, maxPlayer);
        Debug.Log(_player.GetNameWithSubId().Length);
        Debug.Log(_player.GetNameWithSubId());
        Debug.Log("in network create" + (JsonUtility.ToJson((Create) _networkManger.OrderMessage)));
        var connectionTask = Task.Factory.StartNew(() => { _networkManger.StartConnect(); });
        StartCoroutine(StartCommunication(connectionTask, _networkManger, true));
    }

    public void JoinMultiGame(Text GameIdTextField)
    {
        _sceneManager.ConnectionWindow.SetActive(true);
        _sceneManager.WaitingCircle.SetActive(true);
        _sceneManager.CreateErrorConnectionWindow.SetActive(false);
        _sceneManager.InvalidInputWindow.SetActive(false);
        _networkManger = new NetworkManger();
        int GameId = 0;
        try
        {
            GameId = int.Parse(GameIdTextField.text);
        }
        catch (Exception e)
        {
            _sceneManager.InvalidInputWindow.SetActive(true);
            return;
        }

        _networkManger.OrderMessage = new Join(_player.GetNameWithSubId(), GameId);
        var connectionTask = Task.Factory.StartNew(() => { _networkManger.StartConnect(); });
        StartCoroutine(StartCommunication(connectionTask, _networkManger, false));
    }

    public void MultiGameAttributes(Scene s, LoadSceneMode r)
    {
        _game = Instantiate(_multiGame);
        ((MultiGame) _game).NetworkManger = _networkManger;
        _game.Player = _player;
        _game.GameSetting = _gameSetting;
        _game.Style = _gameStyle;
        _game.View = _gameView;
        _game.SceneManger = _sceneManager;
    }


    private IEnumerator StartCommunication(Task connectionTask, NetworkManger networkManger, bool creating)
    {
        while (!connectionTask.IsCompleted)
        {
            yield return new WaitForSeconds(0.1f);
        }

        if (connectionTask.Status == TaskStatus.Faulted)
        {
            _sceneManager.CreateErrorConnectionWindow.SetActive(true);
            _sceneManager.WaitingCircle.SetActive(false);
        }
        else if (connectionTask.Status == TaskStatus.RanToCompletion)
        {
            var communicationTask = Task.Factory.StartNew(networkManger.Connect);
            if (creating)
            {
                while (networkManger.GameId == 0)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                _sceneManager.SuccessedCreatWindow.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                    "Game id : " + networkManger.GameId;
                _sceneManager.SuccessedCreatWindow.SetActive(true);
                while (!networkManger.HasStarted)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                networkManger.PlayerId = _player.GetNameWithSubId();
                networkManger.BuildReceivedBuffer();
                _sceneManager.ConnectionWindow.SetActive(false);
                _gameView = _networkManger.StartGameMessage._boardView;
                _gameStyle = _networkManger.StartGameMessage._gameStyle;
                Debug.Log("Game View : " + _gameView);
                Debug.Log("Game Style : " + _gameStyle);
                _sceneManager.LoadGameScene(_gameView);
                SceneManager.sceneLoaded += MultiGameAttributes;
            }
            else
            {
                //  while (!networkManger.Joined)
                //{
                /* if (networkManger.ConnectionError)
                 {
                     _sceneManager.WrongJoin.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text =
                         networkManger.ConnectionErrorCode;
                     _sceneManager.WrongJoin.SetActive(true);
                     break;
                 }*/
                // Debug.Log(networkManger.Joined);
                //yield return new WaitForSeconds(0.1f);
                // }

                //  if (networkManger.Joined)
                // {
                _sceneManager.SuccessedJoinWindow.SetActive(true);
                while (!networkManger.HasStarted)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                Debug.Log("join and start");
                networkManger.PlayerId = _player.GetNameWithSubId();
                networkManger.BuildReceivedBuffer();
                _sceneManager.ConnectionWindow.SetActive(false);
                _gameView = _networkManger.StartGameMessage._boardView;
                _gameStyle = _networkManger.StartGameMessage._gameStyle;
                Debug.Log("Game View : " + _gameView);
                Debug.Log("Game Style : " + _gameStyle);
                _sceneManager.LoadGameScene(_gameView);
                SceneManager.sceneLoaded += MultiGameAttributes;
            }

            // }
        }
    }

    public void SetGameView(string view)
    {
        _gameView = view;
    }


    public void SetGameStyle(string style)
    {
        _gameStyle = style;
    }

    public void CheckCreationState()
    {
        if (_gameStyle == "" || _gameView == "")
            throw new IllegalGameCreation();
    }

    public void CheckExistenceGameSetting()
    {
        if (File.Exists(_gameSettingInfoPath))
        {
            ReadGameSettings();
        }
        else
        {
            throw new GameSettingNotFound();
        }
    }

    public void CheckExistencePlayer()
    {
        if (File.Exists(_playerInfoPath))
        {
            ReadPlayerInfo();
        }
        else throw new PlayerNotFound();
    }

    public void ReadPlayerInfo()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Open(_playerInfoPath, FileMode.Open);
        _player = (Player) bf.Deserialize(fs);
        fs.Close();
    }

    public void WritePlayerInfo()
    {
        TextMeshProUGUI playerName = null;
        TextMeshProUGUI[] texts = FindObjectsOfType<TextMeshProUGUI>();
        foreach (TextMeshProUGUI t in texts)
        {
            if (t.name == "PlayerName")
            {
                playerName = t;
                Debug.Log(t.text);
                break;
            }
        }

        if (playerName)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(_playerInfoPath, FileMode.Create);
            Player p = new Player(playerName.text);
            bf.Serialize(fs, p);
            fs.Close();
            //test
            _sceneManager.MainMenu.SetActive(true);
            _sceneManager.CreateNewPlayerScene.SetActive(false);
        }
    }

    public void WriteGameSetting()
    {
        _gameSetting.Editing = true;
        var settingAsJson = _gameSetting.GetSettingsAsJson();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Open(_gameSettingInfoPath, FileMode.Create);
        bf.Serialize(fs, settingAsJson);
        Debug.Log(settingAsJson);
        fs.Close();
        _gameSetting.Editing = false;
    }

    public void ReadGameSettings()
    {
        _gameSetting.Editing = true;
        var settingAsJson = "";
        var bf = new BinaryFormatter();
        var fs = File.Open(_gameSettingInfoPath, FileMode.Open);
        settingAsJson = (string) bf.Deserialize(fs);
        _gameSetting = FindObjectOfType<GameSettings>();
        _gameSetting.SetSettingsAsJson(settingAsJson);
        fs.Close();
        _gameSetting.Editing = false;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public void ResetCreationSetting()
    {
        _gameView = "";
        _gameStyle = "";
        _networkManger?.DisConnect();
    }
//test for save and load 

    public void LoadGame()
    {
        _sceneManager.LoadGameScene("TwoD");
        SceneManager.sceneLoaded += LoadSingleGameAttributes;
    }

    public void LoadSingleGameAttributes(Scene s, LoadSceneMode r)
    {
        _game = Instantiate(_singleGame);
        ((SingleGame) _game).LoadedState = ReadGame();
        _game.Player = _player;
        _game.GameSetting = _gameSetting;
        //_gameStyle = _game.Style;
        //_gameView = _game.View;
        //only test
        _gameStyle = "TwoD";
        _gameView = "Survival";
        _game.SceneManger = _sceneManager;
    }

    public void WriteGame(Game game)
    {
        _gameSetting.Editing = true;
        var bf = new BinaryFormatter();
        var fs = File.Open(_gameInfoPath, FileMode.Create);
        bf.Serialize(fs, ((SingleGame) _game).GetGameStateAsJson());
        fs.Close();
        _gameSetting.Editing = false;
    }

    public string ReadGame()
    {
        string gameStateAsJson;
        var bf = new BinaryFormatter();
        var fs = File.Open(_gameInfoPath, FileMode.Open);
        gameStateAsJson = (string) bf.Deserialize(fs);
        fs.Close();
        Debug.Log(gameStateAsJson);
        return gameStateAsJson;
    }

    public void SaveGame()
    {
        WriteGame(_game);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}