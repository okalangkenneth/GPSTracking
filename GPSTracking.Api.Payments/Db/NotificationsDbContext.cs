using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSTracking.Api.Notifications.Db
{
    public class NotificationsDbContext : DbContext
    {

        public NotificationsDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Notification> Notifications { get; set; }
    }
}
