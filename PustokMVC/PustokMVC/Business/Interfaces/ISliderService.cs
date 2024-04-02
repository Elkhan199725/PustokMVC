using PustokMVC.Models;
using System.Linq.Expressions;
using static PustokMVC.Business.Implementations.SliderService;

namespace PustokMVC.Business.Interfaces
{
    public interface ISliderService
    {
        Task<Slider> GetByIdAsync(int id);
        Task<Slider> GetSingleAsync(Expression<Func<Slider, bool>>? expression = null);
        Task<List<Slider>> GetAllAsync(Expression<Func<Slider, bool>>? expression = null, params string[] includes);
        Task<ServiceResult> CreateAsync(Slider slider);
        Task<ServiceResult> UpdateAsync(Slider slider);
        Task DeleteAsync(int id);
        Task SoftDeleteAsync(int id);
    }
}
