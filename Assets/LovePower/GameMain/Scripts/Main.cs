using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LovePower
{
    public class Main : MonoBehaviour
    {
        private void Start()
        {
            NetManager.Instance.ConnectToServer();
            UIManager.Instance.ShowStartPanel();
        }
    }
}

