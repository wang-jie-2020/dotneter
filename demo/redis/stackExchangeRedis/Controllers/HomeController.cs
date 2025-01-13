using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Demo.Helpers;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Demo.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ConnectionMultiplexer _connection;
        private readonly RedisHelper _redis;

        public HomeController(ConnectionMultiplexer connection)
        {
            _connection = connection;
            _redis = new RedisHelper(connection);
        }

        public ActionResult<string> TestString()
        {
            StringBuilder sb = new StringBuilder();

            _redis.StringSet("redis_string_string", "123");
            sb.AppendLine(_redis.StringGet("redis_string_string"));

            SomeClass demo = new SomeClass()
            {
                Id = 1,
                Name = "123"
            };
            _redis.StringSet("redis_string_model", demo);
            var model = _redis.StringGet<SomeClass>("redis_string_model");
            sb.AppendLine(_redis.StringGet("redis_string_model"));

            for (int i = 0; i < 10; i++)
            {
                _redis.StringIncrement("StringIncrement", 2);
            }
            sb.AppendLine(_redis.StringGet("StringIncrement"));

            for (int i = 0; i < 5; i++)
            {
                _redis.StringDecrement("StringIncrement");
            }
            sb.AppendLine(_redis.StringGet("StringIncrement"));

            return sb.ToString();
        }

        public ActionResult<string> TestList()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 10; i++)
            {
                _redis.ListRightPush("list", i);
            }

            for (int i = 10; i < 20; i++)
            {
                _redis.ListLeftPush("list", i);
            }

            var length = _redis.ListLength("list");
            var list = _redis.ListRange<int>("list");
            sb.AppendLine($"当前长度{length}:数据是{string.Join(',', list.ToArray())}");


            var leftpop = _redis.ListLeftPop<string>("list");
            sb.AppendLine($"从左侧取出{leftpop}");

            var rightPop = _redis.ListRightPop<string>("list");
            sb.AppendLine($"从右侧取出{rightPop}");

            list = _redis.ListRange<int>("list");
            length = _redis.ListLength("list");
            sb.AppendLine($"当前长度{length}:数据是{string.Join(',', list.ToArray())}");


            list = _redis.ListTrim<int>("list", 0, 4);
            length = _redis.ListLength("list");
            sb.AppendLine($"当前长度{length}:数据是{string.Join(',', list.ToArray())}");

            return sb.ToString();
        }

        public ActionResult TestHash()
        {
            throw new NotImplementedException();

            //redis.HashSet("user", "u1", "123");
            //redis.HashSet("user", "u2", "1234");
            //redis.HashSet("user", "u3", "1235");
            //var news = redis.HashGet<string>("user", "u2");
        }

        public ActionResult TestSet()
        {
            throw new NotImplementedException();
        }

        public ActionResult TestSortedSet()
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult> TestTrans()
        {
            //redis的事务只是保证一次提交，但一次提交中若出现错误不会回滚，还是继续其他不错的

            string name = _redis.StringGet("tran_name");
            string age = _redis.StringGet("tran_age");

            Console.WriteLine("tran_name:" + name);
            Console.WriteLine("tran_age:" + age);

            var tran = _redis.CreateTransaction();

            //相当于Redis命令中的watch name
            tran.AddCondition(Condition.StringEqual("name", name));

            await tran.StringSetAsync("tran_name", "name_1");
            await tran.StringSetAsync("tran_age", "age1");
            bool committed = tran.Execute();
            Thread.Sleep(4000);

            Console.WriteLine("tran_name:" + _redis.StringGet("age"));
            Console.WriteLine("tran_age:" + _redis.StringGet("name"));

            return Ok();
        }

        public ActionResult TestPipeLine()
        {
            //减少请求的传输次数，Batch批量操作
            var batch = _redis.GetDatabase().CreateBatch();

            Task t1 = batch.StringSetAsync("name", "bob");
            Task t2 = batch.StringSetAsync("age", 100);
            batch.Execute();
            Task.WaitAll(t1, t2);

            Console.WriteLine("Name:" + _redis.StringGet("name"));
            Console.WriteLine("Age:" + _redis.StringGet("age"));

            return Ok();
        }

        public ActionResult TestLock()
        {
            throw new NotImplementedException();
            //var db = _redis.GetDatabase();
            //RedisValue token = Environment.MachineName;
            //if (db.LockTake("lock_test", token, TimeSpan.FromSeconds(10)))
            //{
            //    try
            //    {
            //        //TODO:开始做你需要的事情
            //        Thread.Sleep(5000);
            //    }
            //    finally
            //    {
            //        db.LockRelease("lock_test", token);
            //    }
            //}

            //return Ok();
        }

        public ActionResult TestPub()
        {
            _redis.Subscribe("Channel1", (channel, value) =>
            {
                Console.WriteLine("收到消息" + value);
            });

            for (int i = 0; i < 10; i++)
            {
                _redis.Publish("Channel1", "msg" + i);
                if (i == 8)
                {
                    _redis.Unsubscribe("Channel1");
                }
            }

            return Ok();
        }
    }

    public class SomeClass
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}