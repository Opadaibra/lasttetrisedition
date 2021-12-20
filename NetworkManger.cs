using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Scriptes.Messages;
using UnityEngine;
using Random = System.Random;

public class NetworkManger
{
    public bool Joined
    {
        get => _joined;
        set => _joined = value;
    }

    public string ConnectionErrorCode
    {
        get => _connectionErrorCode;
        set => _connectionErrorCode = value;
    }

    public bool ConnectionError
    {
        get => _connectionError;
        set => _connectionError = value;
    }

    public StartGame StartGameMessage
    {
        get => _startGameMessage;
        set => _startGameMessage = value;
    }

    private bool _connect;
    private bool _hasStarted;
    private bool _connectionError;
    private bool _joined;
    private string _connectionErrorCode;
    private TcpClient _serverSocket;
    private string _serverIp;
    private int _port;
    private int _GameId;
    private string _playerID;

    public string PlayerId
    {
        get => _playerID;
        set => _playerID = value;
    }

    private Message _orderMessage;
    private Message _toSend;
    private StartGame _startGameMessage;
    private Dictionary<string, Message> _receivedBuffer;

    public Dictionary<string, Message> ReceivedBuffer
    {
        get => _receivedBuffer;
        set => _receivedBuffer = value;
    }

    public Message ToSend
    {
        get => _toSend;
        set => _toSend = value;
    }

    public bool HasStarted
    {
        get => _hasStarted;
        set => _hasStarted = value;
    }

    public Message OrderMessage
    {
        get { return _orderMessage; }
        set { _orderMessage = value; }
    }

    public NetworkManger(string serverIp, int port)
    {
        IPAddress ip;
        if (IPAddress.TryParse(serverIp, out ip))
        {
            _serverIp = ip.ToString();
        }
        else
        {
            Debug.Log("The Parsing Not done Well");
            return;
        }

        _port = port;
        GameId = 0;
    }

    public NetworkManger()
    {
        _serverIp = "192.168.43.51";
        _port = 5055;
        GameId = 0;
    }

    public bool IsConnect
    {
        get { return _connect; }
        set { _connect = value; }
    }

    public int GameId
    {
        get { return _GameId; }
        set { _GameId = value; }
    }

    public void StartConnect()
    {
        _serverSocket = new TcpClient();
        _serverSocket.Connect(_serverIp, _port);
        _connect = true;
        Debug.Log("Done");
    }

    public void Connect()
    {
        if (_connect)
        {
            try
            {
                if (_orderMessage.MessageType == "Create")
                {
                    SendMessage(_orderMessage);
                    if (ReciveMessage() == null && GameId != 0)
                    {
                        Debug.Log("Room ID: " + GameId); //to change in Interface
                        Task.Factory.StartNew(() =>
                        {
                            Debug.Log("Send");
                            Message mes;
                            do
                            {
                                mes = ReciveMessage();
                                Thread.Sleep(100);
                            } while (!mes.MessageType.Equals("StartGame"));

                            _startGameMessage = (StartGame) mes;
                            _hasStarted = true;
                          //  BuildReceivedBuffer();
                            while (_serverSocket.Connected)
                            {
                                if (ToSend != null)
                                {
                                    SendMessage(ToSend);
                                    Debug.Log("i've send");
                                    ToSend = null;
                                }

                                Thread.Sleep(100);
                            }
                        });
                        Task.Factory.StartNew(() =>
                        {
                            Thread.Sleep(100);
                            while (_serverSocket.Connected)
                            {
                                ReciveMessage();
//                                Debug.Log(ReciveMessage().Encode());
                            }
                        });
                    }
                }
                else if (_orderMessage.MessageType == "Join")
                {
                    SendMessage(_orderMessage);
                    Task.Factory.StartNew(() =>
                    {
                        Message mes;
                        do
                        {
                            mes = ReciveMessage();
                            Thread.Sleep(100);
                        } while (!mes.MessageType.Equals("StartGame"));

                        _startGameMessage = (StartGame) mes;
                        _hasStarted = true;
                     //   BuildReceivedBuffer();
                        while (_serverSocket.Connected)
                        {
                            if (ToSend != null)
                            {
                                SendMessage(ToSend);
                                Debug.Log("i've send");
                                ToSend = null;
                            }

                            Thread.Sleep(100);
                        }
                    });
                    Task.Factory.StartNew(() =>
                    {
                        
                        Thread.Sleep(100);
                        while (_serverSocket.Connected)
                        {
                            ReciveMessage();
                            //  Debug.Log(ReciveMessage().Encode());
                        }
                    });
                }
            }
            catch (Exception exception)
            {
                _connectionError = true;
                _connectionErrorCode = "Connection Error";
            }
        }
        else
        {
            throw new Exception();
        }
    }

