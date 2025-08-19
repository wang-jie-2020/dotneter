using System.Net;
using SMBLibrary;
using SMBLibrary.Client;

namespace Grammar;

public class SMB2
{
    void Method1()
    {
        SMB2Client client = new SMB2Client(); 
        bool isConnected = client.Connect(IPAddress.Parse("10.210.81.83"), SMBTransportType.DirectTCPTransport);
        if (isConnected)
        {
            NTStatus status = client.Login(String.Empty, "111", "222");
            if (status == NTStatus.STATUS_SUCCESS)
            {
                List<string> shares = client.ListShares(out status);
                foreach (var share in shares)
                {
                    Console.WriteLine(share);
                }
                client.Logoff();
            }
            client.Disconnect();
        }
    }    
}

