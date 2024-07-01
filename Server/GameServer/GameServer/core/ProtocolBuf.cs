using ProtoBuf;
using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

public class ProtocolBuf : ProtocolBase
{
    private readonly Dictionary<int, Type> m_ServerToClientPacketTypes = new Dictionary<int, Type>();
    public void Initialize()
    {
        Type packetBaseType = typeof(CSPacketBase);
        //Type packetHandlerBaseType = typeof(PacketHandlerBase);
        Assembly assembly = Assembly.GetExecutingAssembly();
        Type[] types = assembly.GetTypes();
        for (int i = 0; i < types.Length; i++)
        {
            if (!types[i].IsClass || types[i].IsAbstract)
            {
                continue;
            }

            if (types[i].BaseType == packetBaseType)
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

    public CSPacketHeader DeserializePacketHeader(Stream source)
    {
        return Serializer.DeserializeWithLengthPrefix<CSPacketHeader>(source, PrefixStyle.Fixed32);    
    }

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

    private Type GetServerToClientPacketType(int id)
    {
        Type type = null;
        if (m_ServerToClientPacketTypes.TryGetValue(id, out type))
        {
            return type;
        }

        return null;
    }
}
