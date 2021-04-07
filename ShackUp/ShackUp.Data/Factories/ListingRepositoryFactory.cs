using System;
using ShackUp.Data.ADO;
using ShackUp.Data.Dapper;
using ShackUp.Data.Interfaces;

namespace ShackUp.Data.Factories
{
    public static class ListingRepositoryFactory
    {
        public static IListingRepo GetRepository()
        {
            switch (Settings.GetRepositoryType())
            {
                case "ADO":
                    return new ListingsRepoADO();
                case "Dapper":
                    return new ListingsRepoDapper();
                default:
                    throw new Exception("Could not find valid RepositoryType configuration value.");
            }
        }
    }
}