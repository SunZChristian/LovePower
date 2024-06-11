using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LovePower
{
    public class UIManager : MonoBehaviour
    {
        public VideoHallPanel videoHallPanel;
        public CreateOrJoinRoomPanel startPanel;

        public GameObject UICanvas;

        public static UIManager Instance;
        private void Awake()
        {
            Instance = this;
        }

        public VideoHallPanel ShowVideoHallPanel()
        {
            var ui = Instantiate<VideoHallPanel>(videoHallPanel, UICanvas.transform);
            return ui;
        }

        public CreateOrJoinRoomPanel ShowStartPanel()
        {
            var ui = Instantiate<CreateOrJoinRoomPanel>(startPanel, UICanvas.transform);
            return ui;
        }
    }
}

