﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MegaApi
{
    public enum Error
    {
        // General errors:
        EINTERNAL = -1, // An internal error has occurred. Please submit a bug report, detailing the exact circumstances in which this error occurred.
        EARGS = -2, // You have passed invalid arguments to this command.
        EAGAIN = -3, // (always at the request level, // A temporary congestion or server malfunction prevented your request from being processed. No data was altered. Retry. Retries must be spaced with exponential backoff.
        ERATELIMIT = -4, // You have exceeded your command weight per time quota. Please wait a few seconds, then try again (this should never happen in sane real-life applications).

        // Upload errors:
        EFAILED = -5, // The upload failed. Please restart it from scratch.
        ETOOMANY = -6, // Too many concurrent IP addresses are accessing this upload target URL.
        ERANGE = -7, // The upload file packet is out of range or not starting and ending on a chunk boundary.
        EEXPIRED = -8, // The upload target URL you are trying to access has expired. Please request a fresh one.

        // Filesystem/Account-level errors:
        ENOENT = -9, // Object (typically, node or user) not found
        ECIRCULAR = -10, // Circular linkage attempted
        EACCESS = -11, // Access violation (e.g., trying to write to a read-only share)
        EEXIST = -12, // Trying to create an object that already exists
        EINCOMPLETE = -13, // Trying to access an incomplete resource
        EKEY = -14, // A decryption operation failed (never returned by the API)
        ESID = -15, // Invalid or expired user session, please relogin
        EBLOCKED = -16, // User blocked
        EOVERQUOTA = -17, // Request over quota
        ETEMPUNAVAIL = -18, // Resource temporarily not available, please try again later
    }



    public class NodeHandle
    {
        string handle; // 8 alpha chars
    }

    public class UserHandle
    {
        string handle; // 11 base64
    }

    public class Session
    {
        public int SessionId;
        public int SequenceNumber;

        public static Uri ApiRequestUrl = new Uri("https://g.api.mega.co.nz/cs");

        public Session()
        {
            SequenceNumber = new Random().Next();
        }

        private HttpWebRequest GetNextRequest()
        {
            UriBuilder ub = new UriBuilder(ApiRequestUrl);
            ub.Query += string.Format("id={0}", SequenceNumber);
            Uri uri = ub.Uri;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "text/plain;charset=UTF-8";
            request.Timeout = 10000;
            return request;
        }

        public string Execute(Command command)
        {
            HttpWebRequest request = GetNextRequest();

            string json = command.ToJson();

            byte[] content = Encoding.UTF8.GetBytes(json);

            request.ContentLength = content.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(content, 0, content.Length);
                requestStream.Close();
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            byte[] responseContent = new byte[response.ContentLength]; 

            using (var responseStream = response.GetResponseStream())
            {
                responseStream.Read(responseContent, 0, responseContent.Length);
                responseStream.Close();
            }

            string responseString = Encoding.UTF8.GetString(responseContent);

            JArray a = JArray.Parse(responseString);

            JToken t = a[0];

            command.OnCallback(t);

            return responseString;
        }
    }

    public class Command
    {
        class Argument
        {
            public string name;
            public string value;
        }

        public delegate void SuccessCallBack(JToken result);
        public delegate void ErrorCallBack(Error error);

        private string _command;
        private List<Argument> _arguments = new List<Argument>();
        private SuccessCallBack _successCallBack;
        private ErrorCallBack _errorCallBack;

        public Command(string command, SuccessCallBack successCallBack = null, ErrorCallBack errorCallBack = null)
        {
            _command = command;
            _successCallBack = successCallBack;
            _errorCallBack = errorCallBack;
        }

        public void AddArgument(string name, string value)
        {
            _arguments.Add(new Argument { name = name, value = value });
        }

        public string ToJson()
        {
            var sb = new StringBuilder();

            // in array of one for now
            sb.AppendFormat("[{{ \"a\" : \"{0}\"", _command);
            foreach (var arg in _arguments)
            {
                // need to quote strings only?
                sb.AppendFormat(", \"{0}\" : \"{1}\"", arg.name, arg.value);
            }
            sb.Append(" }]");
            return sb.ToString();
        }

        public void OnCallback(JToken result)
        {
            if (result.Type == JTokenType.Integer)
            {
                Error error = (Error)(int)result;

                if (_errorCallBack != null)
                {
                    _errorCallBack(error);
                }
            }
            else
            {
                if (_successCallBack != null)
                {
                    _successCallBack(result);
                }
            }
        }
    }

    public class LoginResonse
    {
        public string csid;
        public string privk; // private key
        public string k;
    }

    public static class MakeCommand
    {
        // 3 Client-server request reference
        // 3.1 Filesystem operations
        private static readonly string _RetrieveFolderOrUserNodes = "f"; // Returns the contents of the requested folder, or a full view of the requesting user's three filesystem trees, contact list, incoming shares and pending share key requests.
        private static readonly string _AddOrCopyNewNode = "p"; // Adds new nodes. Copies existing files and adds completed uploads to a user's filesystem.
        private static readonly string _DeleteNode = "d"; // Deletes a node, including all of its subnodes.
        private static readonly string _MoveNode = "m"; // Moves a node to a new parent node.
        private static readonly string _SetNodeAttributes = "a"; // Updates the encrypted node attributes object.
        private static readonly string _CreateOrDeletePublicHandle = "l"; // Enables or disables the public handle for a node.
        private static readonly string _CreateOrModifyOrDeleteOutgoingShare = "s"; // Controls the sharing status of a node.
        private static readonly string _KeyHandling = "k"; // Responds to or requests the following types of key processing:
        // cr - Set or request share/node keys
        // sr - Set share keys (in response to a share key request)
        // nk - Set node keys


        // 3.2 Account operations
        private static readonly string _AddOrUpdateUser = "up"; // Adds a new user, upgrades an existing user or sets/modifies a user's credentials.
        private static readonly string _GetUser = "ug"; // Retrieves user details.
        private static readonly string _RetrieveUsersPublicKey = "uk"; // Retrieves a user's RSA public key.
        private static readonly string _AddOrUpdateOrDeleteContract = "ur"; // Modifes the contact status of a given user.
        private static readonly string _InviteUser = "ui"; // Sends invitation e-mail to a user.
        private static readonly string _SendConfirmationEmail = "uc"; // Triggers the confirmation link to be sent to the registering user.
        private static readonly string _ObtainUserDetailsByInvitationCode = "uv"; // Retrieves user details based on the invitation code.
        private static readonly string _VerifyEmailedConfirmationCode = "ud"; // Upgrades an account's status based on the the given confirmation code.

        private static readonly string _LoginSessionChallengeOrResponse = "us"; // Establishes a user session based on the response to a cryptographic challenge.
        private static readonly string _ListUserSessions = "usl"; // Retrieves the user's session history.
        private static readonly string _UserQuotaDetails = "uq"; // Returns the current quota and resource utilization for the user and for the requesting IP address.
        
        // Used by web site
        private static readonly string _UserUpdate = "uu";

        public static Command Login(string user, string hash)
        {
            Command.SuccessCallBack successCallBack = (JToken result) =>
            {
                LoginResonse l = result.ToObject<LoginResonse>();
                Console.WriteLine(l);
            };
            Command.ErrorCallBack errorCallBack = (Error result) =>
            {
                switch (result)
                {
                case Error.ENOENT:
                    Console.WriteLine("ENOENT");
                    break;
                default:
                    Console.WriteLine(result);
                    break;
                }
            };
            var command = new Command(_LoginSessionChallengeOrResponse, successCallBack, errorCallBack);
            command.AddArgument("user", user);
            command.AddArgument("uh", hash);
            return command;
        }
    }
}
