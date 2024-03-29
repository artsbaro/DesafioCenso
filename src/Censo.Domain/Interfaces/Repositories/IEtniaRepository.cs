﻿using Censo.Domain.Types;
using System.Collections.Generic;

namespace Censo.Domain.Interfaces.Repositories
{
    public interface IEtniaRepository : IRepository
    {
        byte Create(Etnia entity);
        IEnumerable<Etnia> List();
        Etnia FindById(byte id);

        void Update(Etnia entity);
    }
}
