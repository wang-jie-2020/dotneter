
namespace WinForms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private static readonly HttpClient s_httpClient = new HttpClient();

    private void button1_Click(object sender, EventArgs e)
    {
        // Console.WriteLine(SynchronizationContext.Current);
        // Task.Run(() =>
        // {
        //     this.button1.Text = "abcd"; //在NetCore环境下这么写不会再抛出ui线程错误了!!!
        // });

        // //但是这种形式的死锁案例下,仍旧死锁???
        // var task = AsyncTask();
        // Console.WriteLine(task.Result);
    }

    async Task<string> AsyncTask()
    {
        var task = Task.Run(() =>
        {
            Thread.Sleep(1000);
            return "123";
        });

        await task;
        return task.Result;
    }
}