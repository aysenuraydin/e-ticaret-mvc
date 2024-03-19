using System;
using Microsoft.EntityFrameworkCore;
using Eticaret.Application.Repositories;
using Eticaret.Domain;
using Eticaret.Persistence.Ef;
using Eticaret.Application.Abstract;

namespace Eticaret.Application.Concrete
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(EticaretDbContext dbContext) : base(dbContext)
        {

        }

    }
}

#nullable disable

#nullable restore