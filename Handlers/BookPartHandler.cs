using bookstore.Models;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookstore.Handlers
{
    public class BookPartHandler : ContentHandler
    {
        public BookPartHandler(IRepository<BookPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }
    }
}
