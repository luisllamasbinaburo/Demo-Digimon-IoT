using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace NFCAndroid
{
    internal class Utils
    {
        internal static long TagId_To_CardId(byte[] tagIdBytes)
        {
            var tagIdString = Utils.ByteArrayToString(tagIdBytes);
            var reverseHex = Utils.LittleEndian(tagIdString);
            var cardId = Convert.ToInt64(reverseHex, 16);
            return cardId;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            var shb = new SoapHexBinary(ba);
            return shb.ToString();
        }

        internal static string LittleEndian(string num)
        {
            var number = Convert.ToInt32(num, 16);
            var bytes = BitConverter.GetBytes(number);
            return bytes.Aggregate("", (current, b) => current + b.ToString("X2"));
        }
    }
}