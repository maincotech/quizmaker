using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.Dtos
{
   public  class FirebaseSettingDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ProjectId { get; set; }
        [Required]
        public string JsonCredentials { get; set; }
    }
}
