using System;

namespace DatingApp.API.Dtos
{
    public class UserForMessageReturnDto
    {
        public int SenderId { get; set; }    
        public string SenderPhotoUrl { get; set; }     
        public int RecipientId { get; set; }        
        public string Content { get; set; }        
        public DateTime MeassgeSent { get; set; }             
        public string SenderKnownAs { get; set; }
               
        public UserForMessageReturnDto()
        {
            MeassgeSent = DateTime.Now;
        }

    }
}