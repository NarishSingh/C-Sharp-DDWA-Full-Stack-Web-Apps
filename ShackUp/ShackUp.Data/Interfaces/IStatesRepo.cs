using System.Collections.Generic;
using ShackUp.Models.Db;

namespace ShackUp.Data.Interfaces
{
    public interface IStatesRepo
    {
        /// <summary>
        /// Read all States from db
        /// </summary>
        /// <returns>List of State obj's</returns>
        List<State> ReadAllStates();
    }
}