using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace hystrix.Test
{
    public interface IPerson
    {
        Task<string> HelloAsync(string name);

        Task<string> HiAsync(string name);

        //int Add(int i, int j);

        //void Test(int i);
    }
}
