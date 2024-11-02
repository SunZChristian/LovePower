//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using ProtoBuf;

namespace Protocol.Base
{
    public abstract class PacketBase : IExtensible
    {
        private IExtension m_ExtensionObject;

        public abstract int Id { get; }

        public PacketBase()
        {
            m_ExtensionObject = null;
        }

        public abstract PacketType PacketType
        {
            get;
        }

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref m_ExtensionObject, createIfMissing);
        }
    }
}
