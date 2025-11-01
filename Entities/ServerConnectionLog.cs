using System;
using LiteDB;

namespace FinalBeansStats {
    public class ServerConnectionLog {
        [BsonId(true)]
        public string SessionId { get; set; }
        public string ShowId { get; set; }
        public string ServerIp { get; set; }
        public DateTime ConnectionDate { get; set; }
        public bool IsNotify { get; set; }
        public bool IsPlaying { get; set; }
    }
}