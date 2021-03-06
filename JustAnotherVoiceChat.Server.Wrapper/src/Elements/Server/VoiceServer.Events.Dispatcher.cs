﻿/*
 * File: VoiceServer.Events.Dispatcher.cs
 * Date: 21.2.2018,
 *
 * MIT License
 *
 * Copyright (c) 2018 JustAnotherVoiceChat
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using JustAnotherVoiceChat.Server.Wrapper.Elements.Models;
using JustAnotherVoiceChat.Server.Wrapper.Enums;
using JustAnotherVoiceChat.Server.Wrapper.Interfaces;

namespace JustAnotherVoiceChat.Server.Wrapper.Elements.Server
{
    public partial class VoiceServer<TClient, TIdentifier> where TClient : IVoiceClient
    {
        
        private bool OnClientConnectingFromVoice(ushort handle, string teamspeakId)
        {
            Log(LogLevel.Trace, $"OnClientConnectingFromVoice({handle}, {teamspeakId})");
            return RunWhenClientValid(handle, client =>
            {
                var eventArgs = new ClientConnectingEventArgs();
                InvokeProtectedEvent(() => OnClientConnecting?.Invoke(client, teamspeakId, eventArgs));
                
                return !eventArgs.Reject;
            });
        }

        private void OnClientConnectedFromVoice(ushort handle)
        {
            Log(LogLevel.Trace, $"OnClientConnectedFromVoice({handle})");
            RunWhenClientValid(handle, client =>
            {
                InvokeProtectedEvent(() => OnClientConnected?.Invoke(client));
            });
        }
        
        private void OnClientRejectedFromVoice(ushort handle, int statusCode)
        {
            Log(LogLevel.Trace, $"OnClientRejectedFromVoice({handle}, {statusCode})");
            RunWhenClientValid(handle, client =>
            {                
                InvokeProtectedEvent(() => OnClientRejected?.Invoke(client, (StatusCode) statusCode));
            });
        }

        private void OnClientDisconnectedFromVoice(ushort handle)
        {
            Log(LogLevel.Trace, $"OnClientDisconnectedFromVoice({handle})");
            RunWhenClientValid(handle, client =>
            {
                InvokeProtectedEvent(() => OnClientDisconnected?.Invoke(client));
            });
        }

        private void OnClientTalkingStatusChangedFromVoice(ushort handle, bool newStatus)
        {
            Log(LogLevel.Trace, $"OnClientTalkingStatusChangedFromVoice({handle}, {newStatus})");
            RunWhenClientValid(handle, client =>
            {
                InvokeProtectedEvent(() => OnClientTalkingChanged?.Invoke(client, newStatus));
            });
        }

        private void OnClientSpeakersMuteChangedFromVoice(ushort handle, bool newStatus)
        {
            Log(LogLevel.Trace, $"OnClientSpeakersMuteChangedFromVoice({handle}, {newStatus})");
            RunWhenClientValid(handle, client =>
            {
                InvokeProtectedEvent(() => OnClientSpeakersMuteChanged?.Invoke(client, newStatus));
            });
        }

        private void OnClientMicrophoneMuteChangedFromVoice(ushort handle, bool newStatus)
        {
            Log(LogLevel.Trace, $"OnClientMicrophoneMuteChangedFromVoice({handle}, {newStatus})");
            RunWhenClientValid(handle, client =>
            {
                InvokeProtectedEvent(() => OnClientMicrophoneMuteChanged?.Invoke(client, newStatus));
            });
        }

        private void OnLogMessageFromVoice(string message, int loglevel)
        {
            Log((LogLevel) loglevel, message);
        }
        
        
        
        internal T RunWhenClientValid<T>(ushort handle, Func<TClient, T> callback)
        {
            var client = GetVoiceClient(handle);
            
            if (client == null)
            {
                return default(T);
            }

            return callback(client);
        }

        internal void RunWhenClientValid(ushort handle, Action<TClient> callback)
        {
            var client = GetVoiceClient(handle);

            if (client == null)
            {
                return;
            }
            
            callback(client);
        }
        
        internal T RunWhenClientConnected<T>(ushort handle, Func<TClient, T> callback)
        {
            return RunWhenClientValid(handle, client =>
            {
                if (!client.Connected)
                {
                    return default(T);
                }

                return callback(client);
            });
        }

        internal void RunWhenClientConnected(ushort handle, Action<TClient> callback)
        {
            RunWhenClientValid(handle, client =>
            {
                if (!client.Connected)
                {
                    return;
                }

                callback(client);
            });
        }
    }
}
