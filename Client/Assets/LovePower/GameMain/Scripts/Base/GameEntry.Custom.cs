using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LovePower
{
    public partial class GameEntry : MonoBehaviour
    {
        public static TcpClientComponent TcpClient
        {
            get;
            private set;
        }

        private static void InitCustomComponents()
        {
            TcpClient = UnityGameFramework.Runtime.GameEntry.GetComponent<TcpClientComponent>();
        }
    }
}

