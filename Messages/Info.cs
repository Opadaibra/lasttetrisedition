using System;
using UnityEngine;

namespace Scriptes.Messages
{
    public class Info :Message
    {
        public string _playerId ;

        public bool  _playerStatue ;

        public int _currScore;

        public string _boardStateCode;
        
        public string _boardElementsColorCode;

        public string _boardStyleCode;
        
        public Info()
        {
            messageType = "Info";
        }
    
        public override Message Decode(string message)
        {
            var desMes = JsonUtility.FromJson<Info>(message);
            return desMes;
        }

    }
}

