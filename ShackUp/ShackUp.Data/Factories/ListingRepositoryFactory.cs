using System;
using ShackUp.Data.ADO;
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
                    throw new NotImplementedException("TODO");
                default:
                    throw new Exception("Could not find valid RepositoryType configuration value.");
            }
        }
    }
}