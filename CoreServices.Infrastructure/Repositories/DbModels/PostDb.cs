using CoreServices.Infrastructure.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreServices.Infrastructure.Repositories.DbModels
{
    public class PostDb
    {
        public int POST_ID { get; set; }
        public string TITLE { get; set; }
        public string DESCRIPTION { get; set; }
        public int CATEGORY_ID { get; set; }
        public DateTime CREATED_DATE { get; set; }

        public static explicit operator PostDb(List<Post> v)
        {
            throw new NotImplementedException();
        }
    }
}
