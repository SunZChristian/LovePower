//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using ProtoBuf;

public abstract class PacketBase 
{

    public PacketBase()
    {
       
    }

    public abstract int Id
    {
        get;
    }

    public abstract PacketType PacketType
    {
        get;
    }
}
