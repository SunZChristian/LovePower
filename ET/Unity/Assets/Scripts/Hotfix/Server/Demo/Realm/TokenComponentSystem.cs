﻿using System;

namespace ET.Server
{
    [EntitySystemOf(typeof(TokenComponent))]
    [FriendOfAttribute(typeof(ET.Server.TokenComponent))]
    public static partial class TokenComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.TokenComponent self)
        {

        }

        public static void Add(this TokenComponent self, string key, string token)
        {
            self.TokenDictionary.Add(key, token);
            self.TimeOutRemoveKey(key, token).Coroutine();
        }

        public static string Get(this TokenComponent self, string key)
        {
            string value = String.Empty;
            self.TokenDictionary.TryGetValue(key, out value);
            return value;
        }

        public static void Remove(this TokenComponent self, string key)
        {
            if (self.TokenDictionary.ContainsKey(key))
            {
                self.TokenDictionary.Remove(key);
            }
        }

        private static async ETTask TimeOutRemoveKey(this TokenComponent self, string key, string tokenKey)
        {
            await self.Root().GetComponent<TimerComponent>().WaitAsync(600000);

            string onlineToken = self.Get(key);

            if (!string.IsNullOrEmpty(onlineToken) && onlineToken == tokenKey)
            {
                self.Remove(key);
            }

        }
    }
}