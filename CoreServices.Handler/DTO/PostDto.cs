using System;
using System.Collections.Generic;
using System.Text;

namespace CoreServices.Handler.DTO
{
    public class PostDto
    {
        public int POST_ID { get; set; }
        public string TITLE { get; set; }
        public string DESCRIPTION { get; set; }
        public int CATEGORY_ID { get; set; }
        public DateTime CREATED_DATE { get; set; }
    }
}
