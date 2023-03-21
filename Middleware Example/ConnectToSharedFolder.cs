using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Net;
using static Vanara.PInvoke.Mpr;

namespace WindowAuthDemo
{
    public class ConnectToSharedFolder : IDisposable
    {
        readonly string _networkName;

        public ConnectToSharedFolder(string networkName)
        {
            _networkName = networkName;

            var netResource = new NetResource
            {
                Scope = ResourceScope.GlobalNetwork,
                ResourceType = ResourceType.Disk,
                DisplayType = ResourceDisplaytype.Share,
                RemoteName = networkName
            };

            var userName = string.Format(@"{0}\{1}", "VALV", "WIPUser");

            var result = WNetAddConnection2(
                new NETRESOURCE(networkName),
                "Y9oOf416BUr1",
                userName,
                Vanara.PInvoke.Mpr.CONNECT.CONNECT_TEMPORARY);

            if (((Vanara.PInvoke.HRESULT)result).Code != 0)
            {
                throw new Win32Exception(((Vanara.PInvoke.HRESULT)result).Code, "Error connecting to remote share");
            }
        }

        ~ConnectToSharedFolder()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            WNetCancelConnection2(_networkName, 0, true);
        }

        [StructLayout(LayoutKind.Sequential)]
        public class NetResource
        {
            public ResourceScope Scope;
            public ResourceType ResourceType;
            public ResourceDisplaytype DisplayType;
            public int Usage;
            public string LocalName;
            public string RemoteName;
            public string Comment;
            public string Provider;
        }

        public enum ResourceScope : int
        {
            Connected = 1,
            GlobalNetwork,
            Remembered,
            Recent,
            Context
        };

        public enum ResourceType : int
        {
            Any = 0,
            Disk = 1,
            Print = 2,
            Reserved = 8,
        }

        public enum ResourceDisplaytype : int
        {
            Generic = 0x0,
            Domain = 0x01,
            Server = 0x02,
            Share = 0x03,
            File = 0x04,
            Group = 0x05,
            Network = 0x06,
            Root = 0x07,
            Shareadmin = 0x08,
            Directory = 0x09,
            Tree = 0x0a,
            Ndscontainer = 0x0b
        }
    }
}
