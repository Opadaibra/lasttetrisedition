using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GameSettings : MonoBehaviour
{
    //Attributes
    [SerializeField] private List<int> themes;
    [SerializeField] private List<Material> darkMaterials;
    [SerializeField] private List<Material> lightMaterials;
    [SerializeField] private Button[] arrows;
    [SerializeField] private string controlAsString;
    [SerializeField] private int background;
    [SerializeField] private int theme;
    [SerializeField] private bool mute;
    [SerializeField] private int soundLevel;
    [SerializeField] private Slider SoundSlider;
    private List<List<Material>> _backgrounds;
    private Control _control;
    private string _gameSettingInfoPath ;
    private IPAddress _serverIp;
    public bool Editing;
    

    
    //Properties
    public Button[] Arrows 
    {
          get => arrows;
          set => arrows = value;
    }

    public Control Control
    {
        get => _control;
        set => _control = value;
    }

    public int Background
    {
        get => background;
        set => background = value;
    }

    public int Theme
    {
        get => theme;
        set => theme = value;
    }
    

    //Methods
    private void Awake()
    {
        DontDestroyOnLoad(this);
        _gameSettingInfoPath = Application.persistentDataPath + Path.DirectorySeparatorChar + "gameSettings.tetris";
        
        _backgrounds = new List<List<Material>>();
        
        //fill backgrounds map
        _backgrounds.Add(darkMaterials);
        _backgrounds.Add(lightMaterials);
    }
    public void SetSound(bool state)
    {
        mute = state;
        SaveCurrentSetting();
    }
    public void SetTheme()
    {
        if (theme == 0) theme = 1;
        else theme = 0;
        ThemeManger.Indexformood = theme;
        ThemeManger[] themableComponents = FindObjectsOfType<ThemeManger>();
        foreach (var component in themableComponents)
        {
            component.SwapBackground();
        }
        SaveCurrentSetting();
    }
    public void SetBackground(int index)
    {
        background = index ;
        SaveCurrentSetting() ;
    }

    public Material GetBackground()
    {
        return _backgrounds[theme][background];
    }
    public void SetSoundLevel()
    {
        soundLevel = Mathf.RoundToInt(SoundSlider.value);
        SaveCurrentSetting();
    }
    public void SetControl(string control)
    {
        switch (control)
        {
            case "Keyboard":
            {
                _control = new Keyboard();
                controlAsString = "Keyboard";
                break;
            }
            case "Touch":
            {
                _control = new TOUCH();
                controlAsString = "Touch";
                break;
            }
            case "Arrows":
            {
                _control = new Arrow(arrows);
                controlAsString = "Arrows";
                break;
            }
            default:
            {
                _control = new Arrow(arrows);
                controlAsString = "Arrows";
                break;
            }
        }
        SaveCurrentSetting();
    }
    public void Default()
    {
        theme = 0;
        mute = false; 
        background = 0;
        soundLevel = 100;
        _serverIp = new IPAddress(new byte[]{192,168,43,51});
        SetControl("Arrows");
    }

    public void ChangeServerIP(TextMeshProUGUI ip)
    {
        IPAddress newIP;
        try
        {
            var ipAsBytes = ip.text.Split('.');
            Debug.Log("prace:" + byte.Parse(ipAsBytes[0]));
            Debug.Log("prace:" + byte.Parse(ipAsBytes[1]));
            Debug.Log("prace:" + byte.Parse(ipAsBytes[2]));
           // Debug.Log("prace:" + byte.Parse(ipAsBytes[3]));
            foreach (var VARIABLE in ipAsBytes)
            {
                Debug.Log(byte.Parse(VARIABLE));
            }
            
         //   Debug.Log($"{byte.Parse(int.Parse(ipAsBytes[0]))},{ipAsBytes[1]},{ipAsBytes[2]},{ipAsBytes[3]}")};
            /*Debug.Log(Encoding.ASCII.GetBytes(ip.text.ToString()).Length);
            newIP = new IPAddress(new byte[]
            {
                byte.Parse(ipAsBytes[0]),
                byte.Parse(ipAsBytes[1]),
                byte.Parse(ipAsBytes[2]),
                byte.Parse(ipAsBytes[3])
            });*/

           // IPAddress.TryParse(ip.text, out newIP);
        }
        catch (Exception e)
        {
         //   Debug.Log(e);
            return;
        }

      //  _serverIp = newIP;
       // Debug.Log("New Ip :  " + newIP.ToString());
      //  Debug.Log(_serverIp.ToString());
    }
    public string GetSettingsAsJson()
    {
        SettingsCheckPoint currPoint = new SettingsCheckPoint();
        currPoint.controlAsString = controlAsString;
        currPoint.background = background;
        currPoint.mute = mute;
        currPoint.theme = theme;
        currPoint.soundLevel = soundLevel;
        return JsonUtility.ToJson(currPoint);
    }
    public void SetSettingsAsJson(string setting)
    {
        var temp = JsonUtility.FromJson<SettingsCheckPoint>(setting);
        background = temp.background;
        mute = temp.mute ;
        theme = temp.theme;
        controlAsString = temp.controlAsString;
        soundLevel = temp.soundLevel;
        SoundSlider.value = soundLevel ;
        SetControl(temp.controlAsString); 
    }

    public void SetMute()
    {
        mute = !mute;
        if(mute)
            SoundSlider.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Image>().color = Color.gray;
        else 
            SoundSlider.transform.GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.Image>().color 
                = new Color32(228,60,66,255);
    }
    public void SaveCurrentSetting()
    {
        if (!Editing)
        {
            Editing = true;
            if (File.Exists(_gameSettingInfoPath))
            {
                File.Delete(_gameSettingInfoPath);
                var curr = GetSettingsAsJson();
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = File.Open(_gameSettingInfoPath, FileMode.Create);
                bf.Serialize(fs, curr);
                fs.Close();
            }
            Editing = false;
        }

    }

    public int GetSoundLevel()
    {
        return mute ? 0 : soundLevel;
    }
    private struct SettingsCheckPoint
    {
        public string controlAsString;
        public int background;
        public int theme;
        public bool mute;
        public int soundLevel;
    }
}