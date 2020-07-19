using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMe.API.Models
{
    public class ResultDto
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public int TotalSuccess { get; set; }
        public int TotalFailed { get; set; }
    }
}
