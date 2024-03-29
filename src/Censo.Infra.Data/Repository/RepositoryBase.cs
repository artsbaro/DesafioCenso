﻿using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Censo.Infra.Data.Repository
{
    public abstract class RepositoryBase
    {
        internal const string CONNECTIONSTRING_KEY = "DefaultConnection";

        private IDbConnection connection;
        internal IDbConnection Connection
        {
            get
            {
                return connection;
            }

            set
            {
                connection = value;
            }
        }

        public RepositoryBase(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(CONNECTIONSTRING_KEY);
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException("Connection string not found");
            Connection = new SqlConnection(connectionString);
        }

        //public IDbTransaction transaction { get; private set; }


        //public void SetTransaction(IDbTransaction transaction)
        //{
        //    this.transaction = transaction;
        //    connection = transaction.Connection;
        //}

        //internal void SetConnection(IDbConnection connection)
        //{
        //    this.connection = connection;
        //    transaction = null;
        //}
    }
}
