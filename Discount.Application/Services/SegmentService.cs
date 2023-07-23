using AutoMapper;
using Discount.Domain.Entities;
using Discount.Domain.Models;
using Discount.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Services
{
    public class SegmentService : ISegmentService
    {
        private readonly ISegmentRepository _segmentRepository;
        private readonly IMapper _mapper;

        public SegmentService(ISegmentRepository segmentRepository, IMapper mapper)
        {
            _segmentRepository = segmentRepository;
            _mapper = mapper;
        }

        public async Task<SegmentDTO> AddSegment(SegmentDTO segmentDTO)
        {
            Segment segment = _mapper.Map<Segment>(segmentDTO);
            segment = await _segmentRepository.AddSegment(segment);
            return _mapper.Map<SegmentDTO>(segment);
        }
    }
}
