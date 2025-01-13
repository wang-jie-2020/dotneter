using Python.Runtime;
using static System.Net.Mime.MediaTypeNames;
using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Diagnostics;


public partial class EmbeddedPython
{
    static IConfiguration _configuration;

    public static void Run(IConfiguration configuration)
    {
        _configuration = configuration;

        //windows
        //var dist = new string[]
        //{
        //    @"C:\Users\jie.wang21\.pyenv\pyenv-win\versions\3.10.11\DLLs",
        //    @"C:\Users\jie.wang21\.pyenv\pyenv-win\versions\3.10.11\Lib",
        //    @"C:\Users\jie.wang21\.pyenv\pyenv-win\versions\3.10.11\libs",
        //    @"C:\Users\jie.wang21\.pyenv\pyenv-win\versions\3.10.11\Lib\site-packages",
        //    @"C:\Users\jie.wang21\.pyenv\pyenv-win\versions\3.10.11\Lib\site-packages/win32",
        //    @"C:\Users\jie.wang21\.pyenv\pyenv-win\versions\3.10.11\Lib\site-packages/win32/lib",
        //    @"C:\Users\jie.wang21\.pyenv\pyenv-win\versions\3.10.11\Lib\site-packages/Pythonwin",
        //    Path.Combine(Directory.GetCurrentDirectory(), "scripts")
        //};

        //Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", @"C:\Users\jie.wang21\.pyenv\pyenv-win\versions\3.10.11\python310.dll", EnvironmentVariableTarget.Process);
        //PythonEngine.PythonPath = string.Join(';', dist);



        //wsl python3.8
        //var dist = new string[]
        //{
        //    "/usr/lib/python38.zip",
        //    "/usr/lib/python3.8",
        //    "/usr/lib/python3.8/lib-dynload",
        //    "/usr/local/lib/python3.8/dist-packages",
        //    "/usr/lib/python3/dist-packages",
        //    Path.Combine(Directory.GetCurrentDirectory(), "scripts")
        //};

        //Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", "/usr/lib/x86_64-linux-gnu/libpython3.8.so.1.0", EnvironmentVariableTarget.Process);
        ////Environment.SetEnvironmentVariable("PYTHONPATH", string.Join(';', dist), EnvironmentVariableTarget.Process);    //同时PythonPath: /usr/lib/python38.zip:/usr/lib/python3.8:/usr/lib/python3.8/lib-dynload
        ////Environment.SetEnvironmentVariable("PYTHONPATH", string.Join(':', dist), EnvironmentVariableTarget.Process);    //同时PythonPath: /usr/lib/python38.zip:/usr/lib/python3.8:/usr/lib/python3.8/lib-dynload
        //PythonEngine.PythonPath = string.Join(':', dist);

        //docker
        var dist = configuration["PYTHONNET_PYPATH"];  //也可以 Environment.GetEnvironmentVariable("PYTHONNET_PYPATH"); 
        PythonEngine.PythonPath = string.Join(':', dist);

        if (!PythonEngine.IsInitialized)
        {
            PythonEngine.Initialize();
            dynamic sys = Py.Import("sys");
            dynamic os = Py.Import("os");

            Console.WriteLine("Python version: {0}", sys.version);
            Console.WriteLine("Current working directory: {0}", os.getcwd());
            Console.WriteLine("PythonPath: {0}", PythonEngine.PythonPath);
            Console.WriteLine("PythonHome: {0}", PythonEngine.PythonHome);
        }

        while (true)
        {
            Console.WriteLine("请输入命令：0; 退出程序，功能命令：1 - n");
            string input = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrEmpty(input))
            {
                continue;
            }

            if (input == "0")
            {
                break;
            }

            object? o = Activator.CreateInstance(typeof(EmbeddedPython));

            try
            {
                typeof(EmbeddedPython).InvokeMember("Method" + input,
                    BindingFlags.Static | BindingFlags.Instance |
                    BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.InvokeMethod,
                    null, o,
                    new object[] { });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    public void Method1()
    {
        using (Py.GIL())
        {
            dynamic np = Py.Import("numpy");
            Console.WriteLine(np.cos(np.pi * 2));

            dynamic sin = np.sin;
            Console.WriteLine(sin(5));

            double c = (double)(np.cos(5) + sin(5));
            Console.WriteLine(c);

            dynamic a = np.array(new List<float> { 1, 2, 3 });
            Console.WriteLine(a.dtype);

            dynamic b = np.array(new List<float> { 6, 5, 4 }, dtype: np.int32);
            Console.WriteLine(b.dtype);

            Console.WriteLine(a * b);

            using (PyModule scope = Py.CreateScope())
            {
                Person person = new Person("John", "Smith");
                PyObject pyPerson = person.ToPython();
                scope.Set("person", pyPerson);

                string code = "fullName = person.FirstName + ' ' + person.LastName;print(fullName)";
                var r1 = scope.Exec(code);
            }
        }
    }

    public void Method2()
    {
        using (Py.GIL())
        {
            using (PyModule scope = Py.CreateScope())
            {
                dynamic foo = Py.Import("PyImportTest.cast_global_var");
                Console.WriteLine(foo.FOO.ToString());
                Console.WriteLine(foo.test_foo().ToString());

                foo.FOO = 2;
                Console.WriteLine(foo.FOO.ToString());
                Console.WriteLine(foo.test_foo().ToString());
            }
        }
    }

    public void Method3()
    {
        using (Py.GIL())
        {
            using (PyModule scope = Py.CreateScope())
            {
                dynamic correlation = Py.Import("GalileoPython.correlation");
                var result = correlation.main("1.txt", "2.txt");
                Console.WriteLine(result.ToString());
            }
        }
    }

    public void Method4()
    {
        Process p = new Process();
        string pythonPath = "./scripts/GalileoPython/correlation.py";
        string sArguments = pythonPath;

        ArrayList arrayList = new ArrayList();
        arrayList.Add("1.txt");
        arrayList.Add("2.txt");

        foreach (var param in arrayList)//添加参数
        {
            sArguments += " " + param;
        }

        p.StartInfo.FileName = "C:\\Users\\jie.wang21\\.pyenv\\pyenv-win\\versions\\3.10.11\\python.exe";
        p.StartInfo.Arguments = sArguments;
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardError = true;
        p.StartInfo.CreateNoWindow = true;
        p.Start();//启动进程

        string output = p.StandardOutput.ReadToEnd();
        p.WaitForExit();
        p.Close();
        Console.WriteLine(output);
    }

    public void Method5()
    {
        Process p = new Process();
        string pythonPath = "./scripts/GalileoPython/correlation.py";
        string sArguments = pythonPath;

        ArrayList arrayList = new ArrayList();
        arrayList.Add("1.txt");
        arrayList.Add("2.txt");

        foreach (var param in arrayList)//添加参数
        {
            sArguments += " " + param;
        }

        p.StartInfo.FileName = "/usr/bin/python3.8";
        p.StartInfo.Arguments = sArguments;
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardError = true;
        p.StartInfo.CreateNoWindow = true;
        p.Start();//启动进程

        string output = p.StandardOutput.ReadToEnd();
        p.WaitForExit();
        p.Close();
        Console.WriteLine(output);
    }
}