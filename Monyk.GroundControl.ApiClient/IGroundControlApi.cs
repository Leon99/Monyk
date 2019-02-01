using System;
using System.Threading.Tasks;
using Monyk.GroundControl.Models;
using Refit;

namespace Monyk.GroundControl.ApiClient
{
    public interface IGroundControlApi
    {
        [Get("/monitors/{id}")]
        Task<Monitor> GetMonitorAsync(Guid id);
    }
}
