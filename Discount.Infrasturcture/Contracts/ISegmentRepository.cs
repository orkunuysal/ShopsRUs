using Discount.Domain.Entities;
using Discount.Infrastructure.Contracts;

namespace Discount.Infrastructure.Repositories
{
    public interface ISegmentRepository
    {
        Task<Segment> AddSegment(Segment segment);
        Task<IEnumerable<Segment>> GetSegmentByDiscountRate(decimal discountRate);
        Task<Segment> GetSegmentByType(string type);
    }
}