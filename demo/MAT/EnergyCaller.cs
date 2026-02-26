using MathWorks.MATLAB.NET.Arrays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAT
{
    public class EnergyCaller
    {
        public Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (energy_pre.energy_pre energy = new energy_pre.energy_pre())
            {
                MWArray[] results = energy.energy_pre_initial(2, "LV", "280PP", "B", 600, "1P52S", 10);

                var mu_bankeng = results[0].ToArray();
                var sigma_bankeng = results[1].ToArray();

                return Task.CompletedTask;
            }
        }
    }
}
