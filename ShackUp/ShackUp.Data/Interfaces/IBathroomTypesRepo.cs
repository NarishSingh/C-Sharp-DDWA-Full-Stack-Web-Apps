using System.Collections.Generic;
using ShackUp.Models.Db;

namespace ShackUp.Data.Interfaces
{
    public interface IBathroomTypesRepo
    {
        /// <summary>
        /// Read all Bathroom Types from db
        /// </summary>
        /// <returns>List of BathroomType obj's</returns>
        List<BathroomType> ReadAllBathroomTypes();
    }
}