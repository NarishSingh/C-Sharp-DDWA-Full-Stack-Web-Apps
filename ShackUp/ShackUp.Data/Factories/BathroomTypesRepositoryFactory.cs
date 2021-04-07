using System;
using ShackUp.Data.ADO;
using ShackUp.Data.Dapper;
using ShackUp.Data.Interfaces;

namespace ShackUp.Data.Factories
{
    public static class BathroomTypesRepositoryFactory
    {
        public static IBathroomTypesRepo GetRepository()
        {
            switch (Settings.GetRepositoryType())
            {
                case "ADO":
                    return new BathroomTypesRepoADO();
                case "Dapper":
                    return new BathroomTypesRepoDapper();
                default:
                    throw new Exception("Could not find valid RepositoryType configuration value.");
            }
        }
    }
}