using System.Net.NetworkInformation;
using Snow.Utils;

namespace Snow
{
    public static class IdGenerator
    {
        private static readonly object SyncRoot = new object();
        private static SnowflakeId? _snow;

        public static long Create()
        {
            if (_snow == null)
            {
                lock (SyncRoot)
                {
                    if (!long.TryParse(Environment.GetEnvironmentVariable("SNOW_WORKERID"), out var workerId))
                    {
                        workerId = 0L;
                    }

                    _snow = new SnowflakeId(workerId);
                }
            }

            return _snow.NextId();
        }
        
        /// <summary>
        /// auto generate workerId, try using mac first, if failed, then randomly generate one
        /// </summary>
        /// <returns>workerId</returns>
        public static long GenerateWorkerId(int maxWorkerId)
        {
            try
            {
                return GenerateWorkerIdBaseOnMac();
            }
            catch
            {
                return GenerateRandomWorkerId(maxWorkerId);
            }
        }

        /// <summary>
        /// use lowest 10 bit of available MAC as workerId
        /// </summary>
        /// <returns>workerId</returns>
        private static long GenerateWorkerIdBaseOnMac()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            //exclude virtual and Loopback
            var firstUpInterface = nics.OrderByDescending(x => x.Speed).FirstOrDefault(x =>
                !x.Description.Contains("Virtual") && x.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                x.OperationalStatus == OperationalStatus.Up);
            if (firstUpInterface == null) throw new Exception("no available mac found");
            var address = firstUpInterface.GetPhysicalAddress();
            var mac = address.GetAddressBytes();

            return ((mac[4] & 0B11) << 8) | (mac[5] & 0xFF);
        }

        /// <summary>
        /// randomly generate one as workerId
        /// </summary>
        /// <returns></returns>
        private static long GenerateRandomWorkerId(int maxWorkerId)
        {
            return new Random().Next(maxWorkerId + 1);
        }
    }
}