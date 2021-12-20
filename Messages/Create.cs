
using System;
using System.Text;
using UnityEngine;
namespace Scriptes.Messages
{
    public class Create:Message
    {
        public string _gameStyle;

        public string _boardView;

        public  int _maxPlayer;
    
        public  string _playerName;

        public Create(string playerName,string gameStyle, string boardView, int maxPlayer)
        {
            messageType = "Create";
            _playerName = playerName ;
            _gameStyle = gameStyle;
            _boardView = boardView;
            _maxPlayer = maxPlayer;
        }

        public Create()
        {
            _gameStyle = null;
            _maxPlayer = 0;
            _boardView = null;
            _playerName = null;
        }
        public override Message Decode(string message)
        {
            var desMes = JsonUtility.FromJson<Create>(message);
          //  var desMes = JsonConvert.DeserializeObject<Create>(message);
            messageType = "Create";
            _gameStyle = desMes._gameStyle;
            _boardView = desMes._boardView;
            _maxPlayer = desMes._maxPlayer;
            _playerName = desMes._playerName;
            return desMes;
        }

    }
}