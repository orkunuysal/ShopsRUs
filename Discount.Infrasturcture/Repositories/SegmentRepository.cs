using Discount.Domain.Entities;
using Discount.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Discount.Infrastructure.Repositories
{
    public class SegmentRepository : ISegmentRepository
    {
        protected readonly CustomerInfoContext _dbContext;
        public SegmentRepository(CustomerInfoContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<Segment> GetSegmentByType(String type)
        {
            var segment = await _dbContext.Segment
                                .Where(o => o.Type == type)
                                .FirstOrDefaultAsync();
            return segment;
        }
        public async Task<IEnumerable<Segment>> GetSegmentByDiscountRate(Decimal discountRate)
        {
            var segmentList = await _dbContext.Segment
                                .Where(o => o.DiscountRate == discountRate)
                                .ToListAsync();
            return segmentList;
        }
        public async Task<Segment> AddSegment(Segment segment)
        {
            _dbContext.Set<Segment>().Add(segment);
            await _dbContext.SaveChangesAsync();
            return segment;
        }

    }
}
