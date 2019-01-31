using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Monyk.GroundControl.Models;
using Refit;

namespace Monyk.GroundControl.ApiClient
{
    public interface IGroundControlApi
    {
        [Get("/monitors/{id}")]
        Task<IEnumerable<Monitor>> GetMonitor(Guid id);
    }
}
