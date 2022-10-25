﻿using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicaAccesoDatos.EF
{
    public class RepositoryMatch : IRepositoryMatch
    {
        public ObligatorioContext _db;

        public RepositoryMatch(ObligatorioContext db)
        {
            _db = db;
        }

        public void Add(Match obj)
        {
            IEnumerable <Match> matches = All();
            
            if(obj.Home.GroupStageId != obj.Away.GroupStageId)
            {
                throw new DomainException("Home and away must belong to the same group.");
            }

            if(obj.Home == obj.Away)
            {
                throw new DomainException("National Team cannot play against itself.");
            }
            foreach (var item in matches)
            {
                if((item.Home == obj.Home && item.Away == obj.Away) || (item.Home == obj.Away && item.Away == obj.Home))
                {
                    throw new DomainException("Match already exists.");
                }
                if(item.MatchDate.Value == obj.MatchDate.Value)
                {
                    throw new DomainException("Match date already scheduled.");
                }
            }

            
            try
            {
                obj.Validate();
                obj.GroupID = (int)obj.Home.GroupStageId;
                _db.Add(obj);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong, please try again later.");
            }
        }

        public IEnumerable<Match> All()
        {
            try
            {
                return _db.Match
                    .Include(th => th.Home)
                    .Include(ta => ta.Away)
                    .Include(m => m.MatchResult)
                    .Include(m => m.Group)
                    .OrderBy(d => d.MatchDate.Value);
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong, please try again later.");
            }
        }

        public Match FindById(int id)
        {
            try
            {
                Match m = _db.Match
                    .Include(ntH => ntH.Home)
                    .Include(ntA => ntA.Away)
                    .Include(mr => mr.MatchResult)
                    .FirstOrDefault(m => m.Id == id);

                if(m == null)
                {
                    throw new DomainException("Match does not exist.");
                }

                return m;
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong, please try again later.");
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Match obj)
        {
            throw new NotImplementedException();
        }
    }
}
