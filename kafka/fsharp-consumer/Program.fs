open Confluent.Kafka
open FsKafka

let log = Serilog.LoggerConfiguration().CreateLogger()

let handler (messages : ConsumeResult<string,string> []) = async {
    for m in messages do
        printfn "Received: %s" m.Message.Value
} 

let cfg = KafkaConsumerConfig.Create("MyClientId", "kafka:9092", ["producer-test-command"], "fsharp-consumer", AutoOffsetReset.Earliest)

async {
    use consumer = BatchedConsumer.Start(log, cfg, handler)
    return! consumer.AwaitShutdown()
} |> Async.RunSynchronously