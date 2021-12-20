using System;
using UnityEngine;
namespace Scriptes.Messages
{
    
    public class Join:Message
    {
        public int _gameId;
        public string _playerName;
        public Join(string PlayerName,int gameId)
        {
            messageType = "Join";
            _gameId = gameId;
            _playerName = PlayerName;
        }
        public Join()
        {
            messageType = "Join";
            _gameId = 0;
            _playerName = null;
        }
    
        public override Message Decode(string message)
        {
            var desMes = JsonUtility.FromJson<Join>(message);
            _gameId = desMes._gameId;
            _playerName = desMes._playerName;
            return desMes;
        }

    }
}