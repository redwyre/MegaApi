using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

    public class Command
    {
        class Argument
        {
            public string name;
            public string value;
        }

        private string _command;
        private List<Argument> _arguments = new List<Argument>();

        public static Uri ApiRequestUrl = new Uri("https://g.api.mega.co.nz/sc");

        public Command(string command)
        {
            _command = command;
        }

        public void AddArgument(string name, string value)
        {
            _arguments.Add(new Argument { name = name, value = value });
        }

        public string ToJson()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("{ a : '{0}'", _command);
            foreach (var arg in _arguments)
            {
                // need to quote strings only?
                sb.AppendFormat(", {0} : '{1}'", arg.name, arg.value);
            }
            sb.Append(" }");
            return sb.ToString();
        }
    }

    public static class MakeCommand
    {
        // 3 Client-server request reference
        // 3.1 Filesystem operations
        private static string _RetrieveFolderOrUserNodes = "f"; // Returns the contents of the requested folder, or a full view of the requesting user's three filesystem trees, contact list, incoming shares and pending share key requests.
        private static string _AddOrCopyNewNode = "p"; // Adds new nodes. Copies existing files and adds completed uploads to a user's filesystem.
        private static string _DeleteNode = "d"; // Deletes a node, including all of its subnodes.
        private static string _MoveNode = "m"; // Moves a node to a new parent node.
        private static string _SetNodeAttributes = "a"; // Updates the encrypted node attributes object.
        private static string _CreateOrDeletePublicHandle = "l"; // Enables or disables the public handle for a node.
        private static string _CreateOrModifyOrDeleteOutgoingShare = "s"; // Controls the sharing status of a node.
        private static string _KeyHandling = "k"; // Responds to or requests the following types of key processing:
        // cr - Set or request share/node keys
        // sr - Set share keys (in response to a share key request)
        // nk - Set node keys


        // 3.2 Account operations
        private static string _AddOrUpdateUser = "up"; // Adds a new user, upgrades an existing user or sets/modifies a user's credentials.
        private static string _GetUser = "ug"; // Retrieves user details.
        private static string _RetrieveUsersPublicKey = "uk"; // Retrieves a user's RSA public key.
        private static string _AddOrUpdateOrDeleteContract = "ur"; // Modifes the contact status of a given user.
        private static string _InviteUser = "ui"; // Sends invitation e-mail to a user.
        private static string _SendConfirmationEmail = "uc"; // Triggers the confirmation link to be sent to the registering user.
        private static string _ObtainUserDetailsByInvitationCode = "uv"; // Retrieves user details based on the invitation code.
        private static string _VerifyEmailedConfirmationCode = "ud"; // Upgrades an account's status based on the the given confirmation code.

        private static string _LoginSessionChallengeOrResponse = "us"; // Establishes a user session based on the response to a cryptographic challenge.
        private static string _ListUserSessions = "usl"; // Retrieves the user's session history.
        private static string _UserQuotaDetails = "uq"; // Returns the current quota and resource utilization for the user and for the requesting IP address.

        // 4 Server-client request reference


        // Used by web site

        private static string _UpdateStatus = "us";

        private static string _UpdateUser = "uu";

        public static Command Login(string user, string uh)
        {
            var command = new Command(_UpdateUser);
            command.AddArgument("user", user);
            command.AddArgument("uh", uh);
            return command;
        }
    }
}