    public void SendMessage(Message message)
    {
        try
        {
            string s = message.Encode();
            _serverSocket.GetStream().Write(Encoding.ASCII.GetBytes(s), 0, Encoding.ASCII.GetBytes(s).Length);
        }
        catch (Exception e)
        {
            _connectionError = true;
            _connectionErrorCode = "Server is out of order";
        }
    }

    public Message ReciveMessage()
    {
        byte[] data = new byte[1024];
        int dataLength;
        try
        {
            dataLength = _serverSocket.GetStream().Read(data, 0, data.Length);
        }
        catch (Exception e)
        {
            _connectionError = true;
            _connectionErrorCode = "Server is not available";
            return null;
        }

        var receivedMes = Encoding.ASCII.GetString(data, 0, dataLength);
        try
        {
            //   Debug.Log(receivedMes);
            if (receivedMes.Contains("Room"))
            {
                try
                {
                    Debug.Log(receivedMes);
                    GameId = Int32.Parse(receivedMes.Split(Convert.ToChar(" "))[2]);
                    _joined = true;
                }
                catch (Exception e)
                {
                    if (receivedMes.Contains("allowed"))
                    {
                        _connectionError = true;
                        _connectionErrorCode = "not allowed join to room";
                    }
                    else if (receivedMes.Contains("Room"))
                    {
                        _connectionError = true;
                        _connectionErrorCode = "Wrong room id";
                    }

                    Debug.Log(receivedMes);
                }

                return null;
            }

            //use any message type only for casting the message type ,no bro you can't'
            if (receivedMes.Contains("StartGame"))
            {
                //Debug.Log("I catch start game");
                Debug.Log(receivedMes);
                return JsonUtility.FromJson<StartGame>(receivedMes);
            }

            if (receivedMes.Contains("Info"))
            {
                Debug.Log(receivedMes);
                Info currReceivedMessage = JsonUtility.FromJson<Info>(receivedMes);
                
                _receivedBuffer[currReceivedMessage._playerId] = currReceivedMessage;
                Debug.Log("i've recevied "  +  currReceivedMessage._playerId);
                foreach(var id in _receivedBuffer.Keys)
                {
                  //  Debug.Log("Player Id : " + id);
                   // Debug.Log("Message in  : " + _receivedBuffer[id].Encode());
                }
                return currReceivedMessage;
            }
        }
        catch (Exception e)
        {
            _connectionError = true;
        }

        return null;
    }

    public void BuildReceivedBuffer()
    {
        _receivedBuffer = new Dictionary<string, Message>();
        for (int i = 0; i < _startGameMessage._maxPlayer-1; i++)
        {
            
            Debug.Log("curr id" + _startGameMessage._playersId[i] + "your id" + _playerID);
            if(_startGameMessage._playersId[i]==_playerID) continue;
            Debug.Log("i've add " + _startGameMessage._playersId[i]);
            _receivedBuffer.Add(_startGameMessage._playersId[i], null);
        }
        Debug.Log("start test");
        foreach (var id in _receivedBuffer.Keys)
        {
            Debug.Log(id);
        }
        Debug.Log("end test");

    }

    bool isEqual(byte[] X, byte[] Y)
    {
        return false;
       /* for(int i=0;i<X.Length;i++)
              if()*/
    }

    public void DisConnect()
    {
        _serverSocket.Client.Close();
    }
}