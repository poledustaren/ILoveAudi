using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILoveAudi.Models;

namespace ILoveAudi.Services
{
    public interface IHtmlParser
    {
        Task<List<Car>> SaveCarToDb();
        List<Car> Cars();
        void RemoveCar(int id);
    }
}
