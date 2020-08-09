using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PhotoShare.Models
{
    public class PhotoVideoDBContext : DbContext
    {
        public DbSet<PhotoVideo> PhotoVideo { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<PhotoVideoTag> PhotoVideoTag { get; set; }
    }
}