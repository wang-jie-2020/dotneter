using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.AspNetCore.Mvc;

namespace MQ.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AdminClientController : ControllerBase
{
    [HttpGet]
    public async Task<object> CreateTopicExample()
    {
        var config = new AdminClientConfig
        {
            BootstrapServers = Global.SERVERS
        };

        using (var adminClient = new AdminClientBuilder(config).Build())
        {
            try
            {
                //Topic已经存在时throw CreateTopicsException
                await adminClient.CreateTopicsAsync(new[]
                {
                    new TopicSpecification { Name = "hello-topic1", ReplicationFactor = 3, NumPartitions = 2 },
                    new TopicSpecification { Name = "hello-topic2", ReplicationFactor = 3, NumPartitions = 2 },
                    new TopicSpecification { Name = "hello-topic3", ReplicationFactor = 3, NumPartitions = 2 }
                });
            }
            catch (CreateTopicsException e)
            {
                Console.WriteLine($"An error occurred creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
            }

            try
            {
                //Topic不存在时throw DescribeTopicsException
                var desc = await adminClient.DescribeTopicsAsync(TopicCollection.OfTopicNames(new[] { "hello-topic" }));
                return desc;
            }
            catch (DescribeTopicsException e)
            {
                Console.WriteLine($"An error occurred describe topic {e.Results.TopicDescriptions}: {e.Results}");
            }
        }

        return Ok();
    }

    [HttpGet]
    public async Task<object> CreateTopic(string topic, int numPartitions = -1, short replicationFactor = -1)
    {
        var config = new AdminClientConfig
        {
            BootstrapServers = Global.SERVERS
        };

        using (var adminClient = new AdminClientBuilder(config).Build())
        {
            try
            {
                await adminClient.CreateTopicsAsync(new[]
                {
                    new TopicSpecification { Name = topic, ReplicationFactor = replicationFactor, NumPartitions = numPartitions }
                });
            }
            catch (CreateTopicsException e)
            {
                Console.WriteLine($"An error occurred creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
            }

            try
            {
                var desc = await adminClient.DescribeTopicsAsync(TopicCollection.OfTopicNames(new[] { topic }));
                return desc;
            }
            catch (DescribeTopicsException e)
            {
                Console.WriteLine($"An error occurred describe topic {e.Results.TopicDescriptions}: {e.Results}");
            }
        }

        return Ok();
    }

    [HttpGet]
    public async Task<object> GetMetadata()
    {
        var config = new AdminClientConfig
        {
            BootstrapServers = Global.SERVERS
        };

        using (var adminClient = new AdminClientBuilder(config).Build())
        {
            var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
            return Ok(metadata);
        }
    }
}