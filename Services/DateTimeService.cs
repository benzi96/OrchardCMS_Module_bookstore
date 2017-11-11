using System;

namespace bookstore.Services {
    public class DateTimeService : IDateTimeService {
        public DateTime Now {
            get { return DateTime.UtcNow; }
        }
    }
}