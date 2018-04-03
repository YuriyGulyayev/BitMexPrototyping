namespace BitMexPrototyping.Applications.BitMexWebSocketApiFacadePrototype
{
   [System.Security.SuppressUnmanagedCodeSecurityAttribute()]
   internal static class TProgram
   {
      private const string ApiKeyId_ = @"ztDPLLk-2yREPICQ16d6nbm3";
      private const string ApiKeySecret_ = @"ogiSNTdmPjig2YlHW7MvERTGfpNjAriqAp1tUTTGtd28wcgg";

      /// <summary>
      /// yg? Is this still needed? Contrary to what their docs says, it appears that this is still needed.
      /// yg? At least for websocket authentication this has to be used.
      /// This shall be under 2**53, which is about 9007 trillion, and this formula currently produces about 1814 trillion.
      /// {....DateTime.MaxValue} produces about 8990 trillion.
      /// </summary>
      private static ulong Nonce_ = (ulong) System.DateTime.UtcNow.Ticks / (ulong) (System.TimeSpan.TicksPerSecond / 28490);

      private static bool StaticBool1_;

      private static void Main
         ( //string[] arguments
         )
      {
         //// yg? Hack. This is needed only for this API key.
         //{
         //   //// todo.4 This will no longer be needed at some point, maybe in 100 years.
         //   //Nonce_ = 8915724276774411UL + (Nonce_ - 1813627394850329UL) / 8192U;
         //
         //   // todo.3 This will become too big soon.
         //   Nonce_ = (ulong) (System.DateTime.UtcNow.Ticks - (new System.DateTime(1970, 1, 1)).Ticks);
         //}

         // todo await ???
         Prototype201803072();

         do
         {
            System.Threading.Thread.Sleep(1);
         }
         while(! StaticBool1_);
      }

      private static async void Prototype201803072()
      {
         await Prototype201803072_1().ConfigureAwait(false);
      }

      private static async System.Threading.Tasks.Task Prototype201803072_1()
      {
         // todo We need exception handling here.

         {
            double dateTimeFrequencyToPhaseFrequencyRatio =
               (double) System.TimeSpan.TicksPerSecond / (double) System.Diagnostics.Stopwatch.Frequency;
            long basePhase = System.Diagnostics.Stopwatch.GetTimestamp();
            long baseUtcDateTimeInTicks = System.DateTime.UtcNow.Ticks;
            basePhase = System.Diagnostics.Stopwatch.GetTimestamp();
            baseUtcDateTimeInTicks = System.DateTime.UtcNow.Ticks;

            var clientWebSocket = new System.Net.WebSockets.ClientWebSocket();
            await
               clientWebSocket.ConnectAsync
                  ( new System.Uri
                        (@"wss://testnet.bitmex.com/realtime"),
                        //(@"wss://bitmex.com/realtime"),
                     System.Threading.CancellationToken.None
                  );

               // todo ??? The caller really should do this if needed.
               // todo ??? But any method really should do this
               // todo ??? if after {await} it's not supposed to return to the synchronization context.
               //// todo Comment in other places where we don't call this.
               //.ConfigureAwait(false);

            if(clientWebSocket.State == System.Net.WebSockets.WebSocketState.Open)
            {
               {
                  const string message = @"GET/realtime";
                  string nonceAsString = (++ Nonce_).ToString();

                  // todo Use ASCII encoding everywhere.
                  byte[] signatureBytes =
                     hmacsha256
                        ( System.Text.Encoding.UTF8.GetBytes(ApiKeySecret_),
                           System.Text.Encoding.UTF8.GetBytes(message + nonceAsString)
                        );

                  string signatureString = ByteArrayToString(signatureBytes);
                  /*const*/ string requestString =
                     "{" +
                        "\"op\":\"authKey\"," +
                        "\"args\":[\"" + ApiKeyId_ + "\"," + nonceAsString + ",\"" + signatureString + "\"]" +
                     "}";
                  byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(requestString);
                  await clientWebSocket.SendAsync
                     ( new System.ArraySegment<byte>(requestBytes),
                        System.Net.WebSockets.WebSocketMessageType.Text,
                        true,
                        System.Threading.CancellationToken.None
                     );
               }

               if(clientWebSocket.State == System.Net.WebSockets.WebSocketState.Open)
               {
                  {
                     const string subscriptionTopicName =
                        //@"quote:XBTUSD";
                        //@"trade:XBTUSD";
                        //@"orderBookL2:XBTUSD";
                        //@"instrument:XBTUSD";
                        //@"instrument";
                        //@"liquidation";
                        //@"quoteBin1m:XBTUSD";
                        @"order";

                     const string requestString =
                        "{" +
                           "\"op\":\"subscribe\"," +
                           "\"args\":[\"" + subscriptionTopicName + "\"]" +
                        "}";
                     byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(requestString);
                     await clientWebSocket.SendAsync
                        ( new System.ArraySegment<byte>(requestBytes),

                           // todo ??? For GDAX binary didn't work, right?
                           System.Net.WebSockets.WebSocketMessageType.Text,
                           //System.Net.WebSockets.WebSocketMessageType.Binary,

                           true,
                           System.Threading.CancellationToken.None
                        );
                  }

                  // todo For GDAX this really doesn't need to be more than 16K, right?
                  var receiveBuffer = new System.ArraySegment<byte>(new byte[33 * 1024]);

                  for(bool isMessageBegin = true; ;)
                  {
                     if(clientWebSocket.State != System.Net.WebSockets.WebSocketState.Open)
                     {
                        System.Console.WriteLine(@"201802085");
                        break;
                     }
                     else
                     {
                     }

                     System.Net.WebSockets.WebSocketReceiveResult webSocketReceiveResult =
                        await clientWebSocket.ReceiveAsync(receiveBuffer, System.Threading.CancellationToken.None);

                     // todo Is this correct?
                     // todo Are these conditions equivalent?
                     if(webSocketReceiveResult.CloseStatus.HasValue || webSocketReceiveResult.Count <= 0)
                     {
                        System.Console.WriteLine(@"201802086");
                        break;
                     }
                     else
                     {
                     }

                     if(isMessageBegin)
                     {
                        isMessageBegin = false;
                        System.Console.WriteLine(',');
                        //System.Console.WriteLine();
                        long currentUtcDateTimeInTicks =
                           (long) ((double) (ulong) (System.Diagnostics.Stopwatch.GetTimestamp() - basePhase) * dateTimeFrequencyToPhaseFrequencyRatio + 0.5) +
                           baseUtcDateTimeInTicks;
                        System.Console.Write
                           ((new System.DateTime(currentUtcDateTimeInTicks)).ToString(@"o", System.Globalization.DateTimeFormatInfo.InvariantInfo));
                     }
                     else
                     {
                     }

                     // todo Preserve decoding state between decoding chunks.
                     string string1 =
                        System.Text.Encoding.ASCII.GetString(receiveBuffer.Array, 0, webSocketReceiveResult.Count);

                     System.Console.Write(string1);

                     if(webSocketReceiveResult.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
                     {
                        System.Console.WriteLine(@"201802087");
                        break;
                     }
                     else
                     {
                     }

                     if(webSocketReceiveResult.EndOfMessage)
                     {
                        isMessageBegin = true;
                        //break;
                     }
                     else
                     {
                     }
                  }
               }
               else
               {
                  System.Console.WriteLine(@"201802088");
               }
            }
            else
            {
               System.Console.WriteLine(@"201804021");
            }

            // todo ??? WebSocketState >= CloseSent

            clientWebSocket.Dispose();
         }

         StaticBool1_ = true;
      }

      private static byte[] hmacsha256
         ( byte[] keyByte,
            byte[] messageBytes
         )
      {
         using(var hash = new System.Security.Cryptography.HMACSHA256(keyByte))
         {
            return hash.ComputeHash(messageBytes);
         }
      }

      public static string ByteArrayToString
         ( byte[] ba
         )
      {
         var hex = new System.Text.StringBuilder(ba.Length * 2);

         foreach (byte b in ba)
         {
            hex.AppendFormat("{0:x2}", b);
         }

         return hex.ToString();
      }
   }
}
