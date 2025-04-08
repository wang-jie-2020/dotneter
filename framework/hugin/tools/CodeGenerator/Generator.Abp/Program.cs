using Generator;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Windows;

namespace AbpGenerator
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var defaultConfig = Path.Combine(Directory.GetParent(typeof(Program).Assembly.Location).FullName, "solutionInfo.json");

            if (args != null && args.Length > 0)
            {
                defaultConfig = args[0];
            }

            if (!File.Exists(defaultConfig))
            {
                throw new Exception("no config");
            }

            try
            {
                InitGlobal(defaultConfig);
                new App().Run(new MainWindow());
            }
            catch (Exception ex)
            {
                MessageBox.Show("意外错误:" + ex.Message);
            }
        }

        private static void InitGlobal(string configPath)
        {
            Global.GeneratorSolution = JsonConvert.DeserializeObject<GeneratorSolution>(File.ReadAllText(configPath, Encoding.UTF8));
            if (Global.GeneratorSolution == null || string.IsNullOrWhiteSpace(Global.GeneratorSolution.CurrentSelectedItem))
            {
                throw new Exception("wrong config");
            }

            Global.Entity = new GeneratorEntity(File.ReadAllText(Global.GeneratorSolution.CurrentSelectedItem, Encoding.UTF8));
        }
    }
}
