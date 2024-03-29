﻿using Censo.Domain.Entities;
using Censo.Domain.Filters;
using Censo.Domain.Interfaces.Repositories;
using Censo.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Censo.Domain.Services
{
    public class PessoaDomainService : IPessoaDomainService
    {
        private readonly IPessoaRepository _pessoaRepository;

        public PessoaDomainService(IPessoaRepository receitaRepository)
        {
            _pessoaRepository = receitaRepository;
        }

        public void Create(Pessoa entity)
        {
            using (var trans = new TransactionScope())
            {
                _pessoaRepository.Create(entity);
                foreach (var item in entity.Filhos ?? Enumerable.Empty<Pessoa>())
                {
                    item.SetParent(entity);
                    _pessoaRepository.Create(item);
                }
                trans.Complete();
            }
        }

        public Pessoa FindById(Guid id)
        {
            var pessoa = _pessoaRepository.FindById(id);
            pessoa.Filhos = _pessoaRepository.ListChildren(id);
            return pessoa;
        }

        public IEnumerable<Pessoa> ListChildren(Guid idParent)
        {
            return _pessoaRepository.ListChildren(idParent);
        }

        public IEnumerable<Pessoa> List(PessoaFilter filter)
        {
            return _pessoaRepository.List(filter);
        }

        public void Remove(Guid id)
        {
            _pessoaRepository.Remove(id);
        }

        public void Update(Pessoa entity)
        {
            using (var trans = new TransactionScope())
            {
                _pessoaRepository.Update(entity);
                foreach (var item in entity.Filhos ?? Enumerable.Empty<Pessoa>())
                {
                    item.SetParent(entity);
                    _pessoaRepository.Update(item);
                }
                trans.Complete();
            }
        }

        public decimal GetPercentPersonWhitNameByRegion(string region, string name)
        {
            return _pessoaRepository.GetPercentPersonWhitNameByRegion(region, name);
        }

        public Pessoa GetGenealogy(Guid id, byte level)
        {
            var pessoa = FindById(id);
            LoadGenealogy(pessoa, level);
            return pessoa;
        }

        private void LoadGenealogy(Pessoa pessoa, byte level)
        {
            if (level == 0)
                return;

            if (pessoa.HasChildren)
            {
                List<Pessoa> pessoas = pessoa.Filhos.ToList();

                for (int i = 0; i < pessoas.Count(); i++)
                {
                    pessoas[i] = FindById(pessoas[i].Id);
                    pessoa.Filhos = pessoas;
                    LoadGenealogy(pessoas[i], level--);
                }
            }
        }
    }
}

