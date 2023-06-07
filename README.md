# rust-kafka-demo

## Gettings started
- open a terminal and run `docker-compose up`
- open another terminal and navigate to `consumer-test` and run `cargo run`, this will start the consumer
- open another terminal and navigate to `producer-test` and run `cargo run`, this will produce one message

## components

Along with a docker-compose which sets up Kafka with Zookeeper we have;

### consumer-test
A sample kafka consumer using rust-kafka which polls the topic `producer-test-command` and prints each message

### producer-test
A sample kafka producer using rust kafka which produces messages to the topic `producer-test-command`
