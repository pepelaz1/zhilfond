using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.ServiceModel.Channels;
using DAL.Models;
using DAL.Interfaces;
using DAL.Repositories;
using System.IO;

namespace UI.Classes
{
    public static class Utils
    {
      //  static readonly ISessionsRepository rep_sessions = new SessionsRepository();
        static readonly IUsersRepository rep_users = new UsersRepository();

        public static string GetClientIp(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop;
                prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else
            {
                return "127.0.0.1";
            }
        }

        public static User  GetCurrentUser()
        {
            //Session s = rep_sessions.GetByToken(HttpContext.Current.Session["token"].ToString());
            //MemCacheProvider p = new MemCacheProvider();
            //return p.GetUser(HttpContext.Current.Session["token"].ToString());  
            return rep_users.GetByToken(HttpContext.Current.Session["token"].ToString());
        }

        public static string GetOperation(string srop)
        {
            if (srop == "eq")
                return "=";
            if (srop == "cn")
                return "like";

            return "";
        }

        public static byte[] StreamToArray(Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

    }    
}