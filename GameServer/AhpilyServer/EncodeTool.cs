using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using ProtoBuf.Meta;
using Protocol.Base;
using Protocol.Header;

namespace AhpilyServer
{
    /// <summary>
    /// 关于编码的工具类
    /// </summary>
    public static class EncodeTool
    {

        #region 粘包拆包问题 封装一个有规定的数据包

        /// <summary>
        /// 构造数据包 ： 包头 + 包尾
        /// </summary>
        /// <returns></returns>
        public static byte[] EncodePacket(byte[] data)
        {
            //内存流对象
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    //先写入长度
                    bw.Write(data.Length);
                    //再写入数据
                    bw.Write(data);

                    byte[] byteArray = new byte[(int)ms.Length];
                    Buffer.BlockCopy(ms.GetBuffer(), 0, byteArray, 0, (int)ms.Length);

                    return byteArray;
                }
            }
        }

        /// <summary>
        /// 解析消息体 从缓存里取出一个一个完整的数据包 
        /// </summary>
        /// <returns></returns>
        public static byte[] DecodePacket(ref List<byte> dataCache)
        {
            //四个字节 构成一个int长度 不能构成一个完整的消息
            if (dataCache.Count < 4)
                return null;
            //throw new Exception("数据缓存长度不足4 不能构成一个完整的消息");

            using (MemoryStream ms = new MemoryStream(dataCache.ToArray()))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    // 1111 111 1
                    int length = br.ReadInt32();
                    int dataRemainLength = (int)(ms.Length - ms.Position);
                    //数据长度不够包头约定的长度 不能构成一个完整的消息
                    if (length > dataRemainLength)
                        return null;
                    //throw new Exception("数据长度不够包头约定的长度 不能构成一个完整的消息");

                    byte[] data = br.ReadBytes(length);
                    //更新一下数据缓存
                    dataCache.Clear();
                    dataCache.AddRange(br.ReadBytes(dataRemainLength));

                    return data;
                }
            }
        }

        #endregion

        #region 构造发送的SocketMsg类

        /// <summary>
        /// 把socketMsg类转换成字节数组 发送出去
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] EncodeMsg(SocketMsg msg)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(msg.OpCode);
            bw.Write(msg.SubCode);
            //如果不等于null  才需要把object 转换成字节数据 存起来
            if (msg.Value != null)
            {
                byte[] valueBytes = EncodeObj(msg.Value);
                bw.Write(valueBytes);
            }
            byte[] data = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(), 0, data, 0, (int)ms.Length);
            bw.Close();
            ms.Close();
            return data;
        }

        /// <summary>
        /// 将收到的字节数组 转换成 socketMsg对象 供我们使用
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static SocketMsg DecodeMsg(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            BinaryReader br = new BinaryReader(ms);
            SocketMsg msg = new SocketMsg();
            msg.OpCode = br.ReadInt32();
            msg.SubCode = br.ReadInt32();
            //还有剩余的字节没读取 代表 value  有值
            if (ms.Length > ms.Position)
            {
                byte[] valueBytes = br.ReadBytes((int)(ms.Length - ms.Position));
                object value = DecodeObj(valueBytes);
                msg.Value = value;
            }
            br.Close();
            ms.Close();
            return msg;
        }

        #endregion

        #region 把一个object类型转换成byte[]

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] EncodeObj(object value)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, value);
                byte[] valueBytes = new byte[ms.Length];
                Buffer.BlockCopy(ms.GetBuffer(), 0, valueBytes, 0, (int)ms.Length);
                return valueBytes;
            }
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <param name="valueBytes"></param>
        /// <returns></returns>
        public static object DecodeObj(byte[] valueBytes)
        {
            using (MemoryStream ms = new MemoryStream(valueBytes))
            {
                BinaryFormatter bf = new BinaryFormatter();
                object value = bf.Deserialize(ms);
                return value;
            }
        }

        #endregion

        #region ProtoBuf

        private static Dictionary<int, Type> m_ClientToServerPacketTypes = new Dictionary<int, Type>();

        public static void Initialize()
        {
            Type packetBaseType = typeof(SCPacketBase);

            Assembly assembly = Assembly.GetAssembly(packetBaseType);
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

                    m_ClientToServerPacketTypes.Add(packetBase.Id, types[i]);
                }
               
            }

            Console.WriteLine("初始化完成");
        }

        public static PacketHeaderBase DeserializePacketHeader(Stream source, out object customErrorData)
        {
            customErrorData = null;
            //var obj = ReferencePool.Acquire<SCPacketHeader>();
            //source.Seek(0, SeekOrigin.Begin);
            return Serializer.DeserializeWithLengthPrefix<CSPacketHeader>(source, PrefixStyle.Fixed32);
            //return (IPacketHeader)RuntimeTypeModel.Default.DeserializeWithLengthPrefix(source, ReferencePool.Acquire<SCPacketHeader>(), obj.GetType(), PrefixStyle.Fixed32, 0);
        }

        public static PacketBase DeserializePacket(PacketHeaderBase packetHeader, Stream source, out object customErrorData)
        {
            // 注意：此函数并不在主线程调用！
            customErrorData = null;

            CSPacketHeader csPacketHeader = packetHeader as CSPacketHeader;
            if (csPacketHeader == null)
            {
                Console.WriteLine("Packet header is invalid.");
                return null;
            }

            PacketBase packet = null;
            if (csPacketHeader.IsValid)
            {
                Type packetType = GetServerToClientPacketType(csPacketHeader.Id);
                if (packetType != null)
                {
                    packet = (PacketBase)RuntimeTypeModel.Default.DeserializeWithLengthPrefix(
                        source, Activator.CreateInstance(packetType), packetType, PrefixStyle.Fixed32, 0);
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

        private static Type GetServerToClientPacketType(int id)
        {
            Type type = null;
            if (m_ClientToServerPacketTypes.TryGetValue(id, out type))
            {
                return type;
            }

            return null;
        }

        #endregion
    }
}
