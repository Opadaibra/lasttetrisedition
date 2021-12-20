using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class Laser : GameBoardStyle
{
    private GameObject _laser;
    private bool _canBringDown;

    private void Start()
    {
        var position = gameBoard.transform.position;
        Debug.Log(position);
        Debug.Log(gameBoard.GetElementSize());
        _canBringDown = true;
        _laser = Instantiate(_laser,
            position + new Vector3(0, (gameBoard.GetHeight() - 1) * gameBoard.GetElementSize(), 0)
            , Quaternion.identity);
        StartCoroutine(BringLaserDownAndUp());
        _laser.transform.localScale = new Vector3(gameBoard.GetElementSize(),gameBoard.GetElementSize(),gameBoard.GetElementSize());
        //_laser.transform.localScale = new Vector3(gameBoard.GetElementSize(),0.3f*gameBoard.GetElementSize(),gameBoard.GetElementSize());

    }

    private float _laserFallSpeed = 0.001f;

    public GameObject GetLaser()
    {
        return _laser;
    }

    public void SetLaser(GameObject laser)
    {
        this._laser = laser;
    }

    public void UpgradeLaserFallSpeedLevel()
    {
        _laserFallSpeed *= 2;
    }

    public override string ToString()
    {
        return "Laser";
    }

    public override string Encode()
    {
        var position = _laser.transform.position;
        Debug.Log("json object : "  + JsonUtility.ToJson(transform.position));
        return
            JsonUtility.ToJson(position);

    }

    public override void Decode(string code)
    {
        Debug.Log("I've Decode : " + code);
        Vector3 laserPos = JsonUtility.FromJson<Vector3>(code);
        Debug.Log("pos After Decode : "  + laserPos);
        _laser.transform.position = laserPos;
        Debug.Log("Laser position : " + _laser.transform.position);
        Time.timeScale =  0;
    }

    public override bool isEnd()
    {
        var laserYPos = Mathf.RoundToInt(_laser.transform.position.y);
        var isThereAnyElement = gameBoard.CheckAny(laserYPos, false);
        if (isThereAnyElement) _canBringDown = false;
        return isThereAnyElement;
    }

    public new void Effect()
    {
        StartCoroutine(BringLaserDownAndUp());
    }

    private IEnumerator BringLaserDownAndUp()
    {
        var lastLines = 0;
        while (_canBringDown && _laser.transform.position.y > 0)
        {
            if (gameBoard.Lines - lastLines > gameBoard.transform.position.y)
            {
                lastLines = gameBoard.Lines;
                _laser.transform.Translate(0, 0.5f * gameBoard.Lines - lastLines, 0);
                continue;
            }

            _laser.transform.Translate(0, -0.025f, 0);
            yield return new WaitForSeconds(0.1f);
        }
    }
}