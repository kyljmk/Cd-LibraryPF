using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cdLibrary.Api.Models;

namespace cdLibrary.Api.Data;

    public class CdContext : DbContext
    {
        public CdContext (DbContextOptions<CdContext> options)
            : base(options)
        {
        }

        public DbSet<Cd> Cd { get; set; }
        public DbSet<Genre> Genre { get; set; }
    } 
