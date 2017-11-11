using Orchard;
using System;

namespace bookstore.Services {
    public interface IDateTimeService : IDependency {
        DateTime Now { get; }
    }
}