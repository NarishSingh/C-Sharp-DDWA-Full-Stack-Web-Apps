using System;
using ShackUp.Data.ADO;
using ShackUp.Data.Dapper;
using ShackUp.Data.Interfaces;

namespace ShackUp.Data.Factories
{
    public static class AccountRepositoryFactory
    {
        public static IAccountRepo GetRepository()
        {
            switch (Settings.GetRepositoryType())
            {
                case "ADO":
                    return new AccountRepoADO();
                case "Dapper":
                    return new AccountRepoDapper();
                default:
                    throw new Exception("Could not find valid RepositoryType configuration value.");
            }
        }
    }
}