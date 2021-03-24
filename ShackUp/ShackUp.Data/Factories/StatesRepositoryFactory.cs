using System;
using ShackUp.Data.ADO;
using ShackUp.Data.Interfaces;

namespace ShackUp.Data.Factories
{
    public static class StatesRepositoryFactory
    {
        public static IStatesRepo GetRepository()
        {
            switch (Settings.GetRepositoryType())
            {
                case "ADO":
                    return new StatesRepoADO();
                default:
                    throw new Exception("Could not find valid RepositoryType configuration value.");
            }
        }
    }
}