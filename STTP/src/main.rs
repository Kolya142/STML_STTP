use std::io::{Read, Write};
use std::{fs, str, thread};
use std::net::{TcpListener, TcpStream};

fn handle_client(mut stream: TcpStream) {
    let mut buffer = [0u8; 1024];
    let bytes_read = stream.read(&mut buffer).expect("Cannot Read Tcp Stream");
    let buffer = &buffer[0..bytes_read];
    /*
    STTP /
    Browser: BetterFly
     */
    if &buffer[..4] != [0x53, 0x54, 0x54, 0x50] {
        stream.write(&[0x34, 0x30, 0x30]).expect("Cannot Write Tcp Stream");
        return;
    }
    let buffer_str = str::from_utf8(&buffer);
    if buffer_str.is_err() {
        stream.write(&[0x34, 0x30, 0x30]).expect("Cannot Write Tcp Stream");
        return;
    }
    let buffer_str = buffer_str.unwrap();
    let rq = {
        let a: Vec<&str> = buffer_str.split('\n').collect();
        a[0]
    };

    let rq_url = {
        let a: Vec<&str> = rq.split(' ').collect();
        a[1]
    };
    let mut rs: Vec<u8> = b"Text: 404<gh> Page Not Found".to_vec();
    if rq_url == "/" {
        let data = fs::read("pages/index.stml");
        if data.is_err() {
            stream.write(&[0x34, 0x30, 0x34]).expect("Cannot Write Tcp Stream");
            return;
        }
        rs = data.unwrap();
    }
    if rs == b"Text: 404<gh> Page Not Found".to_vec() {
        stream.write(b"ERR").expect("Cannot Write Tcp Stream");
    }
    else {
        stream.write(b"OK").expect("Cannot Write Tcp Stream");
    }
    stream.write(b"\nSERVER: simple sttp server\n\n").expect("Cannot Write Tcp Stream");
    stream.write(&*rs).expect("Cannot Write Tcp Stream");
}

fn main() -> std::io::Result<()> {
    let listener = TcpListener::bind("127.0.0.1:80")?;

    // accept connections and process them serially
    for stream in listener.incoming() {
        match stream {
            Ok(stream) => {
                thread::spawn(|| {
                    handle_client(stream);
                });
            }
            Err(e) => eprintln!("Failed to accept a client: {:?}", e),
        }
    }
    Ok(())
}