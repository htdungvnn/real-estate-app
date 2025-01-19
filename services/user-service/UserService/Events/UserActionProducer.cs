using Confluent.Kafka;

public class UserActionProducer
{
    public async Task ProduceMessageAsync(string topic, string key, string message)
    {
        var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
        using var producer = new ProducerBuilder<string, string>(config).Build();
        await producer.ProduceAsync(topic, new Message<string, string> { Key = key, Value = message });
    }
}