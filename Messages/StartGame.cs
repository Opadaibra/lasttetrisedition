using System;
using UnityEngine;

namespace Scriptes.Messages
{
    public class StartGame : Message
    {
        public bool _start;
        
        public string _gameStyle;

        public string _boardView;

        public  int _maxPlayer;

        public string[] _playersId;

        public StartGame(bool start)
        {
            this.messageType = "StartGame";
            _start = start;
        }

        public StartGame()
        {
            this.messageType = "StartGame";
        }

        public override Message Decode(string message)
        {
            var jsonAsMessage = JsonUtility.FromJson<StartGame>(message);
            if (jsonAsMessage.messageType == "StartGame")
                _start = true;
            else
                _start = false;
            return jsonAsMessage;
        }
    }
}