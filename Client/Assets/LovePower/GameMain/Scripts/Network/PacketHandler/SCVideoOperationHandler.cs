using GameFramework.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LovePower
{
    public class SCVideoOperationHandler : PacketHandlerBase
    {
        public override int Id => 1004;

        public override void Handle(object sender, Packet packet)
        {
            SCVideoOperation msg = (SCVideoOperation)packet;
            if (msg.Code == 200)
            {
                if (!PlayerRuntimeData.IsOwner)
                {
                    switch (msg.OperationCode)
                    {
                        case 1:
                            {
                                //播放
                                GameEntry.Video.Play();
                                if (msg.IsForce)
                                {
                                    SetPlayProgress(msg.VideoProgressValue);
                                }
                                break;
                            }
                        case 2:
                            {
                                //暂停
                                GameEntry.Video.Pause();
                                if (msg.IsForce)
                                {
                                    SetPlayProgress(msg.VideoProgressValue);
                                }
                                break;
                            }
                        case 3:
                            {
                                //修改进度
                                SetPlayProgress(msg.VideoProgressValue);
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
        }

        private void SetPlayProgress(int serverPlayProgress)
        {
            var value = serverPlayProgress * 1f / 100f;
            GameEntry.Video.SetPlayProgress(value);
        }
    }
}

