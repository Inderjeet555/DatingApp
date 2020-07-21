using System;

namespace DatingApp.API.Dtos
{
    public class UserForMessageDto
    {       
        public int SenderId { get; set; }        
        public int RecipientId { get; set; }        
        public string Content { get; set; }        
        public DateTime MeassgeSent { get; set; } 
        public string SenderPhotoUrl { get; set; }       
        public string SenderKnownAs { get; set; }
               
        public UserForMessageDto()
        {
            MeassgeSent = DateTime.Now;
        }
    }
}