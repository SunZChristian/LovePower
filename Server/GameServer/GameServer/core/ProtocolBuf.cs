using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

public class ProtocolBuf : ProtocolBase
{
    private byte[] waitToSendBytes;

    private static Dictionary<int, Type> m_ServerToClientPacketTypes = new Dictionary<int, Type>();
    public void Initialize()
    {
        Type packetBaseType = typeof(CSPacketBase);
        Type packetBaseType2 = typeof(SCPacketBase);
        //Type packetHandlerBaseType = typeof(PacketHandlerBase);
        Assembly assembly = Assembly.GetExecutingAssembly();
        Type[] types = assembly.GetTypes();
        for (int i = 0; i < types.Length; i++)
        {
            if (!types[i].IsClass || types[i].IsAbstract)
            {
                continue;
            }

            if (types[i].BaseType == packetBaseType|| types[i].BaseType == packetBaseType2)
            {
                PacketBase packetBase = (PacketBase)Activator.CreateInstance(types[i]);
                Type packetType = GetServerToClientPacketType(packetBase.Id);
                if (packetType != null)
                {
                    Console.WriteLine("Already exist packet type '{0}', check '{1}' or '{2}'?.", packetBase.Id.ToString(), packetType.Name, packetBase.GetType().Name);
                    continue;
                }

                m_ServerToClientPacketTypes.Add(packetBase.Id, types[i]);
            }
        }
    }

    /// <summary>
    /// 反序列化包头
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public CSPacketHeader DeserializePacketHeader(Stream source)
    {
        return Serializer.DeserializeWithLengthPrefix<CSPacketHeader>(source, PrefixStyle.Fixed32);    
    }

    /// <summary>
    /// 反序列化包体
    /// </summary>
    /// <param name="csPacketHeader"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public CSPacketBase DeserializePacket(CSPacketHeader csPacketHeader, Stream source)
    {
        if (csPacketHeader == null)
        {
            Console.WriteLine("Packet header is invalid.");
            return null;
        }

        CSPacketBase packet = null;
        if (csPacketHeader.IsValid)
        {
            Type packetType = GetServerToClientPacketType(csPacketHeader.Id);
            if (packetType != null)
            {
                var typeInfo = packetType.GetTypeInfo();
                var constructor = typeInfo.GetConstructor(Type.EmptyTypes);

                packet = (CSPacketBase)RuntimeTypeModel.Default.DeserializeWithLengthPrefix(source, constructor.Invoke(null), packetType, PrefixStyle.Fixed32, 0);
            }
            else
            {
                Console.WriteLine("Can not deserialize packet for packet id '{0}'.", csPacketHeader.Id.ToString());
            }
        }
        else
        {
            Console.WriteLine("Packet header is invalid.");
        }
        return packet;
    }

    /// <summary>
    /// 反序列化包头
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public SCPacketHeader DeserializePacketHeader2(Stream source)
    {
        return Serializer.DeserializeWithLengthPrefix<SCPacketHeader>(source, PrefixStyle.Fixed32);
    }

    /// <summary>
    /// 反序列化包体
    /// </summary>
    /// <param name="csPacketHeader"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public SCPacketBase DeserializePacket2(SCPacketHeader csPacketHeader, Stream source)
    {
        if (csPacketHeader == null)
        {
            Console.WriteLine("Packet header is invalid.");
            return null;
        }

        SCPacketBase packet = null;
        if (csPacketHeader.IsValid)
        {
            Type packetType = GetServerToClientPacketType(csPacketHeader.Id);
            if (packetType != null)
            {
                var typeInfo = packetType.GetTypeInfo();
                var constructor = typeInfo.GetConstructor(Type.EmptyTypes);

                source.Position = 8;
                packet = (SCPacketBase)RuntimeTypeModel.Default.DeserializeWithLengthPrefix(source, constructor.Invoke(null), packetType, PrefixStyle.Fixed32, 0);

                //packet = Serializer.DeserializeWithLengthPrefix<SCPacketBase>(source, PrefixStyle.Fixed32);
            }
            else
            {
                Console.WriteLine("Can not deserialize packet for packet id '{0}'.", csPacketHeader.Id.ToString());
            }
        }
        else
        {
            Console.WriteLine("Packet header is invalid.");
        }
        return packet;
    }



    private Type GetServerToClientPacketType(int id)
    {
        Type type = null;
        if (m_ServerToClientPacketTypes.TryGetValue(id, out type))
        {
            return type;
        }

        return null;
    }

    public bool Serialize<T>(T packet) where T : Packet
    {
        //m_CachedStream.WriteTo(destination);

        //using (MemoryStream ms = new MemoryStream())
        //{
        //    Serializer.SerializeWithLengthPrefix(ms, packet, PrefixStyle.Fixed32);
        //    var length = ms.ToArray().Length;
        //}

        using (MemoryStream destination = new MemoryStream())
        {
            PacketBase packetImpl = packet as PacketBase;
            if (packetImpl == null)
            {
                Console.WriteLine("Packet is invalid.");
                return false;
            }

            if (packetImpl.PacketType != PacketType.ServerToClient)
            {
                Console.WriteLine("Send packet invalid.");
                return false;
            }

            //m_CachedStream.SetLength(m_CachedStream.Capacity); // 此行防止 Array.Copy 的数据无法写入
            //m_CachedStream.Position = 0L;

            destination.Position = 8L;
            Serializer.SerializeWithLengthPrefix(destination, packet, PrefixStyle.Fixed32);
            //RuntimeTypeModel.Default.SerializeWithLengthPrefix(destination, packet, packet.GetType(), PrefixStyle.Fixed32, 0);

            SCPacketHeader packetHeader = new SCPacketHeader();
            packetHeader.Id = packet.Id;
            packetHeader.PacketLength = (int)destination.Length - 8;
            destination.Position = 0;
            Serializer.SerializeWithLengthPrefix(destination, packetHeader, PrefixStyle.Fixed32);

            waitToSendBytes = destination.ToArray();
        }

        return true;
    }

    public override byte[] Encode()
    {
        return waitToSendBytes;
    }
}
