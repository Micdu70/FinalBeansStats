using System;
using LiteDB;

namespace FinalBeansStats {
    public class PersonalBestLog {
        [BsonId(true)]
        public DateTime PbDate { get; set; }
        public string ShowId { get; set; }
        public string RoundId { get; set; }
        public double Record { get; set; }
        public bool IsPb { get; set; }
    }
}