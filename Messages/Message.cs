using System;
using UnityEngine;

namespace Scriptes.Messages
{
     public abstract class Message
     {
          public string messageType;

          public string MessageType
          {
               get { return messageType; }
               set { messageType = value; }
          }

          public abstract Message Decode(string message);
          public  string Encode()
          {
               return JsonUtility.ToJson(this);
          }
     }
}
