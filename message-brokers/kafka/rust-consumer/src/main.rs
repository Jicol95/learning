use kafka::consumer::{Consumer, FetchOffset, GroupOffsetStorage};
use std::str::from_utf8;


fn main() {
    let mut consumer =
        Consumer::from_hosts(vec!("localhost:9092".to_owned()))
            .with_topic("producer-test-command".to_owned())
            .with_fallback_offset(FetchOffset::Earliest)
            .with_group("rust-consumer".to_owned())
            .with_offset_storage(GroupOffsetStorage::Kafka)
            .create()
            .unwrap();

    loop {
        for ms in consumer.poll().unwrap().iter() {
            for m in ms.messages() {
                println!("{}", Box::new(from_utf8(m.value).unwrap().to_owned()));
            }
            
            consumer.consume_messageset(ms);
        }
        consumer.commit_consumed().unwrap();
    }
}