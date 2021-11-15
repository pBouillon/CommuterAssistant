﻿using Assistant.Contracts.Entities;

using Microsoft.EntityFrameworkCore;

namespace Assistant.Bot.Core.Services
{
    public interface IApplicationContext
    {
        DbSet<User> Users { get; }
    }
}
