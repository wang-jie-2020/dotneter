using MySqlConnector;

namespace basic.线程;

public class TaskExceptionDemo
{
    public void Method1()
    {
        ExecuteAsync().Wait();
    }

    private async Task ExecuteAsync()
    {
        // Parallel.For(1, 10, async index =>
        // {
        //     SavingAsync(index);
        // });

        // await Task.WhenAll(Enumerable.Range(1, 10).Select(async p =>
        // {
        //     try
        //     {
        //         await SavingAsync(p);
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(e);
        //         throw;
        //     }
        // }));

        //await Task.WhenAll(Enumerable.Range(1, 10).Select(async p =>
        //{
        //    await SavingAsync(p);
        //}));

        //try
        //{
        //    await Task.WhenAll(Enumerable.Range(1, 10).Select(p =>
        //    {
        //        return SavingAsync(p);
        //    }));
        //}
        //catch (Exception e)
        //{
        //    Console.WriteLine(e);
        //    throw;
        //}

        await Task.WhenAll(Enumerable.Range(1, 10).Select(async p => { await SavingAsync(p); }));

        Console.WriteLine("all end");
    }

    private static string connectionString =
        "server=10.205.128.47;userid=root;password=Qwer12343!@;database=APC_PRD;Allow User Variables=true;SslMode=none;allowPublicKeyRetrieval=true;pooling=true;Max Pool Size=300";

    private async Task SavingAsync(int index)
    {
        Console.WriteLine($"task{index} start");

        if (index == 4)
        {
            await Task.Delay(2000);
            // throw new Exception("error when index = 4");
        }

        if (index == 6)
        {
            bool shouldContinue = true;

            while (shouldContinue)
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = connection.CreateCommand())
                    {
                        var idsToDelete = new List<int>();
                    
                        command.CommandText = $"SELECT ID FROM APC_EQUIPMENTCHANGE WHERE CreateTime <= '2024-01-15 00:00:00.000' AND DoHandleType = 1 ORDER BY ID Limit {idsToDelete.Count},1000";
                        command.CommandTimeout = 5;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            try
                            {
                                while (reader.Read())
                                {
                                    idsToDelete.Add(reader.GetInt32(reader.GetOrdinal("ID")));
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                throw;
                            }
                        }
                    
                        if (idsToDelete.Count > 0)
                        {
                            string deleteQuery = $"select sleep(2)";
                            using (var deleteCommand = connection.CreateCommand())
                            {
                                deleteCommand.CommandText = deleteQuery;
                                await deleteCommand.ExecuteNonQueryAsync();
                            }
                        }

                        shouldContinue = idsToDelete.Count > 0;
                    }
                }
            }
        }

        Console.WriteLine($"task{index} end");
    }
}