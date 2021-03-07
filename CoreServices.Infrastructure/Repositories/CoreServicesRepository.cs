using CoreServices.Infrastructure.Repositories.context;
using CoreServices.Infrastructure.Repositories.DbModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreServices.Infrastructure.Repositories
{
    public interface ICoreServicesRepository
    {
        Task<List<PostDb>> GetPost();
        Task<List<CategoryDb>> GetCategory();
    }
    public class CoreServicesRepository: ICoreServicesRepository
    {
        private readonly CoreServicesDataContext _context;
        public CoreServicesRepository(CoreServicesDataContext context)
        {
            _context = context;
        }

        public async Task<List<PostDb>> GetPost()
        {
            var postResult = await (from p in _context.Post
                                    select new PostDb
                                    {
                                        POST_ID = p.POST_ID,
                                        CATEGORY_ID = p.CATEGORY_ID,
                                        CREATED_DATE = p.CREATED_DATE,
                                        DESCRIPTION = p.DESCRIPTION,
                                        TITLE = p.TITLE
                                    }).ToListAsync();
            return (List<PostDb>)postResult;
        }

        public async Task<List<CategoryDb>> GetCategory()
        {
            var categoryResult = await (from c in _context.Category
                                    select new CategoryDb
                                    {
                                        ID = c.ID,
                                        NAME = c.NAME,
                                        SLUG = c.SLUG
                                    }).ToListAsync();

            return (List<CategoryDb>)categoryResult;
        }
    }
}
