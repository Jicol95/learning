# kafka-various-lang

Interacting with Kafka with a bunch of languages

## Gettings started
- open a terminal and run `docker-compose up`
- open another terminal and navigate to `rust-consumer-test` and run `cargo run`, this will start the rust consumer
- open another terminal and navigate to `go-consumer-test` and run `go run .` this will start the go consumer
- open another terminal and navigate to `rust-producer-test` and run `cargo run`, this will produce one message

## components

Along with a docker-compose which sets up Kafka with Zookeeper we have;

### rust-consumer-test
A sample rust kafka consumer using rust-kafka which polls the topic `producer-test-command` and prints each message

### go-consumer-test
A sample go kafka consumer using confluent-kafka-go which polls the topic `producer-test-command` and prints each message

### rust-producer-test
A sample kafka producer using rust kafka which produces messages to the topic `producer-test-command`
