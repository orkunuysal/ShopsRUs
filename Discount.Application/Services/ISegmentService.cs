using Discount.Domain.Models;

namespace Discount.Application.Services
{
    public interface ISegmentService
    {
        Task<SegmentDTO> AddSegment(SegmentDTO segmentDTO);
    }
}