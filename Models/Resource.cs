using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace courses_platform.Models
{
    public enum ResourceType
    {
        VIDEO,
        TEXT, // Markdown
    }
    public class Resource
    {
        public int Id { get; set; }
        public ResourceType Type { get; set; }
        public string Path { get; set; }
    }
}