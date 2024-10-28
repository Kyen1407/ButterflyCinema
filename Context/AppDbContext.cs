using ButterflyCinema.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ButterflyCinema.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
