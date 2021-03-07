using AutoMapper;
using CoreServices.Handler.DTO;
using CoreServices.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreServices.Handler
{
    public interface ICoreServiceHandler
    {
        Task<List<PostDto>> GetPost();
        Task<List<CategoryDto>> GetCategory();
    }
    public class CoreServiceHandler:ICoreServiceHandler
    {
        private readonly ICoreServicesRepository _coreServiceRepository;
        private readonly IMapper _mapper;
        public CoreServiceHandler(ICoreServicesRepository coreServicesRepository, IMapper mapper)
        {
            _coreServiceRepository = coreServicesRepository;
            _mapper = mapper;

        }

        public async Task<List<PostDto>> GetPost()
        {
            var result = await _coreServiceRepository.GetPost();
            return _mapper.Map<List<PostDto>>(result);
        }

        public async Task<List<CategoryDto>> GetCategory()
        {
            var result = await _coreServiceRepository.GetCategory();
            return _mapper.Map<List<CategoryDto>>(result);
        }
    }
}
