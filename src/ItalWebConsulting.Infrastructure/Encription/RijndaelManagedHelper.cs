using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ItalWebConsulting.Infrastructure.Encription
{
    public static class RijndaelManagedHelper
    {
        private const string keyValue = "AxTYQWCvGTFRbgLL";
        private const string ivValue = "QWExcfTyUxxLOafO";
        public static string Encode(string stringToEncode)
        {
            var rjm = new RijndaelManaged();
            rjm.KeySize = 128;
            rjm.BlockSize = 128;
            rjm.Key = ASCIIEncoding.ASCII.GetBytes(keyValue);
            rjm.IV = ASCIIEncoding.ASCII.GetBytes(ivValue);
            var input = Encoding.UTF8.GetBytes(stringToEncode);

            var output = rjm.CreateEncryptor().TransformFinalBlock(input, 0, input.Length);
            return Convert.ToBase64String(output);
        }

        /*
         * 
         Public Shared Function Decode(ByVal TextString As String) As String
                Dim rjm As New RijndaelManaged

                rjm.KeySize = 128
                rjm.BlockSize = 128
                rjm.Key = ASCIIEncoding.ASCII.GetBytes(keyValue)
                rjm.IV = ASCIIEncoding.ASCII.GetBytes(ivValue)
                Try
                    Dim input() As Byte = Convert.FromBase64String(TextString)
                    Dim output() As Byte = rjm.CreateDecryptor().TransformFinalBlock(input, 0, input.Length)
                    Return Encoding.UTF8.GetString(output)
                Catch ex As Exception
                    Return TextString
                End Try
            End Function
         */
    }
}
