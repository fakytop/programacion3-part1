﻿using System;
using System.Collections.Generic;
using System.Text;
using LogicaNegocio.Entidades;

namespace LogicaNegocio.Interfaces
{
    public interface IRepositoryCountry: IRepository<Country>
    {
        public Country FindById(int id);
        public Country FindByISO(string iso);
        public IEnumerable<Country> FindByRegion(string region);
    }
}
