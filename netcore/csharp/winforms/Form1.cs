namespace winforms;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        var context1 = SynchronizationContext.Current;

        Task.Run(() =>
        {
            var context2 = SynchronizationContext.Current;
            this.button1.Text = @"Modified 1!";
        });
    }

    private void button2_Click(object sender, EventArgs e)
    {
        var context1 = SynchronizationContext.Current;

        Task.Run(() =>
        {
            var context2 = SynchronizationContext.Current;
            this.button1.Invoke(() =>   //这里故意写的button1改button2
            {
                this.button2.Text = @"Modified 2!";
            });
        });
    }

    private void button3_Click(object sender, EventArgs e)
    {
        var context1 = SynchronizationContext.Current;

        Task.Run(() =>
        {
            var context2 = SynchronizationContext.Current;
            context1?.Send(d =>
            {
                Thread.Sleep(2000);
                this.button3.Text = @"Modified 3.1!";
            }, null);
        });
        Console.WriteLine(@"结束1");

        //Task.Run(() =>
        //{
        //    var context2 = SynchronizationContext.Current;
        //    context1?.Post(d =>
        //    {
        //        Thread.Sleep(2000);
        //        this.button3.Text = @"Modified 3.2!";
        //    }, null);
        //});
        //Console.WriteLine(@"结束2");
    }
}